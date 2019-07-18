using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class OverrulledAdviseBox : CommonBox
    {
        private static readonly string MSG = @"There're {0} parts with overrules detected.

A overrule is a problem detected but the fix would break a savegame, so a patch to overcome the problem is applied.

Do not start new savegames with overruled parts. Use them only to exising ones.";
        internal static void show(int overrule_count)
        {
            GameObject go = new GameObject("TweakScale.AdviseBox");
            TimedMessageBox dlg = go.AddComponent<TimedMessageBox>();

            GUIStyle win = createWinStyle();
            GUIStyle text = createTextStyle();

            dlg.Show(
                "TweakScale advises", 
                String.Format(MSG, overrule_count),
                30, 1, -1,
                win, text
            );
            Log.detail("\"TweakScale advises\" about overrules checks was displayed");
        }
    }
}