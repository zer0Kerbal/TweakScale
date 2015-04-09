using KSPAPIExtensions;
using System;
using System.Linq;
using TweakScale.Annotations;
using UnityEngine;

namespace TweakScale
{
    /// <summary>
    /// Converts from Gaius' GoodspeedTweakScale to updated TweakScale.
    /// </summary>
    public class GoodspeedTweakScale : TweakScale
    {
        private bool _updated;

        protected override void Setup()
        {
            base.Setup();
            if (_updated)
                return;
            tweakName = (int)tweakScale;
            tweakScale = ScaleFactors[tweakName];
            _updated = true;
        }
    }

    public class TweakScale : PartModule, IPartCostModifier
    {
        /// <summary>
        /// The selected scale. Different from currentScale only for destination single update, where currentScale is set to match this.
        /// </summary>
        [KSPField(isPersistant = false, guiActiveEditor = true, guiName = "Scale", guiFormat = "0.000", guiUnits = "m")]
        [UI_ScaleEdit(scene = UI_Scene.Editor)]
// ReSharper disable once InconsistentNaming
        public float tweakScale = -1;

        /// <summary>
        /// Index into scale values array.
        /// </summary>
        [KSPField(isPersistant = false, guiActiveEditor = true, guiName = "Scale")]
        [UI_ChooseOption(scene = UI_Scene.Editor)]
// ReSharper disable once InconsistentNaming
        public int tweakName = 0;

        /// <summary>
        /// The scale to which the part currently is scaled.
        /// </summary>
        [KSPField(isPersistant = true)]
// ReSharper disable once InconsistentNaming
        public float currentScale = -1;

        /// <summary>
        /// The default scale, i.e. the number by which to divide tweakScale and currentScale to get the relative size difference from when the part is used without TweakScale.
        /// </summary>
        [KSPField(isPersistant = true)]
// ReSharper disable once InconsistentNaming
        public float defaultScale = -1;

        /// <summary>
        /// Whether the part should be freely scalable or limited to destination list of allowed values.
        /// </summary>
        [KSPField(isPersistant = false)]
// ReSharper disable once InconsistentNaming
        public bool isFreeScale = false;

        /// <summary>
        /// The scale exponentValue array. If isFreeScale is false, the part may only be one of these scales.
        /// </summary>
        protected float[] ScaleFactors = { 0.625f, 1.25f, 2.5f, 3.75f, 5f };
        
        /// <summary>
        /// The node scale array. If node scales are defined the nodes will be resized to these values.
        ///</summary>
        protected int[] ScaleNodes = { };

        /// <summary>
        /// The unmodified prefab part. From this, default values are found.
        /// </summary>
        private Part _prefabPart;

        /// <summary>
        /// Like currentScale above, this is the current scale vector. If TweakScale supports non-uniform scaling in the future (e.g. changing only the length of destination booster), savedScale may represent such destination scaling, while currentScale won't.
        /// </summary>
        private Vector3 _savedScale;

        /// <summary>
        /// The exponentValue by which the part is scaled by default. When destination part uses MODEL { scale = ... }, this will be different from (1,1,1).
        /// </summary>
        [KSPField(isPersistant = true)]
// ReSharper disable once InconsistentNaming
        public Vector3 defaultTransformScale = new Vector3(0f, 0f, 0f);


        //[KSPField(isPersistant = true)]
        private bool _firstUpdateWithParent = true;
        private bool _setupRun;
        private bool _invalidCfg;

        /// <summary>
        /// Updaters for different PartModules.
        /// </summary>
        private IRescalable[] _updaters = new IRescalable[0];

        private enum Tristate
        {
            True,
            False,
            Unset
        }

        public bool IsRescaled()
        {
            return (Math.Abs(currentScale/defaultScale -1f) > 1e-5f);
        }

        /// <summary>
        /// Whether this instance of TweakScale is the first. If not, log an error and make sure the TweakScale modules don't harmfully interact.
        /// </summary>
        private Tristate _duplicate = Tristate.Unset;

        /// <summary>
        /// The ScaleType for this part.
        /// </summary>
        public ScaleType ScaleType { get; private set; }

        /// <summary>
        /// Cost of unscaled, empty part.
        /// </summary>
        [KSPField(isPersistant = true)]
        public float DryCost;

        private Hotkeyable _chainingEnabled;
        private Hotkeyable _autoscaleEnabled;

        /// <summary>
        /// The ConfigNode that belongs to the part this modules affects.
        /// </summary>
        private ConfigNode PartNode
        {
            get
            {
                return GameDatabase.Instance.GetConfigs("PART").Single(c => c.name.Replace('_', '.') == part.partInfo.name)
                    .config;
            }
        }

        /// <summary>
        /// The ConfigNode that belongs to this module.
        /// </summary>
        public ConfigNode ModuleNode
        {
            get
            {
                return PartNode.GetNodes("MODULE").FirstOrDefault(n => n.GetValue("name") == moduleName);
            }
        }

        /// <summary>
        /// The current scaling factor.
        /// </summary>
        public ScalingFactor ScalingFactor
        {
            get
            {
                return new ScalingFactor(tweakScale / defaultScale, tweakScale / currentScale, isFreeScale ? -1 : tweakName);
            }
        }

        /// <summary>
        /// Loads settings from <paramref name="scaleType"/>.
        /// </summary>
        /// <param name="scaleType">The settings to use.</param>
        private void SetupFromConfig(ScaleType scaleType)
        {
            isFreeScale = scaleType.IsFreeScale;
            if (defaultScale == -1)
                defaultScale = scaleType.DefaultScale;

            if (currentScale == -1)
                currentScale = defaultScale;
            else if (defaultScale != scaleType.DefaultScale)
            {
                Tools.Logf("defaultScale has changed for part {0}: keeping relative scale.", part.name);
                currentScale *= scaleType.DefaultScale / defaultScale;
                defaultScale = scaleType.DefaultScale;
            }

            if (tweakScale == -1)
                tweakScale = currentScale;
            Fields["tweakScale"].guiActiveEditor = false;
            Fields["tweakName"].guiActiveEditor = false;
            ScaleFactors = scaleType.ScaleFactors;
            if (ScaleFactors.Length <= 0)
                return;

            if (isFreeScale)
            {
                Fields["tweakScale"].guiActiveEditor = true;
                var range = (UI_ScaleEdit)Fields["tweakScale"].uiControlEditor;
                range.intervals = scaleType.ScaleFactors;
                range.incrementSlide = scaleType.IncrementSlide;
                Fields["tweakScale"].guiUnits = scaleType.Suffix;
            }
            else
            {
                Fields["tweakName"].guiActiveEditor = scaleType.ScaleFactors.Length > 1;
                var options = (UI_ChooseOption)Fields["tweakName"].uiControlEditor;
                ScaleNodes = scaleType.ScaleNodes;
                options.options = scaleType.ScaleNames;
            }
        }

        /// <summary>
        /// Sets up values from ScaleType, creates updaters, and sets up initial values.
        /// </summary>
        protected virtual void Setup()
        {
            if (part.partInfo == null)
            {
                return;
            }

            if (_setupRun)
            {
                return;
            }

            _prefabPart = PartLoader.getPartInfoByName(part.partInfo.name).partPrefab;

            _updaters = TweakScaleUpdater.CreateUpdaters(part).ToArray();

            var doUpdate = currentScale < 0f;
            SetupFromConfig(ScaleType = new ScaleType(ModuleNode));

            if (doUpdate)
            {
                tweakScale = currentScale = defaultScale;
                DryCost = (float)(part.partInfo.cost - _prefabPart.Resources.Cast<PartResource>().Aggregate(0.0, (a, b) => a + b.maxAmount * b.info.unitCost));
                if (DryCost < 0)
                {
                    DryCost = 0;
                }
            }

            if (!isFreeScale && ScaleFactors.Length != 0)
            {
                tweakName = Tools.ClosestIndex(tweakScale, ScaleFactors);
                tweakScale = ScaleFactors[tweakName];
            }

            if (!doUpdate && IsRescaled())
            {
                UpdateByWidth(false, true);
                foreach (var updater in _updaters)
                {
                    updater.OnRescale(ScalingFactor);
                }
            }
            _setupRun = true;
        }


        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if (part.parent != null)
            {
                _firstUpdateWithParent = false;
            }
            Setup();

            if (HighLogic.LoadedSceneIsEditor)
            {
                _autoscaleEnabled = HotkeyManager.Instance.AddHotkey("Autoscale", new[] {KeyCode.LeftShift},
                    new[] {KeyCode.LeftControl, KeyCode.L}, false);
                _chainingEnabled = HotkeyManager.Instance.AddHotkey("Scale chaining", new[] {KeyCode.LeftShift},
                    new[] {KeyCode.LeftControl, KeyCode.K}, false);
            }
        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            Setup();
        }

        /// <summary>
        /// Moves <paramref name="node"/> to reflect the new scale. If <paramref name="movePart"/> is true, also moves attached parts.
        /// </summary>
        /// <param name="node">The node to move.</param>
        /// <param name="baseNode">The same node, as found on the prefab part.</param>
        /// <param name="movePart">Whether or not to move attached parts.</param>
        /// <param name="absolute">Whether to use absolute or relative scaling.</param>
        private void MoveNode(AttachNode node, AttachNode baseNode, bool movePart, bool absolute)
        {
            if (baseNode == null)
            {
                baseNode = node;
                absolute = false;
            }

            var oldPosition = node.position;

            if (absolute)
                node.position = baseNode.position * ScalingFactor.absolute.linear;
            else
                node.position = node.position * ScalingFactor.relative.linear;

            var deltaPos = node.position - oldPosition;

            if (movePart && node.attachedPart != null)
            {
                if (node.attachedPart == part.parent)
                {
                    part.transform.Translate(-deltaPos, part.transform);
                }
                else
                {
                    var offset = node.attachedPart.attPos*(ScalingFactor.relative.linear - 1);
                    node.attachedPart.transform.Translate(deltaPos + offset, part.transform);
                    node.attachedPart.attPos *= ScalingFactor.relative.linear;
                }
            }
            RescaleNode(node, baseNode);
        }

        /// <summary>
        /// Change the size of <paramref name="node"/> to reflect the new size of the part it's attached to.
        /// </summary>
        /// <param name="node">The node to resize.</param>
        /// <param name="baseNode">The same node, as found on the prefab part.</param>
        private void RescaleNode(AttachNode node, AttachNode baseNode)
        {
            if (isFreeScale || ScaleNodes == null || ScaleNodes.Length == 0)
            {
                float tmpBaseNodeSize = baseNode.size;
                if (tmpBaseNodeSize == 0)
                {
                    tmpBaseNodeSize = 0.5f;
                }
                node.size = (int)(tmpBaseNodeSize * tweakScale / defaultScale +0.49);
            }
            else
            {
           		node.size = baseNode.size + (1 * ScaleNodes[tweakName]);
            }
            if (node.size < 0)
            {
                node.size = 0;
            }
        }

        /// <summary>
        /// Updates properties that change linearly with scale.
        /// </summary>
        /// <param name="moveParts">Whether or not to move attached parts.</param>
        /// <param name="absolute">Whether to use absolute or relative scaling.</param>
        private void UpdateByWidth(bool moveParts, bool absolute)
        {
            if (defaultTransformScale.x == 0.0f)
            {
                defaultTransformScale = part.transform.GetChild(0).localScale;
            }

            _savedScale = part.transform.GetChild(0).localScale = ScalingFactor.absolute.linear * defaultTransformScale;
            part.transform.GetChild(0).hasChanged = true;
            part.transform.hasChanged = true;

            foreach (var node in part.attachNodes)
            {
                var nodesWithSameId = part.attachNodes
                    .Where(a => a.id == node.id)
                    .ToArray();
                var idIdx = Array.FindIndex(nodesWithSameId, a => a == node);
                var baseNodesWithSameId = _prefabPart.attachNodes
                    .Where(a => a.id == node.id)
                    .ToArray();
                if (idIdx < baseNodesWithSameId.Length)
                {
                    var baseNode = baseNodesWithSameId[idIdx];

                    MoveNode(node, baseNode, moveParts, absolute);
                }
                else
                {
                    Tools.LogWf("Error scaling part. Node {0} does not have counterpart in base part.", node.id);
                }
            }

            if (part.srfAttachNode != null)
            {
                MoveNode(part.srfAttachNode, _prefabPart.srfAttachNode, moveParts, absolute);
            }
            if (moveParts)
            {
                foreach (var child in part.children)
                {
                    if (child.srfAttachNode == null || child.srfAttachNode.attachedPart != part)
                        continue;
                    var attachedPosition = child.transform.localPosition + child.transform.localRotation * child.srfAttachNode.position;
                    var targetPosition = attachedPosition * ScalingFactor.relative.linear;
                    child.transform.Translate(targetPosition - attachedPosition, part.transform);
                }
            }
        }

        /// <summary>
        /// Whether the part holds any resources (fuel, electricity, etc).
        /// </summary>
        private bool HasResources
        {
            get
            {
                return part.Resources.Count > 0;
            }
        }

        /// <summary>
        /// Marks the right-click window as dirty (i.e. tells it to update).
        /// </summary>
        private void UpdateWindow() // redraw the right-click window with the updated stats
        {
            if (isFreeScale || !HasResources)
                return;
            foreach (var win in FindObjectsOfType<UIPartActionWindow>().Where(win => win.part == part))
            {
                // This causes the slider to be non-responsive - i.e. after you click once, you must click again, not drag the slider.
                win.displayDirty = true;
            }
        }

        /// <summary>
        /// Find the Attachnode that fastens <paramref name="a"/> to <paramref name="b"/> and vice versa.
        /// </summary>
        /// <param name="a">The source part (often the parent)</param>
        /// <param name="b">The target part (often the child)</param>
        /// <returns>The AttachNodes between the two parts.</returns>
        private static Tuple<AttachNode, AttachNode>? NodesBetween(Part a, Part b)
        {
            var nodeA = a.findAttachNodeByPart(b);
            var nodeB = b.findAttachNodeByPart(a);

            if (nodeA == null || nodeB == null)
                return null;

            return Tuple.Create(nodeA, nodeB);
        }
                
        /// <summary>
        /// Calculate the correct scale to use for scaling a part relative to another.
        /// </summary>
        /// <param name="a">Source part, from which we get the desired scale.</param>
        /// <param name="b">Target part, which will potentially be scaled.</param>
        /// <returns>The difference in scale between the (scaled) attachment nodes connecting <paramref name="a"/> and <paramref name="b"/>, or null if somethinng went wrong.</returns>
        private static float? GetRelativeScaling(TweakScale a, TweakScale b)
        {
            if (a == null || b == null)
                return null;

            var nodes = NodesBetween(a.part, b.part);

            if (!nodes.HasValue)
                return null;
            
            var nodeA = nodes.Value.Item1;
            var nodeB = nodes.Value.Item2;

            var aIdx = a._prefabPart.attachNodes.FindIndex( t => t.id == nodeA.id);
            var bIdx = b._prefabPart.attachNodes.FindIndex( t => t.id == nodeB.id);
            if (aIdx < 0 || bIdx < 0
                || aIdx >= a._prefabPart.attachNodes.Count
                || aIdx >= a._prefabPart.attachNodes.Count)
                return null;
            
            var sizeA = (float)a._prefabPart.attachNodes[aIdx].size;
            var sizeB = (float)b._prefabPart.attachNodes[bIdx].size;
            
            if (sizeA == 0)
                sizeA = 0.5f;
            if (sizeB == 0)
                sizeB = 0.5f;
            
            return (sizeA * a.tweakScale/a.defaultScale)/(sizeB * b.tweakScale/b.defaultScale);
        }

        /// <summary>
        /// Automatically scale part to match other part, if applicable.
        /// </summary>
        /// <param name="a">Source part, from which we get the desired scale.</param>
        /// <param name="b">Target part, which will potentially be scaled.</param>
        private static void AutoScale(TweakScale a, TweakScale b)
        {
            if (a == null || b == null)
                return;

            if (a.ScaleType != b.ScaleType)
                return;

            var factor = GetRelativeScaling(a,b);
            if (!factor.HasValue)
                return;

            b.tweakScale = b.tweakScale * factor.Value;
            if (!b.isFreeScale && (b.ScaleFactors.Length > 0))
            {
                b.tweakName = Tools.ClosestIndex(b.tweakScale, b.ScaleFactors);
            }
            b.OnTweakScaleChanged();
        }

        /// <summary>
        /// Scale children with the part.
        /// </summary>
        private void ChainScale()
        {
            foreach (var child in part.children)
            {
                var ts = child.GetComponent<TweakScale>();
                var factor = GetRelativeScaling(this, ts);
                if (!factor.HasValue)
                    continue;

                if (Math.Abs(factor.Value - (tweakScale / currentScale)) <= 1e-4f)
                {
                    AutoScale(this, ts);
                }
            }
        }

        /// <summary>
        /// Scale has changed!
        /// </summary>
        private void OnTweakScaleChanged()
        {
            if (!isFreeScale)
            {
                tweakScale = ScaleFactors[tweakName];
            }

            if ((_chainingEnabled != null) && _chainingEnabled.State)
            {
                ChainScale();
            }

            UpdateByWidth(true, false);
            UpdateWindow();

            foreach (var updater in _updaters)
            {
                updater.OnRescale(ScalingFactor);
            }
            currentScale = tweakScale;
        }

        /// <summary>
        /// Checks if there is more than one TweakScale instance on this part.
        /// </summary>
        /// <returns>True if duplicates exist, false otherwise.</returns>
        private bool CheckForDuplicateTweakScale()
        {
            if (_duplicate == Tristate.False)
                return false;
            if (_duplicate == Tristate.True)
                return true;

            if (this != part.GetComponent<TweakScale>())
            {
                Tools.LogWf("Duplicate TweakScale module on part [{0}] {1}", part.partInfo.name, part.partInfo.title);
                Fields["tweakScale"].guiActiveEditor = false;
                Fields["tweakName"].guiActiveEditor = false;
                _duplicate = Tristate.True;
                return true;
            }
            _duplicate = Tristate.False;
            return false;
        }

        /// <summary>
        /// Checks if the config for this TweakScale instance is valid. If not, logs it and returns false.
        /// </summary>
        /// <returns>True if the config is invalid, false otherwise.</returns>
        private bool CheckForInvalidCfg()
        {
            if (ScaleFactors.Length != 0) 
                return false;
            if (_invalidCfg) 
                return true;

            _invalidCfg = true;
            Tools.LogWf("{0}({1}) has no valid scale factors. This is probably caused by an invalid TweakScale configuration for the part.", part.name, part.partInfo.title);
            Debug.Log("[TweakScale]" + this.ToString());
            Debug.Log("[TweakScale]" + ScaleType.ToString());
            return true;
        }

        [UsedImplicitly]
        void Update()
        {
            if (CheckForDuplicateTweakScale() || CheckForInvalidCfg())
            {
                return;
            }

            if (HighLogic.LoadedSceneIsEditor)
            {
                if (_firstUpdateWithParent && part.HasParent())
                {
                    if((_autoscaleEnabled != null) && _autoscaleEnabled.State)
                        AutoScale(part.parent.GetComponent<TweakScale>(), this);
                }

                if ( currentScale >= 0f)
                {
                    var changed = currentScale != (isFreeScale ? tweakScale : ScaleFactors[tweakName]);

                    if (changed) // user has changed the scale tweakable
                    {
                        // If the user has changed the scale of the part before attaching it, we want to keep that scale.
                        _firstUpdateWithParent = false;
                        OnTweakScaleChanged();
                    }
                    else if (part.transform.GetChild(0).localScale != _savedScale) // editor frequently nukes our OnStart resize some time later
                    {
                        UpdateByWidth(false, true);
                    }
                }
            }

            if (_firstUpdateWithParent && part.HasParent())
            {
                _firstUpdateWithParent = false;
            }

            foreach (var upd in _updaters.OfType<IUpdateable>())
            {
                upd.OnUpdate();
            }
        }

        public float GetModuleCost(float defaultCost)
        {
            if (!_setupRun)
            {
                Setup();
            }
            if (IsRescaled())
                return (float)(DryCost - part.partInfo.cost + part.Resources.Cast<PartResource>().Aggregate(0.0, (a, b) => a + b.maxAmount * b.info.unitCost));
            else
                return 0;
        }

        public override string ToString()
        {
            var result = "TweakScale{\n";
            result += "\n _invalidCfg = " + _invalidCfg;
            result += "\n _setupRun = " + _setupRun;
            result += "\n isFreeScale = " + isFreeScale;
            result += "\n " + ScaleFactors.Length  + " scaleFactors = ";
            foreach (var s in ScaleFactors)
                result += s + "  ";
            result += "\n tweakScale = "   + tweakScale;
            result += "\n currentScale = " + currentScale;
            result += "\n defaultScale = " + defaultScale;
            //result += " scaleNodes = " + ScaleNodes + "\n";
            //result += "   minValue = " + MinValue + "\n";
            //result += "   maxValue = " + MaxValue + "\n";
            return result + "\n}";
        }
    }
}