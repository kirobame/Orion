using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ludiq.PeekCore;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class WrappedValueDrawer<T,TElement> : OdinValueDrawer<T> where T : WrappedValue<TElement>
    {
        private static Type[] inlinedTypes = new Type[]
        {
            typeof(Enum),
            typeof(string),
            
            typeof(Vector2),
            typeof(Vector3),
            typeof(AnimationCurve),
            typeof(Object),
        };

        private bool inlined;
        private Texture icon;

        private bool foldout;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            icon = typeof(TElement).GetIcon();
            var propertyType = Property.ValueEntry.TypeOfValue;
            
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(PropertyValue<>)) inlined = true;
            else if (typeof(TElement).IsPrimitive) inlined = true;
            else
            {
                foreach (var inlinedType in inlinedTypes)
                {
                    if (!inlinedType.IsAssignableFrom(typeof(TElement))) continue;
                    
                    inlined = true;
                    break;
                }
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (inlined)
            {
                EditorGUILayout.BeginHorizontal();
            
                SirenixEditorGUI.IndentSpace();
            
                var size = EditorGUIUtility.singleLineHeight;
                GUILayout.Label(new GUIContent(" " + label.text,icon), GUILayout.Width(GUIHelper.BetterLabelWidth - 2f), GUILayout.Height(size));

                Property.Children[0].Draw();
            
                if (Property.BaseValueEntry.BaseValueType.IsInterface)
                {
                    if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
                    {
                        Property.BaseValueEntry.WeakSmartValue = null;
                    }
                }
            
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                foldout = SirenixEditorGUI.Foldout(foldout, new GUIContent("  " + label.text, icon));
                if (foldout) Property.Children[0].Draw();
            }
        }    
    }
}