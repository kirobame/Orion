using UnityEngine;

namespace Orion
{
    public abstract class AudioFilter
    {
        public abstract void AddTo(GameObject target);
        public abstract void RemoveFrom(GameObject target);
    }
    
    public class ChorusFilter : AudioFilter
    {
        [SerializeField, Range(0f, 1f)] private float dryMix;
        [SerializeField, Range(0f, 1f)] private float wetMix01;
        [SerializeField, Range(0f, 1f)] private float wetMix02;
        [SerializeField, Range(0f, 1f)] private float wetMix03;
        [SerializeField, Min(0.1f)] private float delay;
        [SerializeField, Range(0f, 20f)] private float rate;
        [SerializeField, Range(0f, 1f)] private float depth;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioChorusFilter>();

            component.dryMix = dryMix;
            component.wetMix1 = wetMix01;
            component.wetMix2 = wetMix02;
            component.wetMix3 = wetMix03;
            component.delay = delay;
            component.rate = rate;
            component.depth = depth;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioChorusFilter>();
            Object.Destroy(component);
        }
    }
    public class DistorsionFilter : AudioFilter
    {
        [SerializeField, Range(0f,1f)] private float level;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioDistortionFilter>();
            component.distortionLevel = level;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioDistortionFilter>();
            Object.Destroy(component);
        }
    }
    public class EchoFilter : AudioFilter
    {
        [SerializeField, Min(10f)] private float delay;
        [SerializeField, Range(0f,1f)] private float decayRatio;
        [SerializeField, Range(0f,1f)] private float wetMix;
        [SerializeField, Range(0f,1f)] private float dryMix;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioEchoFilter>();

            component.delay = delay;
            component.decayRatio = decayRatio;
            component.wetMix = wetMix;
            component.dryMix = dryMix;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioEchoFilter>();
            Object.Destroy(component);
        }
    }
    public class HighpassFilter : AudioFilter
    {
        [SerializeField, Range(10f, 22000f)] private float cutoffFrequency;
        [SerializeField, Range(1f, 10f)] private float resonance;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioHighPassFilter>();

            component.cutoffFrequency = cutoffFrequency;
            component.highpassResonanceQ = resonance;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioHighPassFilter>();
            Object.Destroy(component);
        }
    }
    public class LowpassFilter : AudioFilter
    {
        [SerializeField, Range(10f, 22000f)] private float cutoffFrequency;
        [SerializeField, Range(1f, 10f)] private float resonance;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioLowPassFilter>();

            component.cutoffFrequency = cutoffFrequency;
            component.lowpassResonanceQ = resonance;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioLowPassFilter>();
            Object.Destroy(component);
        }
    }
    public class ReverbFilter : AudioFilter
    {
        [SerializeField] private AudioReverbPreset preset;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioReverbFilter>();
            component.reverbPreset = preset;
        }

        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioReverbFilter>();
            Object.Destroy(component);
        }
    }
}