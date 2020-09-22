using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class PoolableWrapper<T> : Poolable, IProxy<T> where T : Component
    {
        public T Value => value;
        [SerializeField] protected T value;
        
        public override void OnReboot() { }
        
        object IReadable.Read() => value;
        T IReadable<T>.Read() => value;

        void IWritable.Write(object value) => this.value = (T)value;
        void IWritable<T>.Write(T value) => this.value = value;
    }
}