using System;
using UnityEngine;

namespace Orion
{
    public abstract class RemoteEvent<T> : ScriptableObject
    {
        public event Action<T> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T> call;

        public void Invoke(T value) => call?.Invoke(value);
    }
    
    public abstract class RemoteEvent<T1,T2> : ScriptableObject
    {
        public event Action<T1,T2> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T1,T2> call;

        public void Invoke(T1 arg1, T2 arg2) => call?.Invoke(arg1, arg2);
    }
    
    public abstract class RemoteEvent<T1,T2,T3> : ScriptableObject
    {
        public event Action<T1,T2,T3> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T1,T2,T3> call;

        public void Invoke(T1 arg1, T2 arg2, T3 arg3) => call?.Invoke(arg1, arg2, arg3);
    }
}