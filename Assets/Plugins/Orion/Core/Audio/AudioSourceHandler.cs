using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public class AudioSourceHandler : PoolableWrapper<AudioSource>
    {
        private static Dictionary<Type,int> hashingTable = new Dictionary<Type, int>()
        {
            {typeof(OutputPreset), 0},
            {typeof(SoundPreset), 1},
            {typeof(BypassesPreset), 2},
            {typeof(SpatializationPreset), 3},
            {typeof(CurvesPreset), 4},
        };
        private static AudioPreset[] standardPresets = new AudioPreset[]
        {
            new OutputPreset(),
            new SoundPreset(), 
            new BypassesPreset(), 
            new SpatializationPreset(), 
            new CurvesPreset()
        };

        private List<AudioFilter> filters = new List<AudioFilter>();

        public void Play(AudioClip clip)
        {
            if (value.isPlaying) return;

            value.clip = clip;
            value.Play();
            
            if (value.loop) return;
            
            var routine = new WaitForSeconds(clip.length + Time.fixedDeltaTime).ToRoutine();
            routine.Append(new ActionRoutine() {action = () => gameObject.SetActive(false)});

            StartCoroutine(routine.Call);
        }
        
        public void SetPresets(IEnumerable<AudioPreset> presets)
        {
            var indices = new List<int>() {0,1,2,3,4};
            foreach (var preset in presets)
            {
                preset.Apply(value);
                indices.Remove(hashingTable[preset.GetType()]);
            }

            foreach (var index in indices) standardPresets[index].Apply(value);
        }
        
        public void AddFilter(AudioFilter filter)
        {
            filter.AddTo(value.gameObject);
            filters.Add(filter);
        }
        
        public override void OnReboot()
        {
            base.OnReboot();
            
            foreach (var filter in filters) filter.RemoveFrom(value.gameObject);
            filters.Clear();
        }
    }
}