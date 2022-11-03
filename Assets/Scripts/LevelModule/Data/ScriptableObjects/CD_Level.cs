using System.Collections.Generic;
using UnityEngine;

namespace LevelModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "StackGame/CD_Level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public LevelData LevelData;
        /*public ScoreData ScoreData;*/
    }
}
