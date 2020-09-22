using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class PersistentCallDrawer<T> : OdinValueDrawer<T> where T : PersistentCallBase
    {
        private GenericSelector<MethodInfo> dropdown;
        private Object previousTarget;
        
        private Type[] argTypes;
        private bool parameterFoldout;
        
        protected override void Initialize()
        {
            base.Initialize();
            OnTargetChanged(Property.Children[0].ValueEntry.WeakSmartValue as Object);

            var persistentCall = (Property.ValueEntry.WeakSmartValue as PersistentCallBase);
            if (persistentCall.Info == string.Empty) return;
            
            var splittedInfo = persistentCall.Info.Split('/');
            argTypes = new Type[splittedInfo.Length];
            for (var i = 0; i < splittedInfo.Length; i++) argTypes[i] = Type.GetType(splittedInfo[i]);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var persistentCall = Property.ValueEntry.WeakSmartValue as PersistentCallBase;
            var current = persistentCall.GetMethod();
            
            DrawHeader(current);
            DrawParameters(persistentCall, current);
        }

        #region Drawing

        private void DrawHeader(MethodInfo current)
        {
            EditorGUILayout.BeginHorizontal();
            
            var targetProperty = Property.Children[0];
            var target = targetProperty.ValueEntry.WeakSmartValue as Object;
            
            targetProperty.Draw();
            
            string name;
            if (target == null) name = "None";
            else
            {
                if (current == null) name = "None";
                else if (current.Name.Contains("set_")) name = current.Name.Remove(0, 4);
                else name = current.Name;
            }
            
            if (GUILayout.Button(name, EditorStyles.popup, GUILayout.Width(EditorGUIUtility.currentViewWidth - GUIHelper.BetterLabelWidth - 70f)) && target != null)
            {
                var lastRect = targetProperty.LastDrawnValueRect;
                var position = new Vector2(lastRect.xMax + 3f, lastRect.yMax - 2f);
                
                var window = dropdown.ShowInPopup(position, EditorGUIUtility.currentViewWidth - GUIHelper.BetterLabelWidth - 69f);
                window.OnClose += dropdown.SelectionTree.Selection.ConfirmSelection; 
            }
            
            if (target != previousTarget) OnTargetChanged(target);
            previousTarget = target;
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawParameters(PersistentCallBase persistentCall, MethodInfo current)
        {
            if (Property.Children.Count <= 2 || current == null) return;

            parameterFoldout = EditorGUILayout.Foldout(parameterFoldout, new GUIContent("Parameters"));
            if (SirenixEditorGUI.BeginFadeGroup(this, parameterFoldout))
            { 
                var linkage = ((ILinkage)persistentCall).Linkage;
                var parameters = current.GetParameters();

                if (persistentCall.Info != string.Empty)
                {
                    for (var x = 0; x < parameters.Length; x++)
                    {
                        var size = EditorGUIUtility.singleLineHeight;
                        var width = size + 15f;
            
                        var rect = GUILayoutUtility.GetLastRect().AddPosition(0f, size + 2f);
                        rect = new Rect(new Vector2(rect.x + GUIHelper.BetterLabelWidth - width -2f, rect.y), new Vector2(width, rect.height));
                
                        var selection = -1;
                        var availableIndices = new List<int>();
                        for (var y = 0; y < argTypes.Length; y++)
                        {
                            if (linkage[x,y])
                            {
                                selection = y;
                                availableIndices.Add(y);
                        
                                continue;
                            }
                            if (argTypes[y] != parameters[x].ParameterType) continue;

                            availableIndices.Add(y);
                        }

                        var names = availableIndices.ConvertAll(index => (index + 1).ToString());
                        names.Insert(0,"Ø");

                        if (selection == -1) selection = 0;
                        else selection = names.IndexOf((selection + 1).ToString());

                        selection = EditorGUI.Popup(rect, selection, names.ToArray());
                        selection--;

                        if (selection >= 0)
                        {
                            selection = availableIndices[selection];
                    
                            for (var y = 0; y < argTypes.Length; y++)
                            {
                                if (y == selection) linkage[x,y] = true;
                                else linkage[x,y] = false;
                            }
                        }
                        else for (var y = 0; y < argTypes.Length; y++) linkage[x,y] = false;
                
                        Property.Children[2 + x].Draw(new GUIContent(ObjectNames.NicifyVariableName(parameters[x].Name)));
                    }
                }
                else
                {
                    for (var x = 0; x < parameters.Length; x++)
                    {
                        Property.Children[2 + x].Draw(new GUIContent(ObjectNames.NicifyVariableName(parameters[x].Name)));
                    }
                }
            }//
            SirenixEditorGUI.EndFadeGroup();
        }
        #endregion
        
        #region Method Registration

        private void OnTargetChanged(Object target)
        {
            if (target == null)
            {
                var persistentCall = Property.ValueEntry.WeakSmartValue as PersistentCallBase;
                
                var call = new PersistentCall();
                call.Set(null, string.Empty, persistentCall.Info);
                Property.BaseValueEntry.WeakSmartValue = call;
                
                return;
            }
            
            bool IsValidMethod(MethodInfo info)
            {
                if (info.IsGenericMethod) return false;
                if (info.ReturnType != typeof(void)) return false;
                if (info.GetParameters().Length > 4) return false;

                return true;
            }
            
            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(IsValidMethod).ToArray();
            
            string GetMethodName(MethodInfo info)
            {
                if (info.Name.Contains("set_")) return $"Properties/{info.GetParameters().First().ParameterType.Name} {info.Name.Remove(0,4)}";
                else if (methods.Count(otherInfo => info.Name == otherInfo.Name) > 1) return $"Methods/{info.Name}/{info.GetNiceName()}";
                else return $"Methods/{info.GetNiceName()}";
            }
            
            dropdown = new GenericSelector<MethodInfo>(string.Empty, false, GetMethodName, methods);
            dropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            dropdown.SelectionChanged += SaveMethod;
            dropdown.SelectionConfirmed += SaveMethod;
        }

        private void SaveMethod(IEnumerable<MethodInfo> infos)
        {
            var persistentCall = Property.ValueEntry.WeakSmartValue as PersistentCallBase;
            
            var info = infos.FirstOrDefault();
            if (info == null)
            {
                var current = persistentCall.GetMethod();
                if (current != null) dropdown.SetSelection(current);
                
                return;
            }

            var parameters = info.GetParameters();
            var types = new Type[parameters.Length];
            
            var method = info.Name;
            for (var i = 0; i < parameters.Length; i++)
            {
                types[i] = parameters[i].ParameterType;
                method += $"/{types[i].AssemblyQualifiedName}";
            }
            
            
            PersistentCallBase call;
            if (parameters.Length == 0) call = new PersistentCall();
            else
            {
                var callDefinitions = new Type[]
                {
                    typeof(PersistentCall<>),
                    typeof(PersistentCall<,>),
                    typeof(PersistentCall<,,>),
                    typeof(PersistentCall<,,,>),
                };

                var callType = callDefinitions[parameters.Length - 1].MakeGenericType(types);
                call = Activator.CreateInstance(callType) as PersistentCallBase;
            }
            
            call.Set(persistentCall.Target, method, persistentCall.Info);
            Property.BaseValueEntry.WeakSmartValue = call;
        }
        #endregion
    }
}