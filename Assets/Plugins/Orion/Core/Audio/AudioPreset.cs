using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Orion
{
    public abstract class AudioPreset
    {
        public abstract void Apply(AudioSource source);
    }
    
    public class BypassesPreset : AudioPreset
    {
        [SerializeField] private bool bypassEffects;
        [SerializeField] private bool bypassListenerEffects;
        [SerializeField] private bool bypassReverbZones;
        
        public override void Apply(AudioSource source)
        {
            source.bypassEffects = bypassEffects;
            source.bypassListenerEffects = bypassListenerEffects;
            source.bypassReverbZones = bypassReverbZones;
        }
    }
    public class CurvesPreset : AudioPreset
    {
        [SerializeField] private AnimationCurve volumeCurve = AnimationCurve.Linear(0f,1f,1f,0f);
        [SerializeField] private AnimationCurve spatialBlendCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        [SerializeField] private AnimationCurve spreadCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        [SerializeField] private AnimationCurve reverbZoneMixCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        
        public override void Apply(AudioSource source)
        {
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);
            source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, spatialBlendCurve);
            source.SetCustomCurve(AudioSourceCurveType.Spread, spreadCurve);
            source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, reverbZoneMixCurve);
        }
    }
    public class OutputPreset : AudioPreset
    {
        [SerializeField] private AudioMixerGroup group;
        [SerializeField] private bool mute;
        [SerializeField] private bool loop;
        
        public override void Apply(AudioSource source)
        {
            source.outputAudioMixerGroup = group;
            source.mute = mute;
            source.loop = loop;
        }
    }
    public class SoundPreset : AudioPreset
    {
        [SerializeField, Range(0f, 1f)] private float volume = 1f; 
        [SerializeField, Range(0, 255)] private int priority = 128;
        [SerializeField, Range(-3f, 3f)] private float pitch = 1f;
        [SerializeField, Range(-1f, 1f)] private float panStereo = 0f;

        public override void Apply(AudioSource source)
        {
            source.volume = volume;
            source.priority = priority;
            source.pitch = pitch;
            source.panStereo = panStereo;
        }
    }
    public class SpatializationPreset : AudioPreset
    {
        [SerializeField, Range(0, 5)] private float dopplerLevel = 1f;
        [SerializeField, MinMaxSlider(0, 1000)] private Vector2 range;
        
        public override void Apply(AudioSource source)
        {
            source.dopplerLevel =dopplerLevel;
            source.minDistance = range.x;
            source.maxDistance = range.y;
        }
    }
}