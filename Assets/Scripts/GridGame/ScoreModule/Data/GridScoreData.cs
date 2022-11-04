using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;

namespace GridGame.ScoreModule.Data
{
    [System.Serializable]
    public class GridScoreData : ISavable
    {
        public int Score;
        private const SaveLoadType Key = SaveLoadType.GridScoreData;

        public GridScoreData(int score)
        {
            Score = score;
        }

        public GridScoreData()
        {

        }

        public SaveLoadType GetKey()
        {
            return Key;
        }
    } 
}
