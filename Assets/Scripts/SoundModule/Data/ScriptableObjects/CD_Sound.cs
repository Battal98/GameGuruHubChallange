using System.Collections.Generic;
using UnityEngine;

namespace RunnerAudioModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Sound", menuName = "StackGame/CD_Sound", order = 0)]
    public class CD_Sound : ScriptableObject
    {
        public List<SoundData> SoundData;
    } 
}
