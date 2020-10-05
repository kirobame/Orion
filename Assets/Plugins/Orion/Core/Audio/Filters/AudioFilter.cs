using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    [HideReferenceObjectPicker, HideLabel]
    public abstract class AudioFilter 
    {
        public abstract void AddTo(GameObject target);

        public abstract void RemoveFrom(GameObject target);

        public abstract void AssignValues(GameObject target);
    }
}