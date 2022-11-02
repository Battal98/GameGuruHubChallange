using System.Collections.Generic;
using UnityEngine;

namespace LevelModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "GameName/CD_Level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public List<LevelData> LevelData = new List<LevelData>();
        public int LevelID;
        /*public ScoreData ScoreData;*/
    }
}
