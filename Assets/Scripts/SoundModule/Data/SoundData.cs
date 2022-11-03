using AudioModule.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioModule.Data
{
    [System.Serializable]
    public class SoundData
    {
        public SoundType SoundType;
        public AudioClip SoundClip;
        [Range(0.1f,0.5f)]
        public float Volume;
        [Range(1f,2f)]
        public float Pitch;

        public AudioSource AudioSource;
    } 
}
