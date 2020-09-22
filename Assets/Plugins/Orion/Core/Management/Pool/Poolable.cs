using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Orion
{
    public abstract class Poolable : SerializedMonoBehaviour
    {
        //[FoldoutGroup("Events")] public OrionEvent onReboot = new OrionEvent();
    
        private Pool origin;
        
        protected virtual void OnDisable()
        {
            origin.Stock(this);
            origin = null;
        }

        public void SetOrigin(Pool origin) => this.origin = origin;

        public void Reboot()
        {
            //onReboot.Invoke();
            OnReboot();
        }
        public abstract void OnReboot();
    }
}