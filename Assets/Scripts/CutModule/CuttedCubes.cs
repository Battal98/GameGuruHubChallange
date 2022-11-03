using UnityEngine;
using PoolModule.Signals;
using PoolModule.Interfaces;
using PoolModule.Enums;
using DG.Tweening;

public class CuttedCubes : MonoBehaviour, IReleasePoolObject
{
    private const float _time = 1f;
    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
    }

    private void OnEnable()
    {
        DOVirtual.DelayedCall(_time, ()=>  ReleaseObject(this.gameObject, PoolType.CuttedCubes));
    }
}
