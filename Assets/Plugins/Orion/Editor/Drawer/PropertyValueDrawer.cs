using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ludiq.OdinSerializer.Utilities;
using Ludiq.PeekCore;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class PropertyValueDrawer<T,TElement> : OdinValueDrawer<T> where T : PropertyValue<TElement>
    {
        private GenericSelector<PropertyInfo> dropdown;
        private Object previousTarget;

        private Texture icon;
        
        protected override void Initialize()
        {
            base.Initialize();
            icon = typeof(TElement).GetIcon();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUILayout.BeginHorizontal();

            var size = EditorGUIUtility.singleLineHeight;
            GUILayout.Label(new GUIContent("  " + label.text,icon), GUILayout.Width(GUIHelper.BetterLabelWidth - 2f), GUILayout.Height(size));
            
            var targetProperty = Property.Children[0];
            targetProperty.Draw(GUIContent.none);

            var target = targetProperty.ValueEntry.WeakSmartValue as Object;
            if (target != previousTarget) OnTargetChanged();
            previousTarget = target;
            
            var propertyRegistry = Property.Children[1];
            if (GUILayout.Button((string) propertyRegistry.ValueEntry.WeakSmartValue, GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (target == null) return;
                
                var windowRect = targetProperty.LastDrawnValueRect;
                var windowWith = EditorGUIUtility.currentViewWidth - GUIHelper.BetterLabelWidth - 46f;
                
                var window = dropdown.ShowInPopup(new Vector2(windowRect.x, windowRect.yMax - 1f), windowWith);
                window.OnClose += dropdown.SelectionTree.Selection.ConfirmSelection; 
            }
            
            if (Property.BaseValueEntry.BaseValueType.IsInterface)
            {
                if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
                {
                    Property.BaseValueEntry.WeakSmartValue = null;
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void OnTargetChanged()
        {
            var target = Property.Children[0].ValueEntry.WeakSmartValue as Object;
            if (target == null)
            {
                Property.Children[1].ValueEntry.WeakSmartValue = "None";
                return;
            }

            var type = Property.GetProxyType();
            var properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(info => type.IsAssignableFrom(info.PropertyType)).ToArray();
            dropdown = new GenericSelector<PropertyInfo>(string.Empty, false, info => info.Name, properties);
            
            dropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            dropdown.SelectionConfirmed += SavePropertyRegistry;
            dropdown.SelectionChanged += SavePropertyRegistry;
        }

        private void SavePropertyRegistry(IEnumerable<PropertyInfo> properties)
        {
            var property = properties.FirstOrDefault();
            if (property == null) return;
            
            var propertyRegistry = Property.Children[1];
            propertyRegistry.ValueEntry.WeakSmartValue = property.Name;
        }
    }
}