using UnityEngine;
using CoreGameModule.Signals;
using LevelModule.Signals;
using LevelModule.Data;
using LevelModule.Data.ScriptableObjects;
using LevelModule.Commands;
using Sirenix.OdinInspector;

namespace LevelModule
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables


        #endregion

        #region Serialized Variables

        [Header("Data")]
        [SerializeField]
        private LevelData levelData;

        [SerializeField] 
        private GameObject levelHolder;

        #endregion

        #region Private Variables

        private ClearActiveLevelCommand _clearActiveLevel;
        private LevelLoaderCommand _levelLoader;

        [ShowInInspector] 
        private int _levelID;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _levelLoader = new LevelLoaderCommand(ref levelHolder);
            _clearActiveLevel = new ClearActiveLevelCommand(ref levelHolder);
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            //InitializeDataSignals.Instance.onLoadLevelID += OnLoadLevelID;
            LevelSignals.Instance.onLevelInitialize += OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel += OnNextLevel;
            LevelSignals.Instance.onRestartLevel += OnRestartLevel;
            //LevelSignals.Instance.onGetLevel += GetLevelCount;
        }

        private void UnsubscribeEvents()
        {
            //InitializeDataSignals.Instance.onLoadLevelID -= OnLoadLevelID;
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
            //LevelSignals.Instance.onGetLevel-= GetLevelCount;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private int GetLevelCount()
        {
            return _levelID % Resources.Load<CD_Level>("Datas/CD_Level").LevelData.Count;
        }

        private void OnInitializeLevel()
        {
            var newLevelData = GetLevelCount();
            _levelLoader.Execute(newLevelData);
            LevelSignals.Instance.onLevelInitDone?.Invoke();
        }

        private void OnClearActiveLevel()
        {
            _clearActiveLevel.Execute();
        }

        private void OnNextLevel()
        {
            _levelID++;
            //SaveLevelID(_levelID);
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        private void OnRestartLevel()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
        }
/*
        private void SaveLevelID(int levelID)
        {
            InitializeDataSignals.Instance.onSaveLevelID?.Invoke(levelID);
        }

        private void OnLoadLevelID(int levelID)
        {
            _levelID = levelID;
        }*/
    } 
}
