using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public static class OrionGUIUtility 
    {
        private static bool IsPrimitive(Type type)
        {
            if (type.IsPrimitive) return true;
            else if (type == typeof(string)) return true;
            else if (type == typeof(object)) return true;
            else if (type.BaseType == typeof(Enum)) return true;
            
            return false;
        }

        public static Texture GetSmallIcon(this Type type) => GetTypeIcon(type, 16);
        public static Texture GetIcon(this Type type) => GetTypeIcon(type, 32);
        private static Texture GetTypeIcon(Type type, int resolution)
        {
            if (type == typeof(Enum)) return Resources.Load<Texture>($"Icons/Types/enum@{resolution}x");
            else if (type.Namespace != null && type.Namespace.Contains("Orion"))
            {
                var guids = AssetDatabase.FindAssets(type.Name, new string[] {"Assets/Plugins/Orion/Core"});
                var meta = File.ReadAllLines($"{AssetDatabase.GUIDToAssetPath(guids.First())}.meta");

                var startIndex =  meta[7].IndexOf("guid: ", StringComparison.Ordinal) + 6;
                var endIndex =  meta[7].IndexOf(',', startIndex);
                
                var iconPath = AssetDatabase.GUIDToAssetPath(meta[7].Substring(startIndex, endIndex - startIndex));
                return AssetDatabase.LoadAssetAtPath<Texture>(iconPath);
            } 
            else if (type == typeof(Object)) return Resources.Load<Texture>($"Icons/Types/object@{resolution}x");
            else if (typeof(Object).IsAssignableFrom(type)) return EditorGUIUtility.ObjectContent(null, type).image;
            else if (IsPrimitive(type)) return Resources.Load<Texture>($"Icons/Types/{type.GetNiceName()}@{resolution}x");
            else
            {
                var texture = Resources.Load<Texture>($"Icons/Types/{type.GetNiceFullName()}@{resolution}x");
                if (texture == null)
                {
                    if (type.IsClass) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Class@{resolution}x");
                    else if (type.IsValueType) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Struct@{resolution}x");
                    else if (type.IsInterface) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Interface@{resolution}x");
                }

                return texture;
            }
            
            return null;
        }

        public static Type GetProxyType(this InspectorProperty property)
        {
            var type = default(Type);
            
            if (property.ValueEntry.TypeOfValue.IsGenericType) type = property.ValueEntry.TypeOfValue.GetGenericArguments().First();
            else type = property.BaseValueEntry.TypeOfValue.BaseType.GetGenericArguments().First();

            return type;
        }
    }
}