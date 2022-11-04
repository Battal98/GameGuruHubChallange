using Sirenix.OdinInspector;
using PoolModule.Interfaces;
using PoolModule.Signals;
using System.Collections.Generic;
using UnityEngine;
using PoolModule.Enums;
using GridGame.UIModule;
using GridGame.GridModule.Signals;
using GridGame.InputModule.Signals;

namespace GridGame.GridModule
{
    public class GridManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
    {
        [SerializeField]
        private Transform gridPivotTarget;

        [ReadOnly]
        [ShowInInspector]
        private Vector2[] grids;

        private Vector3 _gridPositions;
        private float gridPivotCalculate;
        [SerializeField]
        private GridData GridData;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            OnCreateGrid(GridData.GridSize);
        }

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

        }

        private void OnCreateGrid(int gridInputSize)
        {
            if (this.transform.childCount > 0)
                DeleteGrid();
            grids = new Vector2[gridInputSize * gridInputSize];
            GridData.GridSize = gridInputSize;
            var gridCount = GridData.GridSize * GridData.GridSize;
            if (GridData.GridSize % 2 == 0)
                gridPivotCalculate = GridData.GridSize / 2 - 0.5f;
            else
                gridPivotCalculate = GridData.GridSize / 2;
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
                grids[i] = new Vector2(_gridPositions.x, _gridPositions.z);
            }
        }
        private void DeleteGrid()
        {
            var count = this.transform.childCount;
            for (var i = count - 1; i >= 0; i--)
                ReleaseObject(this.transform.GetChild(i).gameObject, PoolType.GridObject);
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

 

