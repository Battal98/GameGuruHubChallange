using SaveLoadModule.Enums;
using SaveLoadModule.Interfaces;
using System;

namespace LevelModule.Data
{
    [Serializable]
    public class LevelData: ISavable
    {
        public int StackCount;

        private const SaveLoadType Key = SaveLoadType.LevelData;
        public LevelData(int stackCount)
        {
            StackCount = stackCount;  
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
