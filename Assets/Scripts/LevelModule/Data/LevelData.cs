using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using System;

namespace LevelModule.Data
{
    [Serializable]
    public class LevelData: ISavable
    {
        public int LevelID;
        public int LevelIDForText;

        private const SaveLoadType Key = SaveLoadType.LevelData;
        public LevelData(int levelID, int levelIDForText)
        {
            LevelID = levelID;  
            LevelIDForText = levelIDForText;
        }
        public LevelData()
        {

        }

        public SaveLoadType GetKey()
        {
            return Key;
        }
    } 
}
