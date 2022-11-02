using CoreGameModule.Signals;
using LevelModule.Signals;
using SaveLoadModule.Enums;
using SaveLoadModule.Signals;
using ScoreModule.Data;
using ScoreModule.Data.ScriptableObjects;
using UIModules.Signals;
using UnityEngine;

namespace ScoreModule
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private ScoreData _scoreData;
        private const int _uniqeID = 1234;

        private void Start()
        {
            InitLevelData();
        }
        private void InitLevelData()
        {
            _scoreData = GetScoreData();
            if (!ES3.FileExists(_scoreData.GetKey().ToString() + $"{_uniqeID}.es3")) 
            {
                if (!ES3.KeyExists(_scoreData.GetKey().ToString()))
                {
                    _scoreData = GetScoreData();
                    SaveGameScoreData(_scoreData,_uniqeID);
                }
            }
            LoadGameScoreData();
        }

        private ScoreData GetScoreData()
        {
            return Resources.Load<CD_Score>("Datas/CD_Score").ScoreData;
        }

        private void LoadGameScoreData()
        {
            _scoreData = SaveLoadSignals.Instance.onLoadScoreData.Invoke(SaveLoadType.ScoreData, _uniqeID);
            SetGameScore();
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onUpdateGemScore += OnUpdateGemScore;
            CoreGameSignals.Instance.onUpdateCoinScore += OnUpdateCoinScore;
            CoreGameSignals.Instance.onUpdateStarScore += OnUpdateStarScore;
        }
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onUpdateGemScore -= OnUpdateGemScore;
            CoreGameSignals.Instance.onUpdateCoinScore -= OnUpdateCoinScore;
            CoreGameSignals.Instance.onUpdateStarScore -= OnUpdateStarScore;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        private void SetGameScore()
        {
            UISignals.Instance.onUpdateCoinScoreText?.Invoke(_scoreData.TotalMoneyScore);
            UISignals.Instance.onUpdateGemScoreText?.Invoke(_scoreData.TotalGemScore);
            UISignals.Instance.onUpdateStarScoreText?.Invoke(_scoreData.TotalStarScore);
        }

        private void OnUpdateCoinScore(int _amount)
        {
            _scoreData.TotalMoneyScore += _amount;
            UISignals.Instance.onUpdateCoinScoreText?.Invoke(_scoreData.TotalMoneyScore);
            SaveGameScoreData(_scoreData, _uniqeID);
        }

        private void OnUpdateGemScore(int _amount)
        {
            _scoreData.TotalGemScore += _amount;
            UISignals.Instance.onUpdateGemScoreText?.Invoke(_scoreData.TotalGemScore);
            SaveGameScoreData(_scoreData,_uniqeID);
        }
        private void OnUpdateStarScore(int _amount)
        {
            _scoreData.TotalStarScore += _amount;
            UISignals.Instance.onUpdateStarScoreText?.Invoke(_scoreData.TotalStarScore);
            SaveGameScoreData(_scoreData, _uniqeID);
        }

        private void SaveGameScoreData(ScoreData scoreData, int uniqeID) => SaveLoadSignals.Instance.onSaveScoreData?.Invoke(scoreData, uniqeID);

        private void OnApplicationQuit()
        {
            SaveGameScoreData(_scoreData, _uniqeID);
        }
    } 
}
