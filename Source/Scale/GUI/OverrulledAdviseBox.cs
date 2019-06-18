using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class OverrulledAdviseBox : TimedMessageBox
    {
        private static readonly string MSG = @"There're {0} parts with overrules detected.

A overrule is a problem detected but the fix would break a savegame, so a patch to overcome the problem is applied.

Do not start new savegames with overruled parts. Use them only to exising ones.";
        internal static void show(int overrule_count)
        {
            GameObject go = new GameObject("TweakScale.AdviseBox");
            TimedMessageBox dlg = go.AddComponent<TimedMessageBox>();
            
            GUIStyle win = new GUIStyle("Window")
            {
                fontSize = 26,
                fontStyle = FontStyle.Bold
            };
            win.normal.textColor = Color.white;
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
                "TweakScale advises", 
                String.Format(MSG, overrule_count),
                30, 1, -1,
                win, text
            );
            Debug.Log("[TWEAKSCALE] \"TweakScale advises\" about overrules checks was displayed");
        }
    }
}