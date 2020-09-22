﻿using System;

namespace Orion
{
    public class OrionEvent : OrionEventBase
    {
        public event Action Call
        {
            add => runtimeAction += value;
            remove => runtimeAction -= value;
        }
        
        private event Action runtimeAction;
        
        public void Invoke()
        {
            runtimeAction?.Invoke();
            Invoke(new object[0]);
        }

        protected override PersistentCallBase AddPersistentCall() => new PersistentCall();
    }
    
    public class OrionEvent<T> : OrionEventBase
    {
        public event Action<T> Call
        {
            add => runtimeAction += value;
            remove => runtimeAction -= value;
        }
        
        private event Action<T> runtimeAction;
        
        public void Invoke(T arg)
        {
            runtimeAction?.Invoke(arg);

            var args = new object[] {arg};
            Invoke(args);
        }
        
        protected override PersistentCallBase AddPersistentCall()
        {
            var call = new PersistentCall();
            call.Set(null, string.Empty, typeof(T).AssemblyQualifiedName);

            return call;
        }
    }
    
    public class OrionEvent<T1,T2> : OrionEventBase
    {
        public event Action<T1,T2>  Call
        {
            add => runtimeAction += value;
            remove => runtimeAction -= value;
        }
        
        private event Action<T1,T2>  runtimeAction;
        
        public void Invoke(T1 arg1, T2 arg2)
        {
            runtimeAction?.Invoke(arg1, arg2);

            var args = new object[] {arg1, arg2};
            Invoke(args);
        }
        
        protected override PersistentCallBase AddPersistentCall()
        {
            var info = string.Empty;
            info += $"{typeof(T1).AssemblyQualifiedName}/";
            info += typeof(T2).AssemblyQualifiedName;
            
            var call = new PersistentCall();
            call.Set(null, string.Empty, $"{typeof(T1).AssemblyQualifiedName}/{typeof(T2).AssemblyQualifiedName}");

            return call;
        }
    }
    
    public class OrionEvent<T1,T2,T3> : OrionEventBase
    {
        public event Action<T1,T2,T3> Call
        {
            add => runtimeAction += value;
            remove => runtimeAction -= value;
        }
        
        private event Action<T1,T2,T3> runtimeAction;
        
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            runtimeAction?.Invoke(arg1, arg2, arg3);

            var args = new object[] {arg1, arg2, arg3};
            Invoke(args);
        }
        
        protected override PersistentCallBase AddPersistentCall()
        {
            var info = string.Empty;
            info += $"{typeof(T1).AssemblyQualifiedName}/";
            info += $"{typeof(T2).AssemblyQualifiedName}/";
            info += typeof(T3).AssemblyQualifiedName;
            
            var call = new PersistentCall();
            call.Set(null, string.Empty, info);

            return call;
        }
    }
    
    public class OrionEvent<T1,T2,T3,T4> : OrionEventBase
    {
        public event Action<T1,T2,T3,T4> Call
        {
            add => runtimeAction += value;
            remove => runtimeAction -= value;
        }
        
        private event Action<T1,T2,T3,T4>  runtimeAction;
        
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            runtimeAction?.Invoke(arg1, arg2, arg3, arg4);

            var args = new object[] {arg1, arg2, arg3, arg4};
            Invoke(args);
        }
        
        protected override PersistentCallBase AddPersistentCall()
        {
            var info = string.Empty;
            info += $"{typeof(T1).AssemblyQualifiedName}/";
            info += $"{typeof(T2).AssemblyQualifiedName}/";
            info += $"{typeof(T3).AssemblyQualifiedName}/";
            info += typeof(T4).AssemblyQualifiedName;
            
            var call = new PersistentCall();
            call.Set(null, string.Empty, info);

            return call;
        }
    }
}