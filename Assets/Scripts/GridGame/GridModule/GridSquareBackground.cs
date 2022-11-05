using PoolModule.Interfaces;
using PoolModule.Signals;
using UnityEngine;
using PoolModule.Enums;
using System.Collections.Generic;

public class GridSquareBackground : MonoBehaviour, IReleasePoolObject
{
    public int Index;
    public AvaibleType AvaibleType = AvaibleType.Unlock;
    [SerializeField]
    private List<GridSquareBackground> neighbors = new List<GridSquareBackground>();

    private void OnEnable()
    {
        AvaibleType = AvaibleType.Unlock;
        neighbors.Clear();
        neighbors.TrimExcess();
        if (this.transform.childCount > 0)
            ReleaseObject(this.transform.GetChild(0).gameObject, PoolType.GridCrossObject);
    }

    public void GetNeighbors(GridSquareBackground gameObject)
    {
        neighbors.Add(gameObject);
    }

    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
    }
}

public enum AvaibleType
{
    Lock,
    Unlock,
}
