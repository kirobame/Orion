using System;
using UnityEngine;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewRemoteVoidEvent", menuName = "Orion/Remote/Event/Void", order = -6000)]
    public class RemoteVoidEvent : ScriptableObject
    {
        public event Action Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action call;

        public void Invoke() => call?.Invoke();
    }
}