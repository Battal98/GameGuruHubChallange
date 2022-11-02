using TMPro;

namespace UIModules.Controllers
{
    public class LevelPanelController
    {

        #region Self Variables

        #region Private Variables

        private readonly TextMeshProUGUI _gemText;
        private readonly TextMeshProUGUI _moneyText;
        private readonly TextMeshProUGUI _levelText;
        private readonly TextMeshProUGUI _starText;

        #endregion

        #endregion

        public LevelPanelController(TextMeshProUGUI gemText, TextMeshProUGUI moneyText, TextMeshProUGUI levelText, TextMeshProUGUI starText)
        {
            _gemText = gemText;
            _moneyText = moneyText;
            _levelText = levelText;
            _starText = starText;
        }

        public void SetGemScoreText(int gemValue)
        {
            _gemText.text = gemValue.ToString();
        }

        public void SetCoinScoreText(int moneyValue)
        {
            _moneyText.text = moneyValue.ToString();
        }

        public void SetLevelText(int levelValue)
        {
            _levelText.text = "LEVEL " + levelValue.ToString();
        }

        public void SetStarScoreText(int starValue)
        {
            _starText.text = starValue.ToString();
        }
    }
}
