using Extentions;
using UnityEngine.Events;

namespace GridGame.InputModule.Signals
{

    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction onClick = delegate { };
    }
}
