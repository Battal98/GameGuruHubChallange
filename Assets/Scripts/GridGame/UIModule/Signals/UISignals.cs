using Extentions;
using UnityEngine.Events;
using GridGame.UIModule.Enums;

namespace GridGame.UIModule.Signals
{
    public class UISignals : MonoSingleton<UISignals>
    {
        public UnityAction<PanelTypes> onOpenPanel = delegate { };
        public UnityAction<PanelTypes> onClosePanel = delegate { };
        public UnityAction<int> onUpdateGridScoreText = delegate { };
    }
}
