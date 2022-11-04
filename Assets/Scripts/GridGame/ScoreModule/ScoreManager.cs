using GridGame.CoreGameModule.Signals;
using SaveLoadModule.Enums;
using SaveLoadModule.Signals;
using GridGame.ScoreModule.Data;
using GridGame.ScoreModule.Data.ScriptableObjects;
using UnityEngine;

namespace GridGame.ScoreModule
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private GridScoreData _gridScoreData;
        private const int _uniqeID = 1234;

        private void Start()
        {
            InitLevelData();
        }
        private void InitLevelData()
        {
            _gridScoreData = GetScoreData();
            if (!ES3.FileExists(_gridScoreData.GetKey().ToString() + $"{_uniqeID}.es3"))
            {
                if (!ES3.KeyExists(_gridScoreData.GetKey().ToString()))
                {
                    _gridScoreData = GetScoreData();
                    SaveGameScoreData(_gridScoreData, _uniqeID);
                }
            }
            LoadGameScoreData();
        }

        private GridScoreData GetScoreData()
        {
            return Resources.Load<CD_GridScore>("Datas/CD_GridScore").GridScoreData;
        }

        private void LoadGameScoreData()
        {
            _gridScoreData = SaveLoadSignals.Instance.onLoadGridScoreData.Invoke(SaveLoadType.GridScoreData, _uniqeID);
            SetGameScore();
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onUpdateGridGameScore += OnUpdateGridGameScore;
        }
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onUpdateGridGameScore -= OnUpdateGridGameScore;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
        private void SetGameScore()
        {
            //UISignals.Instance.onUpdateStarScoreText?.Invoke(_scoreData.Score);
        }

        private void OnUpdateGridGameScore(int _amount)
        {
            _gridScoreData.Score += _amount;
            //UISignals.Instance.onUpdateStarScoreText?.Invoke(_scoreData.Score);
            SaveGameScoreData(_gridScoreData, _uniqeID);
        }

        private void SaveGameScoreData(GridScoreData scoreData, int uniqeID) => SaveLoadSignals.Instance.onSaveGridScoreData?.Invoke(scoreData, uniqeID);

        private void OnApplicationQuit()
        {
            SaveGameScoreData(_gridScoreData, _uniqeID);
        }
    }
}
