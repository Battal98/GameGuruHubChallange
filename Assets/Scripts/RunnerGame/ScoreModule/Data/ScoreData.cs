using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using System;

namespace RunnerScoreModule.Data
{
    [Serializable]
    public class ScoreData : ISavable
    {
        public int TotalMoneyScore;
        public int TotalGemScore;
        public int TotalStarScore;

        private const SaveLoadType Key = SaveLoadType.ScoreData;
        public ScoreData(int gemScore, int moneyScore, int starScore)
        {
            TotalMoneyScore = moneyScore;
            TotalGemScore = gemScore;
            TotalStarScore = starScore;
        }
        public ScoreData()
        {

        }
        public SaveLoadType GetKey()
        {
            return Key;
        }
    } 
}
