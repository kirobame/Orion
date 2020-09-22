using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker, InlineProperty]
    public class ReferenceProxy<T> : IProxy<T> where T : Object
    {
        [SerializeField, HideLabel] private Token token;
        
        object IReadable.Read() => Read();
        public T Read() => (T)Repository.Objects[token];

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value) => Repository.Referencers[token].SetValue(value);
    }
}