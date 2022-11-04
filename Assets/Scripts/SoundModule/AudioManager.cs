using RunnerAudioModule.Data.ScriptableObjects;
using UnityEngine;
using RunnerAudioModule.Signals;
using RunnerAudioModule.Enums;
using System.Collections.Generic;

namespace RunnerAudioModule
{
    public class AudioManager : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables
        [SerializeField]
        private List<AudioSource> Sources = new List<AudioSource>();
        #endregion

        #region Private Variables
        private CD_Sound _cdSound;
        #endregion

        #endregion
        private void Awake()
        {
            _cdSound = GetSoundData();
            foreach (var item in _cdSound.SoundData)
            {
                item.AudioSource = Sources[(int)item.SoundType];

                item.AudioSource.clip = item.SoundClip;
                item.AudioSource.volume = item.Volume;
                item.AudioSource.pitch = item.Pitch;
            }
        }
        private CD_Sound GetSoundData()
        {
            return Resources.Load<CD_Sound>("Datas/CD_Sound");
        }

        #region Events Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AudioSignals.Instance.onPlaySound += OnPlaySound;
        }
        private void UnsubscribeEvents()
        {
            AudioSignals.Instance.onPlaySound = OnPlaySound;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void OnPlaySound(SoundType arg0, float pitchValue)
        {
            Sources[(int)arg0].Play();
            Sources[(int)arg0].pitch = pitchValue;
        }
    } 
}
