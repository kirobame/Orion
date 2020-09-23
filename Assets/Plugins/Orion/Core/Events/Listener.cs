using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    public abstract class Listener<T> : SerializedMonoBehaviour
    {
        public OrionEvent<T> Callback => callback;
        
        [SerializeField] private RemoteEvent<T>[] remoteEvents = new RemoteEvent<T>[0];
        [SerializeField] private OrionEvent<T> callback = new OrionEvent<T>();

        private void OnEnable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call += callback.Invoke; }
        private void OnDisable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call -= callback.Invoke; }
    }
    
    public abstract class Listener<T1,T2> : SerializedMonoBehaviour
    {
        public OrionEvent<T1,T2> Callback => callback;
        
        [SerializeField] private RemoteEvent<T1,T2>[] remoteEvents;
        [SerializeField] private OrionEvent<T1,T2> callback;

        private void OnEnable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call += callback.Invoke; }
        private void OnDisable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call -= callback.Invoke; }
    }
    
    public abstract class Listener<T1,T2,T3> : SerializedMonoBehaviour
    {
        public OrionEvent<T1,T2,T3> Callback => callback;
        
        [SerializeField] private RemoteEvent<T1,T2,T3>[] remoteEvents;
        [SerializeField] private OrionEvent<T1,T2,T3> callback;

        private void OnEnable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call += callback.Invoke; }
        private void OnDisable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call -= callback.Invoke; }
    }
}