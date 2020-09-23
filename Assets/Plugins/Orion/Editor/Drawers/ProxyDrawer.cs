using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class ProxyDrawer<T,TElement> : OdinValueDrawer<T>  where T : IReadable<TElement>, IWritable<TElement>, IProxy<TElement>
    {
        private bool foldout;
        private Texture icon;

        private Rect rect = Rect.zero;

        protected override void Initialize()
        {
            base.Initialize();
            icon = typeof(TElement).GetIcon();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (!Property.BaseValueEntry.BaseValueType.IsInterface)
            {
                CallNextDrawer(label);
                return;
            }
            
            var text = label.text.Trim();
            label = new GUIContent("  " + text, icon);

            var firstChild = Property.Children.First();
            var firstChildType = firstChild.ValueEntry.TypeOfValue;

            var isObject = typeof(Object).IsAssignableFrom(firstChildType);
            var isArray = typeof(IEnumerable).IsAssignableFrom(firstChildType);
            
            var count = Property.Children.Count;
            var subCount = Property.Children.First().Children.Count;

            if (count > 1)
            {
                if (Property.Attributes.Contains(new InlinedAttribute())) DrawInlined(label);
                else
                {
                    foldout = SirenixEditorGUI.Foldout(foldout, label);
                    if (SirenixEditorGUI.BeginFadeGroup(this, foldout))
                    {
                        GUIHelper.PushIndentLevel(1);
                        for (var i = 0; i < Property.Children.Count; i++) Property.Children[i].Draw();
                        GUIHelper.PopIndentLevel();
                    }
                    SirenixEditorGUI.EndFadeGroup();
                }
            }
            else if (isObject) DrawInlined(label);
            else if (isArray)
            {
                if (firstChild.ValueEntry.WeakSmartValue == null)
                {
                    firstChild.ValueEntry.WeakSmartValue = Activator.CreateInstance(firstChildType,0);
                }
                
                EditorGUILayout.Space(1f);
                EditorGUILayout.BeginHorizontal();

                var size = EditorGUIUtility.singleLineHeight;
                EditorGUILayout.LabelField(new GUIContent(icon), GUILayout.Width(size + 3f), GUILayout.Height(size));
                
                Property.Children.First().Draw(new GUIContent(label.text));

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(-1f);
            }
            else if (subCount <= 1)
            {
                var size = EditorGUIUtility.singleLineHeight;
                if (firstChild.ValueEntry.TypeOfValue.GetCustomAttribute<InlinePropertyAttribute>() == null) SubDraw();
                else
                {
                    EditorGUILayout.Space(-2f);
                    GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth + 3f);
                    
                    SubDraw();
                    
                    GUIHelper.PopLabelWidth();
                    EditorGUILayout.Space(5f);
                    
                }

                void SubDraw()
                {
                    EditorGUILayout.BeginHorizontal();
                    firstChild.Draw(label);

                    if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
                    {
                        Property.BaseValueEntry.WeakSmartValue = null;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                var size = EditorGUIUtility.singleLineHeight;
                if (rect != Rect.zero)
                {
                    rect = new Rect(new Vector2(rect.x + GUIHelper.BetterLabelWidth + 4f, rect.y), Vector2.one * size);
                    if (GUI.Button(rect, EditorIcons.X.Active)) Property.BaseValueEntry.WeakSmartValue = null;
                }
                if (!Property.Children.Any()) return;
                
                GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth + 3f);
                
                Property.Children.First().Draw(label);
                rect = Property.LastDrawnValueRect;
                
                GUIHelper.PopLabelWidth();
            }
        }

        private void DrawInlined(GUIContent label)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(label, GUILayout.Width(GUIHelper.BetterLabelWidth));
            for (var i = 0; i < Property.Children.Count; i++) Property.Children[i].Draw(GUIContent.none);
            
            var size = EditorGUIUtility.singleLineHeight;
            if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
            {
                Property.BaseValueEntry.WeakSmartValue = null;
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}