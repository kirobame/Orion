using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewDynamicToken", menuName = "Orion/Tokens/Dynamic")]
    public class DynamicToken : ScriptableObject
    {
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
    }
}