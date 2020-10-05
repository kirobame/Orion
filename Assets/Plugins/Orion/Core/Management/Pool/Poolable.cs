using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Orion
{
    public abstract class Poolable<T> : SerializedMonoBehaviour, IProxy<T>
    {
        [FoldoutGroup("Events")] public OrionEvent onReboot = new OrionEvent();

        public T Value => value;
        [SerializeField] private T value;
         
        private Pool<T> origin;
        
        protected virtual void OnDisable()
        {
            origin.Stock(this);
            origin = null;
        }

        public void SetOrigin(Pool<T> origin) => this.origin = origin;

        public void Reboot()
        {
            onReboot.Invoke();
            OnReboot();
        }
        public virtual void OnReboot() { }
        
        object IReadable.Read() => Read();
        public T Read() => value;
        
        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => this.value = value;

    }
}