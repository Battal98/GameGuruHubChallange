using UnityEngine.Events;
using Extentions;
using RunnerAudioModule.Enums;

namespace RunnerAudioModule.Signals
{
    public class AudioSignals : MonoSingleton<AudioSignals>
    {
        public UnityAction<SoundType, float> onPlaySound = delegate { };
    } 
}
