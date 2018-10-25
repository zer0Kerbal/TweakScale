using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class CheckFailureAlertBox : TimedMessageBox
    { 
        private static readonly string MSG = @"TweakScale found {0} parts that failed being checked! See KSP.log for details.

This does not means that the part has a problem, it can be alright. But since TweakScale cannot know for sure, it's a concern.

This usually happens due Third Parties Add'On or DLC getting into the way, botching the check. Please report, we are working hard to overcome this.";
        
        internal static void show(int check_failures)
        {
            GameObject go = new GameObject("TweakScale.WarningBox");
            TimedMessageBox dlg = go.AddComponent<TimedMessageBox>();
            
            GUIStyle win = new GUIStyle("Window")
            {
                fontSize = 26,
                fontStyle = FontStyle.Bold
            };
            win.normal.textColor = Color.yellow;
            win.border.top = 36;

            GUIStyle text = new GUIStyle("Label")
            {
                fontSize = 18,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft
            };
            text.padding.top = 8;
            text.padding.bottom = text.padding.top;
            text.padding.left = text.padding.top;
            text.padding.right = text.padding.top;
            {
                Texture2D tex = new Texture2D(1,1);
                tex.SetPixel(0,0,new Color(0f, 0f, 0f, 0.45f));
                tex.Apply();
                text.normal.background = tex;
            }

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