using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public interface IAudioProvider
    {
        AudioClip GetClip();
        
        IReadOnlyList<AudioPreset> Presets { get; }
        IReadOnlyList<AudioFilter> Filters { get; }
    }
}