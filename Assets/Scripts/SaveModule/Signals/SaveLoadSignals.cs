using UnityEngine.Events;
using Extentions;
using System;
using RunnerLevelModule.Data ;
using SaveLoadModule.Enums;
using RunnerScoreModule.Data;
using GridGame.ScoreModule.Data;

namespace SaveLoadModule.Signals
{
    public class SaveLoadSignals : MonoSingleton<SaveLoadSignals>
    {
        public UnityAction<LevelData, int> onSaveLevelData = delegate { };
        public Func<SaveLoadType, int, LevelData> onLoadLevelData;
        public UnityAction<ScoreData, int> onSaveScoreData = delegate { };
        public Func<SaveLoadType,int, ScoreData> onLoadScoreData;
        public UnityAction<GridScoreData, int> onSaveGridScoreData = delegate { };
        public Func<SaveLoadType, int, GridScoreData> onLoadGridScoreData;
    } 
}
