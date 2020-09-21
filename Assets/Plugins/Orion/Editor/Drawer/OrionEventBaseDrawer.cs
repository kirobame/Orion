using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    public class OrionEventBaseDrawer<T> : OdinValueDrawer<T> where T : OrionEventBase
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var arrayProperty = Property.Children.First();
            arrayProperty.Draw(label);

            var size = EditorGUIUtility.singleLineHeight + 4f;
            
            var rect = arrayProperty.LastDrawnValueRect;
            rect = new Rect(new Vector2(rect.xMax - 44f - 6f, rect.y), Vector2.one * size);
            rect = rect.Padding(0f);
            
            EditorGUI.LabelField(rect, new GUIContent(Resources.Load<Texture>($"Icons/Language/VisualStudioMonochrome/Interface@32x")));
        }
    }
}