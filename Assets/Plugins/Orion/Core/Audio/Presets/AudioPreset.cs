using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [HideReferenceObjectPicker, HideLabel]
    public abstract class AudioPreset
    {
        public abstract void Apply(AudioSource source);
    }
}