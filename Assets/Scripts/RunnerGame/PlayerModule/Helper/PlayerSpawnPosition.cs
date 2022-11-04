using UnityEngine;
using RunnerCoreGameModule.Signals;

public class PlayerSpawnPosition : MonoBehaviour
{
    private void OnEnable()
    {
        CoreGameSignals.Instance.onSetPlayerSpawnPosition?.Invoke(this.transform.position);
    }
}
