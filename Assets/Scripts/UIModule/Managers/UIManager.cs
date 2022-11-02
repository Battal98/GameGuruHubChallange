using UnityEngine;
using UIModules.Enums;
using Enums;
using UIModules.Controllers;
using System.Collections.Generic;
using TMPro;
using CoreGameModule.Signals;
using UIModules.Signals;
using LevelModule.Signals;

namespace UIModules.Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField]
        private List<GameObject> panels;
        [SerializeField]
        private TextMeshProUGUI gemText;
        [SerializeField]
        private TextMeshProUGUI coinText;        
        [SerializeField]
        private TextMeshProUGUI levelText;  
        [SerializeField]
        private TextMeshProUGUI starText;
        #endregion

        #region Private Variables

        private UIPanelControllers _uiPanelController;
        private LevelPanelController _levelPanelController;

        #endregion

        #endregion

        private void Awake()
        {
            _uiPanelController = new UIPanelControllers(panels);
            _levelPanelController = new LevelPanelController(gemText, coinText, levelText, starText);
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onOpenPanel += OnOpenPanel;
            UISignals.Instance.onClosePanel += OnClosePanel;
            UISignals.Instance.onUpdateCoinScoreText += OnUpdateCoinScore;
            UISignals.Instance.onUpdateGemScoreText += OnUpdateGemScore;
            UISignals.Instance.onUpdateStarScoreText += OnUpdateStarScore;

            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;

            LevelSignals.Instance.onLevelInitialize += OnLevelInitialize;
            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            
        }

        private void UnsubscribeEvents()
        {
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            UISignals.Instance.onUpdateCoinScoreText -= OnUpdateCoinScore;
            UISignals.Instance.onUpdateGemScoreText -= OnUpdateGemScore;
            UISignals.Instance.onUpdateStarScoreText -= OnUpdateStarScore;

            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;

            LevelSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnOpenPanel(PanelTypes panelParam)
        {
            _uiPanelController.OpenPanel(panelParam);
        }

        private void OnClosePanel(PanelTypes panelParam)
        {
            _uiPanelController.ClosePanel(panelParam);
        }

        private void InitPanels()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.LevelPanel);
            _uiPanelController.OpenPanel(PanelTypes.StartPanel);
        }

        private void OnReset()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.LevelPanel);
        }

        private void OnPlay()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.LevelPanel);
        }

        private void OnLevelFailed()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.FailedPanel);
        }

        private void OnLevelSuccessful()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.WinPanel);
        }

        private void OnLevelInitialize()
        {
            InitPanels();
            _levelPanelController.SetLevelText(LevelSignals.Instance.onGetLevel.Invoke() + 1);
        }

        public void PlayButton()
        {
            Debug.Log("click");
            CoreGameSignals.Instance.onPlay?.Invoke();
        }

        public void NextLevelButton()
        {
            LevelSignals.Instance.onNextLevel?.Invoke();
        }

        public void RestartButton()
        {
            _uiPanelController.CloseAllPanel();
            _uiPanelController.OpenPanel(PanelTypes.LevelPanel);
            LevelSignals.Instance.onRestartLevel?.Invoke();
            CoreGameSignals.Instance.onPlay?.Invoke();
        }

        private void OnUpdateGemScore(int gemValue)
        {
            _levelPanelController.SetGemScoreText(gemValue);
        }

        private void OnUpdateCoinScore(int coinValue)
        {
            _levelPanelController.SetCoinScoreText(coinValue);
        }

        private void OnUpdateStarScore(int moneyValue)
        {
            _levelPanelController.SetStarScoreText(moneyValue);
        }
    } 
}
