using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class PoolableWrapper<T> : Poolable where T : Component
    {
        public T Value => value;
        [SerializeField] protected T value;
        
        public override void OnReboot() { }
    }
}