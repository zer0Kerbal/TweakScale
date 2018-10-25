using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class SanityCheckAlertBox : TimedMessageBox
    { 
        private static readonly string MSG = @"TweakScale found {0} parts that failed sanity checks! See KSP.log for details.

Parts that fails sanity check had TweakScale support withdrawn. This was necessary to prevent them to crash the game. At the present, there's no way to use them without nasty consequences.

TweakScale is working to support that parts.";
        
        internal static void show(int sanity_failures)
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
                String.Format(MSG, sanity_failures),
                30, 1, 0,
                win, text
            );
            Log.detail("\"TweakScale Warning\" about sanity checks was displayed");
        }
    }
}