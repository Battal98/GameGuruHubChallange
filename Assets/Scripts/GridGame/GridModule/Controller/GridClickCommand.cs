using PoolModule.Signals;
using PoolModule.Interfaces;
using UnityEngine;
using PoolModule.Enums;
using System.Collections.Generic;
using GridGame.UIModule.Signals;
using GridGame.CoreGameModule.Signals;

public class GridClickCommand: IGetPoolObject, IReleasePoolObject
{
    private readonly int _scoreRaiseAmount = 1;
    private List<GridSquareBackground> _neighbors = new List<GridSquareBackground>();
    private ParticleSystem _matchParticle;
    private ParticleSystem _clickParticle;
    public GridClickCommand(ParticleSystem matchParticle, ParticleSystem clickParticle)
    {
        _matchParticle = matchParticle;
        _clickParticle = clickParticle;
    }
    public void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out GridSquareBackground gridSquareManager))
            {
                if (gridSquareManager.AvaibleType != AvaibleType.Lock)
                {
                    var obj = GetObject(PoolType.GridCrownObject);
                    obj.transform.position = new Vector3(gridSquareManager.transform.position.x,
                        gridSquareManager.transform.position.y + 0.2f,
                        gridSquareManager.transform.position.z); ;
                    obj.transform.SetParent(gridSquareManager.transform);
                    gridSquareManager.AvaibleType = AvaibleType.Lock;
                    _neighbors.Clear();
                    CheckNeighbors(gridSquareManager);

                    if (_neighbors.Count >= 3)
                    {
                        CoreGameSignals.Instance.onUpdateGridGameScore?.Invoke(_scoreRaiseAmount);
                        _matchParticle.transform.position = obj.transform.position;
                        _matchParticle.Play();
                        for (int i = _neighbors.Count - 1; i >= 0; i--)
                        {
                            ReleaseObject(_neighbors[i].transform.GetChild(0).gameObject,PoolType.GridCrownObject);
                            _neighbors[i].AvaibleType = AvaibleType.Unlock;

                        }
                        _neighbors.Clear();
                    }
                    else
                    {
                        _clickParticle.transform.position = obj.transform.position;
                        _clickParticle.transform.localScale = Vector3.one /2;
                        _clickParticle.Play();
                    }
                }

            }
        }
    }

    private void CheckNeighbors(GridSquareBackground gridSquareManager)
    {
        if (_neighbors.Contains(gridSquareManager)) return;
        _neighbors.Add(gridSquareManager);
        for (int i = 0; i < gridSquareManager.MyNeighbors.Count; i++)
        {
            var currentGrid = gridSquareManager.GetNeighborSpesificIndex(i);
            if (gridSquareManager.MyNeighbors[i].AvaibleType == AvaibleType.Lock)
                CheckNeighbors(currentGrid);
        }
    }

    public GameObject GetObject(PoolType poolType)
    {
        return PoolSignals.Instance.onGetObjectFromPool?.Invoke(poolType);
    }

    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool?.Invoke(obj, poolType);
    }
}
