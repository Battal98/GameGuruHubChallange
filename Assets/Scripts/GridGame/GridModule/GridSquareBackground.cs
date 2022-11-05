using PoolModule.Interfaces;
using PoolModule.Signals;
using UnityEngine;
using PoolModule.Enums;
using System.Collections.Generic;

public class GridSquareBackground : MonoBehaviour, IReleasePoolObject
{
    public int Index;
    public AvaibleType AvaibleType = AvaibleType.Unlock;
    public List<GridSquareBackground> MyNeighbors = new List<GridSquareBackground>();

    private void OnEnable()
    {
        AvaibleType = AvaibleType.Unlock;
        MyNeighbors.Clear();
        MyNeighbors.TrimExcess();
        if (this.transform.childCount > 0)
            ReleaseObject(this.transform.GetChild(0).gameObject, PoolType.GridCrownObject);
    }

    public void GetNeighbors(GridSquareBackground gameObject)
    {
        MyNeighbors.Add(gameObject);
    }

    public GridSquareBackground GetNeighborSpesificIndex(int index)
    {
        return MyNeighbors[index];
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
