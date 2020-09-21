using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public abstract class AudioBundle : SerializedScriptableObject, IAudioProvider
    {
        public IReadOnlyList<AudioPreset> Presets => presets;
        public IReadOnlyList<AudioFilter> Filters => filters;
        
        [SerializeField] private AudioPreset[] presets = new AudioPreset[0];
        [SerializeField] private AudioFilter[] filters = new AudioFilter[0];

        public abstract AudioClip GetClip();
        
        [Button]
        private void Verify()
        {
            presets = presets.Where(preset => preset != null && presets.Count(other => other.GetType() == preset.GetType()) == 1).ToArray();
            filters = filters.Where(filter => filter != null && filters.Count(other => other.GetType() == filter.GetType()) == 1).ToArray();
        }
    }
}