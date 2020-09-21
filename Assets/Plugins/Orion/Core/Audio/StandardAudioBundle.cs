using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewAudioBundle", menuName = "Orion/Audio/Bundles/Standard")]
    public class StandardAudioBundle : AudioBundle
    {
        [SerializeField, PropertyOrder(-1)] private AudioClip clip;

        public override AudioClip GetClip() => clip;
    }
}