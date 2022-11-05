using PoolModule.Signals;
using PoolModule.Interfaces;
using UnityEngine;
using PoolModule.Enums;
using System.Collections.Generic;

public class GridClickCommand: IGetPoolObject
{
    public GridClickCommand()
    {
    }
    public void Click(List<Vector2> neighborList)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out GridSquareBackground gridSquareManager))
            {
                if (gridSquareManager.AvaibleType != AvaibleType.Lock)
                {
                    var obj = GetObject(PoolType.GridCrossObject);
                    obj.transform.position = new Vector3(gridSquareManager.transform.position.x,
                        gridSquareManager.transform.position.y + 0.2f,
                        gridSquareManager.transform.position.z); ;
                    obj.transform.SetParent(gridSquareManager.transform);
                    gridSquareManager.AvaibleType = AvaibleType.Lock;
                }

            }
        }
    }

    private void CheckNeighbor(GridSquareBackground gridSquareManager, int OffsetX, int OffsetZ)
    {
        
    }

    public GameObject GetObject(PoolType poolType)
    {
        return PoolSignals.Instance.onGetObjectFromPool?.Invoke(poolType);
    }
}
