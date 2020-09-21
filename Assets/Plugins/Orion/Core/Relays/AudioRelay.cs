using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Orion
{
    [CreateAssetMenu(fileName = "AudioRelay", menuName = "Orion/Relays/Audio")]
    public class AudioRelay : ScriptableObject
    {
        public void Play(IAudioProvider provider, Token handlerToken)
        {
            var handler = Repository.Get<AudioSourceHandler>(handlerToken);
            Play(provider, handler);
        }
        public void Play(IAudioProvider provider, Token handlerToken, DynamicKey<AudioSourceHandler> registry)
        {
            var handler = Repository.Get<AudioSourceHandler>(handlerToken);
            Play(provider, handler);
            
            registry.Register(handler);
        }

        public void Play(Token poolToken, IAudioProvider provider)
        {
            var pool = Repository.Get<AudioPool>(poolToken);
            var audio = pool.RequestSingle();
            
            Play(provider, audio);
        }
        public void Play(Token poolToken, IAudioProvider provider, DynamicKey<AudioSourceHandler> registry)
        {
            var pool = Repository.Get<AudioPool>(poolToken);
            var audio = pool.RequestSingle();
            
            Play(provider, audio);
            
            registry.Register(audio);
        }

        public void Play(IAudioProvider provider, AudioSourceHandler handler)
        {
            handler.SetPresets(provider.Presets);
            foreach (var filter in provider.Filters) handler.AddFilter(filter);

            handler.Play(provider.GetClip());
        }
    }
}