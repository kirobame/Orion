﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class HighpassFilter : AudioFilter
    {
        [SerializeField, Range(10f, 22000f)] private float cutoffFrequency = 10f;
        [SerializeField, Range(1f, 10f)] private float resonance = 1f;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioHighPassFilter>();
            AssignValues(component);
        }
        
        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioHighPassFilter>();
            Object.Destroy(component);
        }
        
        public override void AssignValues(GameObject target) => AssignValues(target.GetComponent<AudioHighPassFilter>());

        private void AssignValues(AudioHighPassFilter component)
        {
            component.cutoffFrequency = cutoffFrequency;
            component.highpassResonanceQ = resonance;
        }
    }
}