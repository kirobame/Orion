using System.Collections;
using UnityEngine;

namespace Orion
{
    public class SoundEffect : Feedback
    {
        [SerializeField] private IProxy<AudioSource> audioProxy;
        private AudioSource audio
        {
            get => audioProxy.Read();
            set => audioProxy.Write(value);
        }

        [SerializeField] private AudioClip clip;
        
        public override IEnumerator GetRoutine()
        {
            audio.clip = clip;
            audio.Play();

            Complete();
            
            yield break;
        }
    }
}