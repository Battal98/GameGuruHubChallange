using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveLoadModule.Interfaces;
using SaveLoadModule.Enums;
using LevelModule.Data;

namespace SaveLoadModule.Abstractions
{
    public abstract class SaveLoadBase : ISavable
    {
        public virtual SaveLoadType GetKey()
        {
            return SaveLoadType.LevelData;
        }

        public virtual LevelData LevelDatas()
        {
            return null;
        }

        public virtual void ScoreData()
        {

        }
    }

}