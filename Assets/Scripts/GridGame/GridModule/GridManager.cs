using Sirenix.OdinInspector;
using PoolModule.Interfaces;
using PoolModule.Signals;
using UnityEngine;
using PoolModule.Enums;
using GridGame.GridModule.Signals;
using GridGame.InputModule.Signals;
using System.Collections.Generic;

namespace GridGame.GridModule
{
    public class GridManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
    {
        [SerializeField]
        private GridData GridData;
        [SerializeField]
        private Transform gridPivotTarget;

        [ReadOnly]
        [ShowInInspector]
        private GridSquareBackground[,] _gridArray;

        private readonly Vector2[] neighborsPattern =
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1)
        };

        private Vector3 _gridPositions;
        private float gridPivotCalculate;
        private Camera _camera;
        private GridClickCommand _gridClickCommand;

        private void Awake()
        {
            _gridClickCommand = new GridClickCommand(GetGridMatchParticle(), GetGridClickParticle());
            _camera = Camera.main;
        }

        private ParticleSystem GetGridMatchParticle() => GetObject(PoolType.GridMatchParticle).GetComponent<ParticleSystem>();
        private ParticleSystem GetGridClickParticle() => GetObject(PoolType.GridClickParticle).GetComponent<ParticleSystem>();

        #region Event Subscriptions

        private void OnEnable()
        {
            SubsciribeEvents();
        }
        private void SubsciribeEvents()
        {
            GridSignals.Instance.onCreateGrid += OnCreateGrid;
            InputSignals.Instance.onClick += OnClick;
        }
        private void UnsubsciribeEvents()
        {
            GridSignals.Instance.onCreateGrid -= OnCreateGrid;
            InputSignals.Instance.onClick -= OnClick;
        }
        private void OnDisable()
        {
            UnsubsciribeEvents();
        }
        #endregion

        private void OnClick()
        {
            _gridClickCommand.Click();
        }

        private void OnCreateGrid(int gridInputSize)
        {
            if (gridInputSize <= 0)
                return;
            if (this.transform.childCount > 0)
                ReleaseAllGridObject(this.transform,PoolType.GridObject);

            _gridArray = new GridSquareBackground[gridInputSize, gridInputSize];
            GridData.GridSize = gridInputSize;

            var gridCount = GridData.GridSize * GridData.GridSize;

            gridPivotCalculate = CheckPivotPosition(gridCount);

            var cameraCross = GridData.GridOffsets.x > GridData.GridOffsets.y ? GridData.GridOffsets.x : GridData.GridOffsets.y;
            _camera.orthographicSize = GridData.GridSize * cameraCross;

            gridPivotTarget.transform.localPosition = new Vector3(-gridPivotCalculate * GridData.GridOffsets.x, gridPivotTarget.transform.localPosition.y, -gridPivotCalculate * GridData.GridOffsets.y);
            for (int i = 0; i < gridCount; i++)
            {
                var modX = (int)(i % GridData.GridSize);
                var divideZ = (int)(i / GridData.GridSize);
                var modZ = (int)(divideZ % GridData.GridSize);

                var position = gridPivotTarget.position;
                _gridPositions = new Vector3(modX * GridData.GridOffsets.x + position.x, position.y,
                    modZ * GridData.GridOffsets.y + position.z);

                var obj = GetObject(PoolType.GridObject);
                obj.transform.SetParent(this.transform);
                obj.transform.position = _gridPositions;

                var objComponent = obj.GetComponent<GridSquareBackground>();
                _gridArray[modX, modZ] = objComponent;

            }
            FoundAllNeighbors(gridCount);
        }
        private void FoundAllNeighbors(int gridCount)
        {
            for (int i = 0; i < gridCount; i++)
            {
                var modX =(int) i % GridData.GridSize;
                var divideZ = (int)(i / GridData.GridSize);
                var currentGrid = _gridArray[modX, divideZ];

                for (int j = 0; j < neighborsPattern.Length; j++)
                {
                    var neighborPattern = neighborsPattern[j];
                    var neighborXIndex = modX + neighborPattern.x;
                    var neighborZIndex = divideZ + neighborPattern.y;
                    if (CheckEdges(neighborXIndex, neighborZIndex)) continue;
                    currentGrid.GetNeighbors(_gridArray[(int)neighborXIndex, (int) neighborZIndex]);
                }
            }
        }
        private bool CheckEdges(float neighborX, float neighborZ)
        {
            if (neighborX >= GridData.GridSize || neighborX < 0 || neighborZ >= GridData.GridSize || neighborZ < 0)
                return true;
            return false;
        }
        private float CheckPivotPosition(int gridSize)
        {
            if (GridData.GridSize % 2 == 0)
                return GridData.GridSize / 2 - 0.5f;
            return GridData.GridSize / 2;
        }

        private void ReleaseAllGridObject(Transform transformParent, PoolType poolType)
        {
            var count = transformParent.childCount;
            for (var i = count - 1; i >= 0; i--)
                ReleaseObject(transformParent.GetChild(i).gameObject, poolType);
        }

        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool(poolType);
        }

        public void ReleaseObject(GameObject obj, PoolType poolType)
        {
            PoolSignals.Instance.onReleaseObjectFromPool?.Invoke(obj, poolType);
        }

    } 
}

 

