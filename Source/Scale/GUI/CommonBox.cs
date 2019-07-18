using System;
using UnityEngine;
using KSPe.UI;

namespace TweakScale.GUI
{
    internal class CommonBox : TimedMessageBox
    {
        internal static GUIStyle createWinStyle ()
        {
            GUIStyle style = new GUIStyle("Window")
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold
            };
            style.normal.textColor = Color.yellow;
            style.border.top = 24;
            
            return style;
        }
        
        internal static GUIStyle createTextStyle ()
        {
            GUIStyle style = new GUIStyle("Label")
            {
                fontSize = 14,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft
            };
            style.padding.top = 8;
            style.padding.bottom = style.padding.top;
            style.padding.left = style.padding.top;
            style.padding.right = style.padding.top;
            {
                Texture2D tex = new Texture2D(1,1);
                tex.SetPixel(0,0,new Color(0f, 0f, 0f, 0.45f));
                tex.Apply();
                style.normal.background = tex;
            }
            
            return style;
        }
    }
}
