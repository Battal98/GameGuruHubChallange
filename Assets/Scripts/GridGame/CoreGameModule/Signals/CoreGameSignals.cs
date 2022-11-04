using Extentions;
using UnityEngine.Events;
using Enums;

namespace GridGame.CoreGameModule.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<GameStates> onChangeGameState = delegate { };
        public UnityAction onReset = delegate { };
        public UnityAction onPlay = delegate { };
        public UnityAction<int> onUpdateGridGameScore = delegate { };
    } 
}
