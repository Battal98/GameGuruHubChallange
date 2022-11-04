using UnityEngine;
using GridGame.UIModule.Enums;
using GridGame.CoreGameModule.Signals;
using GridGame.UIModule.Signals;
using System.Collections.Generic;
using TMPro;
using GridGame.UIModule.Commands;
using GridGame.GridModule.Signals;
using System;

namespace GridGame.UIModule
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField]
        private List<GameObject> panels;
        [SerializeField]
        private TextMeshProUGUI gridScoreText;
        [SerializeField]
        private TMP_InputField gridInput;
        #endregion

        #region Private Variables

        private UIPanelCommand _uiPanelCommand;
        private LevelPanelCommand _levelPanelCommand;

        #endregion

        #endregion

        private void Awake()
        {
            _uiPanelCommand = new UIPanelCommand(panels);
            _levelPanelCommand = new LevelPanelCommand(gridScoreText);
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
            UISignals.Instance.onUpdateGridScoreText += OnUpdateGridScore;

            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;

        }

        private void UnsubscribeEvents()
        {
            UISignals.Instance.onOpenPanel -= OnOpenPanel;
            UISignals.Instance.onClosePanel -= OnClosePanel;
            UISignals.Instance.onUpdateGridScoreText -= OnUpdateGridScore;

            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnOpenPanel(PanelTypes panelParam)
        {
            _uiPanelCommand.OpenPanel(panelParam);
        }

        private void OnClosePanel(PanelTypes panelParam)
        {
            _uiPanelCommand.ClosePanel(panelParam);
        }

        private void InitPanels()
        {
            _uiPanelCommand.CloseAllPanel();
            _uiPanelCommand.OpenPanel(PanelTypes.GamePanel);
        }

        private void OnReset()
        {
            _uiPanelCommand.CloseAllPanel();
            _uiPanelCommand.OpenPanel(PanelTypes.GamePanel);
        }

        private void OnPlay()
        {
            _uiPanelCommand.CloseAllPanel();
            _uiPanelCommand.OpenPanel(PanelTypes.GamePanel);
        }

        public void PlayButton()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
        }

        public void CreateButton()
        {
            var value = Int32.Parse(gridInput.text);
            GridSignals.Instance.onCreateGrid?.Invoke(value);
        }

        public void RestartButton()
        {
            _uiPanelCommand.CloseAllPanel();
            _uiPanelCommand.OpenPanel(PanelTypes.GamePanel);
            CoreGameSignals.Instance.onPlay?.Invoke();
        }

        private void OnUpdateGridScore(int coinValue)
        {
            _levelPanelCommand.SetGridScoreText(coinValue);
        }

    }
}
