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
            Log.info("WriteDryCost: Started");

            {  // Toe Stomping Fest prevention
                for (int i = WAIT_ROUNDS; i >= 0 && null == PartLoader.LoadedPartsList; --i)
                {
                    yield return null;
                    if (0 == i) Log.warn("Timeout waiting for PartLoader.LoadedPartsList!!");
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
                        if (0 == i) Log.warn("Timeout waiting for PartLoader.LoadedPartsList.Count!!");
                    }
    			 }
            }
            
            int check_failures = 0;
            int sanity_failures = 0;
            int showstoppers_failures = 0;
            int check_overrulled = 0;
           
            foreach (AvailablePart p in PartLoader.LoadedPartsList)
            {
                for (int i = WAIT_ROUNDS; i >= 0 && (null == p.partPrefab || null == p.partPrefab.Modules); --i)
                {
                    yield return null;
                    if (0 == i) Log.error("Timeout waiting for {0}.prefab.Modules!!", p.name);
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
                        Log.error("Exception on {0}.prefab.Modules.Contains: {1}", p.name, culprit);
                        Log.detail("{0}", prefab.Modules);
                        continue;
                    }

                    if (!containsTweakScale)
                        continue;

                    // End of hack. Ugly, uh? :P
                }
#if DEBUG
                {
                    Log.dbg("Found part named {0}. title {1}:", p.name, p.title);
                    foreach (PartModule m in prefab.Modules)
                        Log.dbg("\tPart {0} has module {1}", p.name, m.moduleName);
                }
#endif
                try {
                    string r = null;
                    
                    // We check for fixable problems first, in the hope to prevent by luck a ShowStopper below.
                    // These Offending Parts never worked before, or always ends in crashing KSP, so the less worse
                    // line of action is to remove TweakScale from them in order to allow the player to at least keep
                    // playing KSP. Current savegames can break, but they were going to crash and loose everything anyway!!
                    if (null != (r = this.checkForSanity(prefab)))
                    {   // There are some known situations where TweakScale is capsizing. If such situations are detected, we just
                        // refuse to scale it. Sorry.
                        Log.warn("Removing TweakScale support for {0}.", p.name);
                        prefab.Modules.Remove(prefab.Modules["TweakScale"]);
                        Log.error("Part {0} didn't passed the sanity check due {1}.", p.name, r);
                        ++sanity_failures;
                        continue;
                    }
                    
                    // This one is for my patches that "break things again" in a controlled way to salvage already running savegames
                    // that would be lost by fixing things right. Sometimes, it's possible to keep the badly patched parts ongoing, as
                    // as the nastiness will not crash KSP (besides still corrupting savegames and craft files in a way that would not
                    // allow the user to share them).
                    // Since we are overruling the checks, we abort the remaining ones. Yes, this allows abuse, but whatever... I can't
                    // save the World, just the savegames. :)
                    if (null != (r = this.checkForOverules(prefab)))
                    {   // This is for detect and log the Breaking Parts patches.
                        // See issue [#56]( https://github.com/net-lisias-ksp/TweakScale/issues/56 ) for details.
                        // This is **FAR** from a good measure, but it's the only viable.
                        Log.warn("Part {0} has the issue overrule {1}.", p.name, r);
                        ++check_overrulled;
                    }
                    // And now we check for the ShowStoppers.
                    // These ones happens due rogue patches, added after a good installment could starts savegames, what ends up corrupting them!
                    // Since we don't have how to know when this happens, and since originally the part was working fine, we don't know
                    // how to proceeed. So the only sensible option is to scare the user enough to make him/her go to the Forum for help
                    // so we can identify the offending patch and then provide a solution that would preserve his savegame.
                    // We also stops any further processing, as we could damage something that is already damaged.
                    else if (null != (r = this.checkForShowStoppers(prefab)))
                    {   // This are situations that we should not allow the KSP to run to prevent serious corruption.
                        // This is **FAR** from a good measure, but it's the only viable.
                        Log.warn("**FATAL** Found a showstopper problem on {0}.", p.name);
                        Log.error("**FATAL** Part {0} has a fatal problem due {1}.", p.name, r);
                        ++showstoppers_failures;
                        continue;
                    }                    

                }
                catch (Exception e)
                {
                    ++check_failures;
                    Log.error("part={0} ({1}) Exception on Sanity Checks: {2}", p.name, p.title, e);
                }

                // If we got here, the part is good to go, or was overulled into a sane configuration that would allow us to proceed.
				
                try
                {
					TweakScale m = prefab.Modules["TweakScale"] as TweakScale;
                    m.DryCost = (float)(p.cost - prefab.Resources.Cast<PartResource>().Aggregate(0.0, (a, b) => a + b.maxAmount * b.info.unitCost));
                    m.ignoreResourcesForCost |= prefab.Modules.Contains("FSfuelSwitch");

                    if (m.DryCost < 0)
                    {
                        Log.error("PrefabDryCostWriter: negative dryCost: part={0}, DryCost={1}", p.name, m.DryCost);
                        m.DryCost = 0;
                    }
                    Log.dbg("Part {0} has drycost {1} with ignoreResourcesForCost {2}", p.name, m.DryCost, m.ignoreResourcesForCost);
                }
                catch (Exception e)
                {
                    ++check_failures;
                    Log.error("part={0} ({1}) Exception on writeDryCost: {2}", p.name, p.title, e);
                }
            }
            Log.info("TweakScale::WriteDryCost: Concluded : {0} checks failed ; {1} parts with issues overruled ; {2} Show Stoppers found; {3} Sanity Check failed;", check_failures, check_overrulled, showstoppers_failures, sanity_failures);
            PrefabDryCostWriter.isConcluded = true;
            
            if (showstoppers_failures > 0)
            {
                GUI.ShowStopperAlertBox.Show(showstoppers_failures);
            }
            else
            {
                if (check_overrulled > 0)   GUI.OverrulledAdviseBox.show(check_overrulled);
                if (sanity_failures > 0)    GUI.SanityCheckAlertBox.show(sanity_failures);
                if (check_failures > 0)     GUI.CheckFailureAlertBox.show(check_failures);
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
                        if (basket.HasValue("ISSUE_OVERRULE")) continue;
                        if (1 != basket.GetValues(property.name).Length)
                            return "having duplicated properties - see issue [#34]( https://github.com/net-lisias-ksp/TweakScale/issues/34 )";
                    }
                }
            }
            
            return null;
        }

        private string checkForOverules(Part p)
        {
            {
                ConfigNode part = GameDatabase.Instance.GetConfigNode(p.partInfo.partUrl);
                foreach (ConfigNode basket in part.GetNodes("MODULE"))
                {
                    if ("TweakScale" != basket.GetValue("name")) continue;
                    foreach (ConfigNode.Value property in basket.values)
                    {
                        if (basket.HasValue("ISSUE_OVERRULE"))
                            return String.Format("{0}. See [#56]( https://github.com/net-lisias-ksp/TweakScale/issues/56 )", basket.GetValue("ISSUE_OVERRULE"));
                    }
                }
            }
            
            return null;
        }
	}
}
