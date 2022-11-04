using System.Collections.Generic;
using UnityEngine;
using GridGame.UIModule.Enums;
using System;

namespace GridGame.UIModule.Commands
{
    public class UIPanelCommand
    {
        #region Self Variables

        #region Private Variables

        private List<GameObject> _panels;

        #endregion

        #endregion

        public UIPanelCommand(List<GameObject> panels)
        {
            _panels = panels;
        }

        public void OpenPanel(PanelTypes panelParam)
        {
            _panels[(int)panelParam].SetActive(true);
        }

        public void ClosePanel(PanelTypes panelParam)
        {
            _panels[(int)panelParam].SetActive(false);
        }

        public void CloseAllPanel()
        {
            var panelCount = Enum.GetNames(typeof(PanelTypes)).Length;
            for (int i = 0; i < panelCount; i++)
                _panels[i].SetActive(false);
        }
    }
}
