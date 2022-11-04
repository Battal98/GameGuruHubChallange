using UnityEngine.Events;
using Extentions;

namespace GridGame.GridModule.Signals
{
    public class GridSignals : MonoSingleton<GridSignals>
    {
        public UnityAction<int> onCreateGrid = delegate { };
    } 
}
