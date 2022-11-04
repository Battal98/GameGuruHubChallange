using Extentions;
using UnityEngine.Events;

namespace RunnerInputModule.Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction onClick = delegate { };
    } 
}
