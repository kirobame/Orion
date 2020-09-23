using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class VoidListener : SerializedMonoBehaviour
    {
        public OrionEvent Callback => callback;
        
        [SerializeField] private RemoteVoidEvent[] remoteEvents = new RemoteVoidEvent[0];
        [SerializeField] private OrionEvent callback = new OrionEvent();

        private void OnEnable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call += callback.Invoke; }
        private void OnDisable() { foreach (var remoteEvent in remoteEvents) remoteEvent.Call -= callback.Invoke; }
    }
}