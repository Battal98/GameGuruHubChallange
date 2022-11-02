using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using System;

namespace LevelModule.Data
{
    [Serializable]
    public class LevelData: ISavable
    {
        public int Value;

        private const SaveLoadType Key = SaveLoadType.LevelData;
        public LevelData(int value)
        {
            Value = value;  
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
