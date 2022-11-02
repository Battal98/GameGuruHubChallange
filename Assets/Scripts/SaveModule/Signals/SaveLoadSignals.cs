using UnityEngine.Events;
using Extentions;
using System;
using LevelModule.Data ;
using SaveLoadModule.Enums;
using ScoreModule.Data;

namespace SaveLoadModule.Signals
{
    public class SaveLoadSignals : MonoSingleton<SaveLoadSignals>
    {
        public UnityAction<LevelData, int> onSaveLevelData = delegate { };
        public Func<SaveLoadType, int, LevelData> onLoadLevelData;
        public UnityAction<ScoreData, int> onSaveScoreData = delegate { };
        public Func<SaveLoadType,int, ScoreData> onLoadScoreData;
    } 
}
