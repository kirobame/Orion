using System;
using Ludiq.PeekCore;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker]
    public class WrappedValue<T> : IProxy<T>
    {
        [SerializeField, HideReferenceObjectPicker, HideLabel] private T value;
        
        object IReadable.Read() => Read();
        public T Read() => value;

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => this.value = value;
    }
}