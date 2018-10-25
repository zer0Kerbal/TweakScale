using TweakScale.Annotations;
using UnityEngine;
using System.Linq;
using System;

namespace TweakScale
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    internal class PrefabDryCostWriter : SingletonBehavior<PrefabDryCostWriter>
    {
        [UsedImplicitly]
        private void Start()
        {
            WriteDryCost();
        }

        private void WriteDryCost()
        {
            Debug.Log("TweakScale::WriteDryCost: Started");
            foreach (AvailablePart p in PartLoader.LoadedPartsList)
            {
				Part prefab = p.partPrefab;
                if (prefab == null)
                {
                    Tools.LogWf("partPrefab is null: " + p.name);
                    continue;
                }
                try
                {
                    if (prefab.Modules == null)
                    {
                        Tools.LogWf("partPrefab.Modules is null: " + p.name);
                        continue;
                    }
                    if (!prefab.Modules.Contains("TweakScale"))
                        continue;

					TweakScale m = prefab.Modules["TweakScale"] as TweakScale;
                    m.DryCost = (float)(p.cost - prefab.Resources.Cast<PartResource>().Aggregate(0.0, (a, b) => a + b.maxAmount * b.info.unitCost));
                    if (prefab.Modules.Contains("FSfuelSwitch"))
                        m.ignoreResourcesForCost = true;

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
                    Debug.LogErrorFormat("[TweakScale] part={0} ({1}) Exception on writeDryCost: {2}", p.name, p.title, e);
                }
            }
            Debug.Log("TweakScale::WriteDryCost: Concluded");
        }
    }
}
