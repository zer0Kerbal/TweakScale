using System;
using System.Collections;
using System.Linq;
using System.Reflection;

using UnityEngine;
using TweakScale.Annotations;

namespace TweakScale
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    internal class PrefabDryCostWriter : SingletonBehavior<PrefabDryCostWriter>
    {
		private static readonly int WAIT_ROUNDS = 120; // @60fps, would render 2 secs.
        
		internal static bool isConcluded = false;
        
        [UsedImplicitly]
        private void Start()
        {
            StartCoroutine("WriteDryCost");
        }

        private IEnumerator WriteDryCost()
        {
            PrefabDryCostWriter.isConcluded = false;
            Debug.Log("TweakScale::WriteDryCost: Started");

            {  // Toe Stomping Fest prevention
                for (int i = WAIT_ROUNDS; i >= 0 && null == PartLoader.LoadedPartsList; --i)
                {
                    yield return null;
                    if (0 == i) Debug.LogError("TweakScale::Timeout waiting for PartLoader.LoadedPartsList!!");
                }
    
    			 // I Don't know if this is needed, but since I don't know that this is not needed,
    			 // I choose to be safe than sorry!
                {
                    int last_count = int.MinValue;
    			    for (int i = WAIT_ROUNDS; i >= 0; --i)
    				{
                        if (last_count == PartLoader.LoadedPartsList.Count) break;
    					last_count = PartLoader.LoadedPartsList.Count;
                        yield return null;
                        if (0 == i) Debug.LogError("TweakScale::Timeout waiting for PartLoader.LoadedPartsList.Count!!");
    				}
    			 }
            }
            
            int check_failures = 0;
            int sanity_failures = 0;
            int showstoppers_failures = 0;
           
            foreach (AvailablePart p in PartLoader.LoadedPartsList)
            {
                for (int i = WAIT_ROUNDS; i >= 0 && (null == p.partPrefab || null == p.partPrefab.Modules || p.partPrefab.Modules.Count < 1); --i)
                {
                    yield return null;
                    if (0 == i) Debug.LogErrorFormat("TweakScale::Timeout waiting for {0}.prefab.Modules!!", p.name);
                }
              
                Part prefab;
                { 
                    // Historically, we had problems here.
                    // However, that co-routine stunt appears to have solved it.
                    // But we will keep this as a ghinea-pig in the case the problem happens again.
                    int retries = WAIT_ROUNDS;
                    bool containsTweakScale = false;
                    Exception culprit = null;
                    
                    prefab = p.partPrefab; // Reaching the prefab here in the case another Mod recreates it from zero. If such hypothecical mod recreates the whole part, we're doomed no matter what.
                    
                    while (retries > 0)
                    { 
                        bool should_yield = false;
                        try 
                        {
                            containsTweakScale = prefab.Modules.Contains("TweakScale"); // Yeah. This while stunt was done just due this. All the rest is plain clutter! :D 
                            break;
                        }
                        catch (Exception e)
                        {
                            culprit = e;
                            --retries;
                            should_yield = true;
                        }
                        if (should_yield) // This stunt is needed as we can't yield from inside a try-catch!
                            yield return null;
                    }

                    if (0 == retries)
                    {
                        Debug.LogErrorFormat("[TweakScale] Exception on {0}.prefab.Modules.Contains: {1}", p.name, culprit);
                        Debug.LogWarningFormat("{0}", prefab.Modules);
                        continue;
                    }

                    if (!containsTweakScale)
                        continue;

                    // End of hack. Ugly, uh? :P
                }
#if DEBUG
                {
                    Debug.LogFormat("Found part named {0}. title {1}:", p.name, p.title);
                    foreach (PartModule m in prefab.Modules)
                        Debug.LogFormat("\tPart {0} has module {1}", p.name, m.moduleName);
                }
#endif
                try {
                    string r = null;
                    // We check for fixable problems first, in the hope to prevent by luck a ShowStopper below.
                    if (null != (r = this.checkForSanity(prefab)))
    				{   // There are some known situations where TweakScale is capsizing. If such situations are detected, we just
                        // refuse to scale it. Sorry.
                        Debug.LogWarningFormat("[TweakScale] Removing TweakScale support for {0}.", p.name);
                        prefab.Modules.Remove(prefab.Modules["TweakScale"]);
                        Debug.LogErrorFormat("[TweakScale] Part {0} didn't passed the sanity check due {1}.", p.name, r);
                        ++sanity_failures;
                        continue;
					}
                    
                    if (null != (r = this.checkForShowStoppers(prefab)))
                    {   // This are situations that we should not allow the KSP to run to prevent serious corruption.
                        // This is **FAR** from a good measure, but it's the only viable.
                        Debug.LogWarningFormat("[TweakScale] **FATAL** Found a showstopper problem on {0}.", p.name);
                        prefab.Modules.Remove(prefab.Modules["TweakScale"]);
                        Debug.LogErrorFormat("[TweakScale] **FATAL** Part {0} has a fatal problem due {1}.", p.name, r);
                        ++showstoppers_failures;
                        continue;
                    }
                }
                catch (Exception e)
                {
                    ++check_failures;
                    Debug.LogErrorFormat("[TweakScale] part={0} ({1}) Exception on Sanity Checks: {2}", p.name, p.title, e);
                }

				try
                {
					TweakScale m = prefab.Modules["TweakScale"] as TweakScale;
                    m.DryCost = (float)(p.cost - prefab.Resources.Cast<PartResource>().Aggregate(0.0, (a, b) => a + b.maxAmount * b.info.unitCost));
					m.ignoreResourcesForCost |= prefab.Modules.Contains("FSfuelSwitch");

                    if (m.DryCost < 0)
                    {
                        Debug.LogErrorFormat("TweakScale::PrefabDryCostWriter: negative dryCost: part={0}, DryCost={1}", p.name, m.DryCost);
                        m.DryCost = 0;
                    }
#if DEBUG
					Debug.LogFormat("Part {0} has drycost {1} with ignoreResourcesForCost {2}", p.name, m.DryCost, m.ignoreResourcesForCost);
#endif
                }
                catch (Exception e)
                {
                    ++check_failures;
                    Debug.LogErrorFormat("[TweakScale] part={0} ({1}) Exception on writeDryCost: {2}", p.name, p.title, e);
                }
            }
            Debug.LogFormat("TweakScale::WriteDryCost: Concluded : {0} checks failed ; {1} Show Stoppers found; {2} Sanity Check failed;", check_failures, showstoppers_failures, sanity_failures);
            PrefabDryCostWriter.isConcluded = true;
            
            if (showstoppers_failures > 0)
            {
                GUI.ShowStopperAlertBox.Show(showstoppers_failures);
            }
            else if (check_failures > 0 || sanity_failures > 0)
            {
                GUI.SanityCheckAlertBox.show(sanity_failures, check_failures);
            }
        }
        
        private string checkForSanity(Part p)
		{
            {
                TweakScale m = p.Modules.GetModule<TweakScale>();
                if (m.Fields["tweakScale"].guiActiveEditor == m.Fields["tweakName"].guiActiveEditor)
                    return "not being correctly initialized - see issue [#30]( https://github.com/net-lisias-ksp/TweakScale/issues/30 )";
            }
            
            if (p.Modules.Contains("ModulePartVariants"))
			{
				PartModule m = p.Modules["ModulePartVariants"];
                foreach(FieldInfo fi in m.ModuleAttributes.publicFields)
				{
                    if("variantList" != fi.Name) continue;
                    IList variantList = (IList)fi.GetValue(m);
                    foreach (object partVariant in variantList)
					    foreach (PropertyInfo property in partVariant.GetType().GetProperties())
                        { 
						    if ("Cost" == property.Name && 0.0 != (float)property.GetValue(partVariant, null))
                                return "having a ModulePartVariants with Cost - see issue [#13]( https://github.com/net-lisias-ksp/TweakScale/issues/13 )";                                        
                            if ("Mass" == property.Name && 0.0 != (float)property.GetValue(partVariant, null))
                                return "having a ModulePartVariants with Mass - see issue [#13]( https://github.com/net-lisias-ksp/TweakScale/issues/13 )";                                        
						}
				}
			}
            if (p.Modules.Contains("FSbuoyancy"))
                return "using FSbuoyancy module - see issue [#9]( https://github.com/net-lisias-ksp/TweakScale/issues/9 )";

            if (p.Modules.Contains("ModuleB9PartSwitch"))
			{
                if (p.Modules.Contains("FSfuelSwitch"))
                    return "having ModuleB9PartSwitch together FSfuelSwitch - see issue [#12]( https://github.com/net-lisias-ksp/TweakScale/issues/12 )";
                if (p.Modules.Contains("ModuleFuelTanks"))
                    return "having ModuleB9PartSwitch together ModuleFuelTanks - see issue [#12]( https://github.com/net-lisias-ksp/TweakScale/issues/12 )";
			}

			return null;
		}
        
        private string checkForShowStoppers(Part p)
        {
            {
                ConfigNode part = GameDatabase.Instance.GetConfigNode(p.partInfo.partUrl);
                foreach (ConfigNode basket in part.GetNodes("MODULE"))
                {
                    if ("TweakScale" != basket.GetValue("name")) continue;                      
                    foreach (ConfigNode.Value property in basket.values)
                    {
                        if (1 != basket.GetValues(property.name).Length)
                            return "having duplicated properties - see issue [#34]( https://github.com/net-lisias-ksp/TweakScale/issues/34 )";
                    }
                }
            }
            
            return null;
        }
	}
}
