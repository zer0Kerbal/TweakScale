using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class CheckFailureAlertBox : CommonBox
    { 
        private static readonly string MSG = @"TweakScale found {0} parts that failed being checked! See KSP.log for details.

This does not means that the part(s) has(have) a problem, it(they) can be alright. But since TweakScale cannot know for sure, it's a concern.

This usually happens due Third Parties Add'On or DLC getting into the way, botching the check. Please report, we are working hard to overcome this.";

        internal static void show(int check_failures)
        {
            GameObject go = new GameObject("TweakScale.WarningBox");
            TimedMessageBox dlg = go.AddComponent<TimedMessageBox>();
            
            GUIStyle win = createWinStyle();
            GUIStyle text = createTextStyle();
            
            dlg.Show(
                "TweakScale Warning", 
                String.Format(MSG, check_failures),
                30, 1, 1,
                win, text
            );
            Log.detail("\"TweakScale Warning\" about check failures was displayed");
        }
    }
}