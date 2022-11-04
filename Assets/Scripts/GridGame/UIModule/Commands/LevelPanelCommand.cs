using TMPro;

namespace GridGame.UIModule.Commands
{
    public class LevelPanelCommand
    {

        #region Self Variables

        #region Private Variables

        private readonly TextMeshProUGUI _gridScoreText;

        #endregion

        #endregion

        public LevelPanelCommand(TextMeshProUGUI gridScoreText)
        {
            _gridScoreText = gridScoreText;
        }

        public void SetGridScoreText(int gemValue)
        {
            _gridScoreText.text = gemValue.ToString();
        }
    }
}
