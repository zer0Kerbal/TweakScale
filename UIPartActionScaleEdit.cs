using System;
using System.Globalization;
using UnityEngine;

// for KAE
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using KSPAPIExtensions;

// ReSharper disable once CheckNamespace
namespace TweakScale
{

    [UI_ScaleEdit]
    public class UIPartActionScaleEdit : UIPartActionFieldItem
    {
        public SpriteText fieldName;
        public SpriteText fieldValue;
        public UIButton incLargeDown;
        public SpriteText incLargeDownLabel;
        public UIButton incLargeUp;
        public SpriteText incLargeUpLabel;
        public UIProgressSlider slider;

        private float value;
        public int intervalNo = 0;

        private uint controlState;

        public static UIPartActionScaleEdit CreateTemplate()
        {
            // Create the control
            GameObject editGo = new GameObject("UIPartActionScaleEdit", SystemUtils.VersionTaggedType(typeof(UIPartActionScaleEdit)));
            UIPartActionScaleEdit edit = editGo.GetTaggedComponent<UIPartActionScaleEdit>();
            editGo.SetActive(false);

            // TODO: since I don'type have access to EZE GUI, I'm copying out bits from other existing GUIs 
            // if someone does have access, they could do this better although really it works pretty well.
            UIPartActionButton evtp = UIPartActionController.Instance.eventItemPrefab;
            GameObject srcTextGo = evtp.transform.Find("Text").gameObject;
            GameObject srcBackgroundGo = evtp.transform.Find("Background").gameObject;
            GameObject srcButtonGo = evtp.transform.Find("Btn").gameObject;

            UIPartActionFloatRange paFlt = (UIPartActionFloatRange)UIPartActionController.Instance.fieldPrefabs.Find(cls => cls.GetType() == typeof(UIPartActionFloatRange));
            GameObject srcSliderGo = paFlt.transform.Find("Slider").gameObject;


            // Start building our control
            GameObject backgroundGo = (GameObject)Instantiate(srcBackgroundGo);
            backgroundGo.transform.parent = editGo.transform;

            GameObject sliderGo = (GameObject)Instantiate(srcSliderGo);
            sliderGo.transform.parent = editGo.transform;
            sliderGo.transform.localScale = new Vector3(0.65f, 1, 1);
            edit.slider = sliderGo.GetComponent<UIProgressSlider>();
            edit.slider.ignoreDefault = true;


            GameObject fieldNameGo = (GameObject)Instantiate(srcTextGo);
            fieldNameGo.transform.parent = editGo.transform;
            fieldNameGo.transform.localPosition = new Vector3(40, -8, 0);
            edit.fieldName = fieldNameGo.GetComponent<SpriteText>();

            GameObject fieldValueGo = (GameObject)Instantiate(srcTextGo);
            fieldValueGo.transform.parent = editGo.transform;
            fieldValueGo.transform.localPosition = new Vector3(110, -8, 0);
            edit.fieldValue = fieldValueGo.GetComponent<SpriteText>();


            GameObject incLargeDownGo = (GameObject)Instantiate(srcButtonGo);
            incLargeDownGo.transform.parent = edit.transform;
            incLargeDownGo.transform.localScale = new Vector3(0.45f, 1.1f, 1f);
            incLargeDownGo.transform.localPosition = new Vector3(11.5f, -9, 0); //>11
            edit.incLargeDown = incLargeDownGo.GetComponent<UIButton>();

            GameObject incLargeDownLabelGo = (GameObject)Instantiate(srcTextGo);
            incLargeDownLabelGo.transform.parent = editGo.transform;
            incLargeDownLabelGo.transform.localPosition = new Vector3(5.5f, -7, 0); // <6
            edit.incLargeDownLabel = incLargeDownLabelGo.GetComponent<SpriteText>();
            edit.incLargeDownLabel.Text = "<<";

            GameObject incLargeUpGo = (GameObject)Instantiate(srcButtonGo);
            incLargeUpGo.transform.parent = edit.transform;
            incLargeUpGo.transform.localScale = new Vector3(0.45f, 1.1f, 1f);
            incLargeUpGo.transform.localPosition = new Vector3(187.5f, -9, 0); // >187
            edit.incLargeUp = incLargeUpGo.GetComponent<UIButton>();

            GameObject incLargeUpLabelGo = (GameObject)Instantiate(srcTextGo);
            incLargeUpLabelGo.transform.parent = editGo.transform;
            incLargeUpLabelGo.transform.localPosition = new Vector3(181.5f, -7, 0); //<182
            edit.incLargeUpLabel = incLargeUpLabelGo.GetComponent<SpriteText>();
            edit.incLargeUpLabel.Text = ">>";
            return edit;
        }


        protected UI_ScaleEdit FieldInfo
        {
            get
            {
                return (UI_ScaleEdit)control;
            }
        }

        // ReSharper disable ParameterHidesMember
        public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
        {
            base.Setup(window, part, partModule, scene, control, field);
            incLargeDown.SetValueChangedDelegate(obj => buttons_ValueChanged(false));
            incLargeUp.SetValueChangedDelegate(obj => buttons_ValueChanged(true));
            slider.SetValueChangedDelegate(slider_OnValueChanged);

            // so update runs.
            value = GetFieldValue() + 0.1f;
            UpdateFieldInfo();
        }
        // ReSharper restore ParameterHidesMember

        private void buttons_ValueChanged(bool up)
        {
            float newValue = this.value;
            if (up) 
            {
                if (intervalNo < FieldInfo.intervals.Length - 2)
                {
                    if (newValue == FieldInfo.intervals[intervalNo+1])
                        newValue = FieldInfo.intervals[intervalNo+2];
                    intervalNo++;
                }
                else
                    newValue = FieldInfo.intervals [intervalNo+1];
            }
            else
            {
                if (intervalNo > 0)
                {
                    if (newValue == FieldInfo.intervals[intervalNo])
                        newValue = FieldInfo.intervals[intervalNo-1];
                    intervalNo--;
                }
                else
                    newValue = FieldInfo.intervals [0];
            }
            RestrictToInterval (newValue);
        }

        private void slider_OnValueChanged(IUIObject obj)
        {
            float valueLow = FieldInfo.intervals [intervalNo];
            float valueHi  = FieldInfo.intervals [intervalNo + 1];
            float newValue = Mathf.Lerp(valueLow, valueHi, slider.Value);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            float inc = GetIncrementSlide ();
            if (inc != 0)
                newValue = Mathf.Round(newValue / inc) * inc;

            RestrictToInterval(newValue);
        }

        private void OnValueChanged(float newValue)
        {
            //update intervalNo
            intervalNo = 0;

            for( int i=0; i<FieldInfo.intervals.Length-1; i++)
                if(newValue >= FieldInfo.intervals[i])
                    intervalNo = i;

            UpdateValueDisplay (newValue);
        }

        private void RestrictToInterval(float newValue)
        {
            newValue = Math.Max(newValue, FieldInfo.intervals [intervalNo]);
            newValue = Math.Min(newValue, FieldInfo.intervals [intervalNo + 1]);

            SetValueFromGUI(newValue);
        }

        private void SetValueFromGUI(float newValue)
        {
            UpdateValueDisplay(newValue);

            field.SetValue(newValue, field.host);
            if (scene == UI_Scene.Editor)
                SetSymCounterpartValue(newValue);
        }

        private float GetFieldValue()
        {
            return isModule ? field.GetValue<float>(partModule) : field.GetValue<float>(part);
        }

        private float GetIncrementSlide()
        {
            if (FieldInfo.incrementSlide.Length > 1)
                return FieldInfo.incrementSlide [intervalNo];
            else if (FieldInfo.incrementSlide.Length == 1)
                return FieldInfo.incrementSlide [0];
            else
                return 0;
        }

        public override void UpdateItem()
        {
            // update from fieldName. No listeners.
            fieldName.Text = field.guiName;

            // Update the value.
            float fValue = GetFieldValue();
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (fValue != value)
                OnValueChanged(fValue);

            uint newHash = FieldInfo.GetHashedState();
            if (controlState != newHash)
            {
                UpdateFieldInfo();
                controlState = newHash;
            }
        }

        private void UpdateValueDisplay(float newValue)
        {
            this.value = newValue;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            float inc = GetIncrementSlide();
            if (inc != 0)
            {
                newValue = Mathf.Round(newValue / inc) * inc;
                slider.gameObject.SetActive(true);
                float valueLow = FieldInfo.intervals [intervalNo];
                float valueHi = FieldInfo.intervals [intervalNo + 1];
                slider.Value = Mathf.InverseLerp (valueLow, valueHi, newValue);
            }
            else
                slider.gameObject.SetActive(false);

            fieldValue.Text = newValue.ToString(field.guiFormat) + field.guiUnits;
        }

        private void UpdateFieldInfo()
        {
            if (CheckConsistency ())
            {
                incLargeDown.gameObject.SetActive (true);
                incLargeDownLabel.gameObject.SetActive (true);
                incLargeUp.gameObject.SetActive (true);
                incLargeUpLabel.gameObject.SetActive (true);

                slider.transform.localScale = new Vector3(0.81f, 1, 1);
                fieldName.transform.localPosition = new Vector3(24, -8, 0);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (GetIncrementSlide() == 0)
                    slider.gameObject.SetActive(false);
                else
                    slider.gameObject.SetActive(true);
            }
            else
            {
                incLargeDown.gameObject.SetActive (false);
                incLargeDownLabel.gameObject.SetActive (false);
                incLargeUp.gameObject.SetActive (false);
                incLargeUpLabel.gameObject.SetActive (false);

                slider.gameObject.SetActive(false);
            }
        }

        public bool CheckConsistency()
        {
            if (FieldInfo.intervals.Length < 2)
                return false;

            if ((FieldInfo.incrementSlide.Length > 1) &&
                (FieldInfo.incrementSlide.Length < FieldInfo.intervals.Length - 1))
            {
                Debug.LogWarning("[KAE Warning] UI_ScaleEdit: not enough incrementSlide values. Using only the first." + Environment.NewLine + StackTraceUtility.ExtractStackTrace());
                float first = FieldInfo.incrementSlide[0];
                FieldInfo.incrementSlide = new float[1];
                FieldInfo.incrementSlide [0] = first;
                return true;
            }

            for (int i = 0; i < FieldInfo.intervals.Length-2; i++)
            {
                if (FieldInfo.intervals [i] == FieldInfo.intervals [i + 1])
                {
                    Debug.LogWarning("[KAE Warning] UI_ScaleEdit: duplicate value in intervals list" + Environment.NewLine + StackTraceUtility.ExtractStackTrace());
                    return false;
                }
                else if (FieldInfo.intervals [i] > FieldInfo.intervals [i + 1])
                {
                    Debug.LogWarning("[KAE Warning] UI_ScaleEdit: intervals list not sorted" + Environment.NewLine + StackTraceUtility.ExtractStackTrace());
                    return false;
                }
            }
            return true;
        }
    }

    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class UI_ScaleEdit : UI_Control
    {
        private const string UIControlName = "UIPartActionScaleEdit";

        public float[] intervals = { 1, 2, 4 };
        public float[] incrementSlide = {0.02f, 0.04f };

        public float MinValue()
        {
            return intervals [0];
        }
        public float MaxValue()
        {
            return intervals [intervals.Length-1];
        }

        public override void Load(ConfigNode node, object host)
        {
            base.Load(node, host);
        }

        public override void Save(ConfigNode node, object host)
        {
            base.Save(node, host);
        }

        internal uint GetHashedState()
        {
            return ((uint)intervals.GetHashCode()) ^ ((uint)incrementSlide.GetHashCode());
        }
    }

    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    internal class UIPartActionsExtendedRegistration : MonoBehaviour
    {
        private static bool loaded;
        private static bool isLatestVersion;
        private bool isRunning;


        public UIPartActionsExtendedRegistration()
        {
            //Debug.Log("TS-Registrator();");
            //Start();
            //OnLevelWasLoaded(0);
        }


        public void Awake()
        {
            Debug.Log("TS-Registrator.Awake();");
            //Start();
            //OnLevelWasLoaded(0);
        }

        public void Start()
        {
            Debug.Log("TS-Registrator.Start();");
            if (loaded)
            {
                // prevent multiple copies of same object
                Destroy(gameObject);
                return;
            }
            loaded = true;

            DontDestroyOnLoad(gameObject);

            isLatestVersion = true;

            OnLevelWasLoaded(0);
        }

        public void OnLevelWasLoaded(int level)
        {
            if(isRunning)
                StopCoroutine("Register");
            if (!HighLogic.LoadedSceneIsEditor && !HighLogic.LoadedSceneIsFlight)
                return;
            isRunning = true;
            StartCoroutine("Register");
        }

        internal IEnumerator Register()
        {
            UIPartActionController controller;
            while((controller = UIPartActionController.Instance) == null)
                yield return false;

            FieldInfo typesField = (from fld in controller.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                    where fld.FieldType == typeof(List<Type>)
                                    select fld).First();
            List<Type> fieldPrefabTypes;
            while((fieldPrefabTypes = (List<Type>)typesField.GetValue(controller)) == null
                  || fieldPrefabTypes.Count == 0 
                  || !UIPartActionController.Instance.fieldPrefabs.Find(cls => cls.GetType() == typeof(UIPartActionFloatRange)))
                yield return false;

            Debug.Log("[TS] Registering field prefabs for version " + Assembly.GetExecutingAssembly().GetName().Version + (isLatestVersion?" (latest)":""));

            // Register prefabs. This needs to be done for every version of the assembly. (the types might be called the same, but they aren't the same)
            //controller.fieldPrefabs.Add(UIPartActionFloatEdit.CreateTemplate());
            //fieldPrefabTypes.Add(typeof(UI_FloatEdit));

            controller.fieldPrefabs.Add(UIPartActionScaleEdit.CreateTemplate());
            fieldPrefabTypes.Add(typeof(UI_ScaleEdit));

            //controller.fieldPrefabs.Add(UIPartActionChooseOption.CreateTemplate());
            //fieldPrefabTypes.Add(typeof(UI_ChooseOption));

            // Register the label and resource editor fields. This should only be done by the most recent version.
            // => let KSPApiExtensions do this! 
/*            if (isLatestVersion && HighLogic.LoadedSceneIsEditor)
            {
                int idx = controller.fieldPrefabs.FindIndex(item => item.GetType() == typeof (UIPartActionLabel));
                controller.fieldPrefabs[idx] = UIPartActionLabelImproved.CreateTemplate((UIPartActionLabel)controller.fieldPrefabs[idx]);
                controller.resourceItemEditorPrefab = UIPartActionResourceEditorImproved.CreateTemplate(controller.resourceItemEditorPrefab);
            }*/
            isRunning = false;
        }
    }

    internal class UIPartActionResourceEditorImproved : UIPartActionResourceEditor
    {
        // ReSharper disable ParameterHidesMember
        public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
        {
            double amount = resource.amount;
            base.Setup(window, part, scene, control, resource);
            this.resource.amount = amount;

            slider.SetValueChangedDelegate(OnSliderChanged);
        }
        // ReSharper restore ParameterHidesMember

        private float oldSliderValue;

        public override void UpdateItem()
        {
            base.UpdateItem();

            SIPrefix prefix = (resource.maxAmount).GetSIPrefix();
            // ReSharper disable once InconsistentNaming
            Func<double, string> Formatter = prefix.GetFormatter(resource.maxAmount, sigFigs: 4);

            resourceMax.Text = Formatter(resource.maxAmount) + " " + prefix.PrefixString();
            resourceAmnt.Text = Formatter(resource.amount);

            oldSliderValue = slider.Value = (float)(resource.amount / resource.maxAmount);
        }

        private void OnSliderChanged(IUIObject obj)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldSliderValue == slider.Value)
                return;
            oldSliderValue = slider.Value;

            SIPrefix prefix = resource.maxAmount.GetSIPrefix();
            resource.amount = prefix.Round(slider.Value * resource.maxAmount, digits:4);
            //PartMessageService.Send<PartResourceInitialAmountChanged>(this, part, resource, resource.amount);
            if (scene == UI_Scene.Editor)
                SetSymCounterpartsAmount(resource.amount);
            resourceAmnt.Text = resource.amount.ToString("F1");
            GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
        }

        protected new void SetSymCounterpartsAmount(double amount)
        {
            if (part == null)
                return;

            foreach (Part sym in part.symmetryCounterparts)
            {
                if (sym == part)
                    continue;
                PartResource symResource = sym.Resources[resource.info.name];
                symResource.amount = amount;
                //PartMessageService.Send<PartResourceInitialAmountChanged>(this, sym, symResource, symResource.amount);
            }
        }

        internal static UIPartActionResourceEditorImproved CreateTemplate(UIPartActionResourceEditor oldEditor)
        {
            GameObject editGo = (GameObject)Instantiate(oldEditor.gameObject);
            Destroy(editGo.GetComponent<UIPartActionResourceEditor>());
            UIPartActionResourceEditorImproved edit = editGo.AddTaggedComponent<UIPartActionResourceEditorImproved>();
            editGo.SetActive(false);
            edit.transform.parent = oldEditor.transform.parent;
            edit.transform.localPosition = oldEditor.transform.localPosition;

            // Find all the bits.
            edit.slider = editGo.transform.Find("Slider").GetComponent<UIProgressSlider>();
            edit.resourceAmnt = editGo.transform.Find("amnt").GetComponent<SpriteText>();
            edit.resourceName = editGo.transform.Find("name").GetComponent<SpriteText>();
            edit.resourceMax = editGo.transform.Find("total").GetComponent<SpriteText>();
            edit.flowBtn = editGo.transform.Find("StateBtn").GetComponent<UIStateToggleBtn>();

            return edit;
        }
    }


    internal class UIPartActionLabelImproved : UIPartActionLabel
    {
        private SpriteText label;

        public void Awake()
        {
            label = gameObject.GetComponentInChildren<SpriteText>();
        }

        public override void UpdateItem()
        {
            object target = isModule ? (object)partModule : part;

            Type fieldType = field.FieldInfo.FieldType;
            if (fieldType == typeof(double))
            {
                double value = (double)field.FieldInfo.GetValue(target);
                label.Text = (string.IsNullOrEmpty(field.guiName) ? field.name : field.guiName) + " " +
                    (string.IsNullOrEmpty(field.guiFormat) ? value.ToString(CultureInfo.CurrentUICulture) : value.ToStringExt(field.guiFormat))
                        + field.guiUnits;
            }
            if (fieldType == typeof(float))
            {
                float value = (float)field.FieldInfo.GetValue(target);
                label.Text = (string.IsNullOrEmpty(field.guiName) ? field.name : field.guiName) + " " +
                    (string.IsNullOrEmpty(field.guiFormat) ? value.ToString(CultureInfo.CurrentUICulture) : value.ToStringExt(field.guiFormat))
                        + field.guiUnits;
            }
            else
                label.Text = field.GuiString(target);
        }

        internal static UIPartActionLabelImproved CreateTemplate(UIPartActionLabel oldLabel)
        {
            GameObject labelGo = (GameObject)Instantiate(oldLabel.gameObject);
            Destroy(labelGo.GetComponent<UIPartActionLabel>());
            UIPartActionLabelImproved label = labelGo.AddTaggedComponent<UIPartActionLabelImproved>();
            labelGo.SetActive(false);
            label.transform.parent = oldLabel.transform.parent;
            label.transform.localPosition = oldLabel.transform.localPosition;

            return label;
        }
    }

    internal static class GameObjectExtension
    {
        internal static T AddTaggedComponent<T> (this GameObject go) where T : Component
        {
            Type taggedType = SystemUtils.VersionTaggedType(typeof(T));
            return (T)go.AddComponent(taggedType);
        }
        internal static T GetTaggedComponent<T> (this GameObject go) where T : Component
        {
            Type taggedType = SystemUtils.VersionTaggedType(typeof(T));
            return (T)go.GetComponent(taggedType);
        }
    }

}
