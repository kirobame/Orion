using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class AlteredValueDrawer<T,TElement> : OdinValueDrawer<T> where T : AlteredValue<TElement>
    {
        private Texture icon;
        
        protected override void Initialize()
        {
            base.Initialize();
            icon = typeof(TElement).GetIcon();
        }
        
        protected override void DrawPropertyLayout(GUIContent label) => CallNextDrawer(new GUIContent("  " + label.text, icon));
    }
}