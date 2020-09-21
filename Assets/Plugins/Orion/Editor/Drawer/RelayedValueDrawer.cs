using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class RelayedValueDrawer<T, TElement> : OdinValueDrawer<T> where T : RelayedValue<TElement>
    {
        private GenericSelector<MethodInfo> setDropdown;
        private GenericSelector<MethodInfo> getDropdown;

        private Object previousTarget;
        private bool foldout;
        
        private Texture icon;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            var type = Property.GetProxyType();
            icon = type.GetIcon();
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            foldout = SirenixEditorGUI.Foldout(foldout, new GUIContent("  " + label.text, icon));

            if (!foldout) return;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(GUIHelper.BetterLabelWidth));
            
            var targetProperty = Property.Children[0];
            targetProperty.Draw(GUIContent.none);

            var target = targetProperty.ValueEntry.WeakSmartValue as Object;
            if (target != previousTarget) OnTargetChanged();
            previousTarget = target;

            var tokenProperty = Property.Children[1];
            tokenProperty.Draw(GUIContent.none);
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - GUIHelper.BetterLabelWidth - 26f));

            if (GUILayout.Button("Set - " + (string)Property.Children[2].ValueEntry.WeakSmartValue, EditorStyles.popup))
            {
                if (target == null) return;
                OpenDropdown(setDropdown, targetProperty.LastDrawnValueRect);
            }
            
            if (GUILayout.Button("Get - " + (string)Property.Children[3].ValueEntry.WeakSmartValue, EditorStyles.popup))
            {
                if (target == null) return;
                OpenDropdown(getDropdown, tokenProperty.LastDrawnValueRect);
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void OpenDropdown(GenericSelector<MethodInfo> dropdown, Rect rect)
        {
            var width = EditorGUIUtility.currentViewWidth - GUIHelper.BetterLabelWidth - 26f;
                
            var window = dropdown.ShowInPopup(new Vector2(rect.xMax + 4f, rect.yMax - 2f), width);
            window.OnClose += dropdown.SelectionTree.Selection.ConfirmSelection; 
        }
        
        private void OnTargetChanged()
        {
            var target = Property.Children[0].ValueEntry.WeakSmartValue as Object;
            if (target == null)
            {
                Property.Children[2].ValueEntry.WeakSmartValue = "None";
                Property.Children[3].ValueEntry.WeakSmartValue = "None";
                return;
            }
            
            var type =  Property.BaseValueEntry.TypeOfValue.BaseType.GetGenericArguments().First();
            var getMethods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(info =>
            {
                if (info.ReturnType != type) return false;

                var parameters = info.GetParameters();
                if (parameters.Length < 1 || parameters.Length > 1) return false;

                return parameters.First().ParameterType == typeof(Token);
            }).ToArray();
            string GetGetterName(MethodInfo method)
            {
                if (method.Name.Contains("get_")) return $"{method.ReturnType.Name} {method.Name.Remove(0, 4)}";
                else return $"{method.ReturnType.Name} {method.Name}()";
            }
            getDropdown = new GenericSelector<MethodInfo>(string.Empty, false, GetGetterName, getMethods);
            
            getDropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            getDropdown.SelectionConfirmed += SaveGetMethod;
            getDropdown.SelectionChanged += SaveGetMethod;
            
            var setMethods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(info =>
            {
                if (info.ReturnType != typeof(void)) return false;
                
                var parameters = info.GetParameters();
                if (parameters.Length < 2 || parameters.Length > 2) return false;

                return parameters.First().ParameterType == typeof(Token) && parameters.Last().ParameterType == type;
            }).ToArray();
            string GetSetterName(MethodInfo method)
            {
                if (method.Name.Contains("set_")) return $"{method.GetParameters().First().ParameterType.Name} {method.Name.Remove(0, 4)}";
                else return $"{method.GetParameters().First().ParameterType.Name} {method.Name}()";
            }
            setDropdown = new GenericSelector<MethodInfo>(string.Empty, false, GetSetterName, setMethods);

            setDropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            setDropdown.SelectionConfirmed += SaveSetMethod;
            setDropdown.SelectionChanged += SaveSetMethod;
        }

        private void SaveSetMethod(IEnumerable<MethodInfo> methods) => SaveMethod(methods, 2);
        private void SaveGetMethod(IEnumerable<MethodInfo> methods) => SaveMethod(methods, 3);
        private void SaveMethod(IEnumerable<MethodInfo> methods, int methodIndex)
        {
            var method = methods.FirstOrDefault();
            if (method == null) return;
            
            var methodRegistry = Property.Children[methodIndex];
            methodRegistry.ValueEntry.WeakSmartValue = method.Name;
        }
    }
}