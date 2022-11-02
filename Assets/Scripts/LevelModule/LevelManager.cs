using UnityEngine;
using CoreGameModule.Signals;
using LevelModule.Signals;
using LevelModule.Data;
using LevelModule.Data.ScriptableObjects;
using LevelModule.Commands;
using Sirenix.OdinInspector;
using SaveLoadModule.Signals;
using SaveLoadModule.Enums;

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
        private int _uniqeID = 1234;
        private int _value;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            InitLevelData();
        }
        private void InitLevelData()
        {
            _levelID = GetLevelCount();
            levelData = GetLevelData();
            _value = levelData.StackCount;
            if (!ES3.FileExists(this.levelData.GetKey().ToString() + $"{_uniqeID}.es3")) // _levelId asl�nda uniqeid
            {
                if (!ES3.KeyExists(this.levelData.GetKey().ToString()))
                {
                    _levelID = GetLevelCount();
                    levelData = GetLevelData();
                    _value = levelData.StackCount;
                    Save(_uniqeID);
                }
            }
            Load(_uniqeID);
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
            LevelSignals.Instance.onGetLevel += GetLevelCount;
        }

        private void UnsubscribeEvents()
        {
            //InitializeDataSignals.Instance.onLoadLevelID -= OnLoadLevelID;
            LevelSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            LevelSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            LevelSignals.Instance.onNextLevel -= OnNextLevel;
            LevelSignals.Instance.onRestartLevel -= OnRestartLevel;
            LevelSignals.Instance.onGetLevel-= GetLevelCount;
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
        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Datas/CD_Level").LevelData[_levelID];
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
            Save(_uniqeID);
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        private void OnRestartLevel()
        {
            LevelSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            LevelSignals.Instance.onLevelInitialize?.Invoke();
        }
        public void Load(int uniqeID)
        {
            LevelData _data = SaveLoadSignals.Instance.onLoadLevelData?.Invoke(SaveLoadType.LevelData, uniqeID);
            levelData = _data;
            _value = levelData.StackCount;
        }

        public void Save(int uniqeID)
        {
            LevelData newlevelData = new LevelData(_value);
            SaveLoadSignals.Instance.onSaveLevelData?.Invoke(newlevelData, uniqeID);
        }
    } 
}
