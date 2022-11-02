using UnityEngine;
using SaveLoadModule.Command;
using SaveLoadModule.Signals;
using LevelModule.Data;
using ScoreModule.Data;

namespace SaveLoadModule
{
    public class SaveLoadManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private LoadGameCommand _loadGameCommand;
        private SaveGameCommand _saveGameCommand;


        #endregion

        #endregion

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _loadGameCommand = new LoadGameCommand();
            _saveGameCommand = new SaveGameCommand();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveLevelData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadLevelData += _loadGameCommand.Execute<LevelData>;            
            SaveLoadSignals.Instance.onSaveScoreData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadScoreData += _loadGameCommand.Execute<ScoreData>;
        }

        private void UnsubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveLevelData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadLevelData -= _loadGameCommand.Execute<LevelData>;
            SaveLoadSignals.Instance.onSaveScoreData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadScoreData -= _loadGameCommand.Execute<ScoreData>;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
    } 
}
