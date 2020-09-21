using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [HideReferenceObjectPicker]
    public class RelayedValue<T> : IProxy<T>
    {
        [SerializeField, HideLabel] private Object target;
        [SerializeField, HideLabel] private Token token;
        
        [SerializeField, HideLabel] private string setMethod = "None";
        [SerializeField, HideLabel] private string getMethod = "None";
        
        private bool hasSetterBeenInitialized;
        private Action<Token, T> setter;

        private bool hasGetterBeenInitialized;
        private Func<Token, T> getter;
        
        object IReadable.Read() => Read();
        public T Read()
        {
            if (!hasSetterBeenInitialized) BootUpSetter();
            return getter(token);
        }

        void IWritable.Write(object value) => Write((T)value);
        public void Write(T value)
        {
            if (!hasGetterBeenInitialized) BootUpGetter();
            setter(token, value);
        }

        private void BootUpSetter()
        {
            var setInfo = target.GetType().GetMethod(setMethod, BindingFlags.Instance | BindingFlags.Public);
            setter = setInfo.CreateDelegate(typeof(Action<Token,T>)) as Action<Token,T>;
            
            hasSetterBeenInitialized = true;
        }
        private void BootUpGetter()
        {
            var getInfo = target.GetType().GetMethod(getMethod, BindingFlags.Instance | BindingFlags.Public);
            getter = getInfo.CreateDelegate(typeof(Func<Token, T>)) as Func<Token, T>;
            
            hasGetterBeenInitialized = true;
        }
    }
}