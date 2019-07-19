using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class OverrulledAdviseBox : CommonBox
    {
        private static readonly string MSG = @"There're {0} parts with overrules detected.

A overrule is applied to problems already detected but the fix would break a savegame, so a patch to overcome this new problem is applied.

Do not start new savegames with overruled parts as they make your artifacts non standard and, so, no shareable. Use them only to exising savegames to keep them going.";

        internal static void show(int overrule_count)
        {
            GameObject go = new GameObject("TweakScale.AdviseBox");
            TimedMessageBox dlg = go.AddComponent<TimedMessageBox>();

            GUIStyle win = createWinStyle();
            GUIStyle text = createTextStyle();

            dlg.Show(
                "TweakScale advises", 
                String.Format(MSG, overrule_count),
                30, 0, -1,
                win, text
            );
            Log.detail("\"TweakScale advises\" about overrules checks was displayed");
        }
    }
}