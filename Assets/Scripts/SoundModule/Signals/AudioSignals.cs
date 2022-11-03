using UnityEngine.Events;
using Extentions;
using AudioModule.Enums;

namespace AudioModule.Signals
{
    public class AudioSignals : MonoSingleton<AudioSignals>
    {
        public UnityAction<SoundType, float> onPlaySound = delegate { };
    } 
}
