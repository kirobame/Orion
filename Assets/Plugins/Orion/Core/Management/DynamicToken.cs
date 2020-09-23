using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewDynamicToken", menuName = "Orion/Tokens/Dynamic")]
    public class DynamicToken : ScriptableObject
    {
        public IReadOnlyDictionary<int, object> Registry => registry;
        private Dictionary<int, object> registry = new Dictionary<int, object>();

        public void Register(Object owner, object value)
        {
            try
            {
                registry.Add(owner.GetInstanceID(), value);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Debug.Break();

                return;
            }
        }
        public bool Unregister(Object owner) => registry.Remove(owner.GetInstanceID());

        public T Get<T>(Object owner) => (T)registry[owner.GetInstanceID()];
        public bool TryGet<T>(Object owner, out T value)
        {
            if (registry.TryGetValue(owner.GetInstanceID(), out var rawValue))
            {
                value = (T)rawValue;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public void Set<T>(Object owner, T value)
        {
            var id = owner.GetInstanceID();
            
            if (registry.ContainsKey(id)) registry[id] = value;
            else Register(owner, value);
        }
    }
}