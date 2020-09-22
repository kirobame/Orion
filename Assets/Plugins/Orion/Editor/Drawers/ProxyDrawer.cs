using System.Collections;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class ProxyDrawer<T,TElement> : OdinValueDrawer<T>  where T : IReadable<TElement>, IWritable<TElement>, IProxy<TElement>
    {
        private bool foldout;
        private Texture icon;

        protected override void Initialize()
        {
            base.Initialize();
            icon = typeof(TElement).GetIcon();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            label = new GUIContent("  " + label.text, icon);
            
            Debug.Log($"{Property.Name} / {Property.ValueEntry.TypeOfValue} / {Property.Children.First().Children.Count}");

            var  count = Property.Children.Count;
            var subCount = Property.Children.First().Children.Count;
            
            if (Property.Attributes.Contains(new InlinedAttribute()) || count == 1 && subCount == 1)
            {
                Debug.Log(0);
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
            else if (subCount > 1)
            {
                Debug.Log(1);

                var rect = GUIHelper.GetCurrentLayoutRect();
                SirenixEditorGUI.DrawSolidRect(rect, new Color(1, 0, 0, 0.1f));
                
                Property.Children.First().Draw(label);

                /*var rect = Property.LastDrawnValueRect;
                rect = new Rect(new Vector2(rect.x + GUIHelper.BetterLabelWidth, rect.y), Vector2.one * EditorGUIUtility.singleLineHeight);
                
                if (GUI.Button(rect, EditorIcons.X.Active))
                {
                    Property.BaseValueEntry.WeakSmartValue = null;
                }*/
            }
            else
            {
                Debug.Log(2);
                
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
    }
}