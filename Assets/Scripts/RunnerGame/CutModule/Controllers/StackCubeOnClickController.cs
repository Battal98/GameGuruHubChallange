using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolModule.Signals;
using PoolModule.Interfaces;
using PoolModule.Enums;
using RunnerCutModule.Data;
using RunnerCoreGameModule.Signals;

namespace RunnerCutModule.Controllers
{
    public class StackCubeOnClickController : MonoBehaviour, IGetPoolObject
    {
        [SerializeField]
        private StackCubeSpawnerManager stackCubeSpawnerManager;

        private int _colorCount;

        public void Click(List<GameObject> _stackCubes, float _stackCubeOffsetZ,float _maxCubeCount ,StackCubeData _stackCubeData)
        {
            //Create Movement Cube and Calculations
            var movementStackCube = GetObject(PoolType.MovementStackCube);
            movementStackCube.transform.localScale = _stackCubes[_stackCubes.Count - 1].transform.localScale;
            var objMat = movementStackCube.GetComponentInChildren<MeshRenderer>().material;
            float objScaleZ = movementStackCube.transform.localScale.z;
            float lastObjPosZ = _stackCubes[_stackCubes.Count - 1].transform.position.z;
            _stackCubeOffsetZ = lastObjPosZ + objScaleZ;

            if (_colorCount >= _stackCubeData.CubeColors.Count)
                _colorCount = 0;

            objMat.color = _stackCubeData.CubeColors[_colorCount];

            if (stackCubeSpawnerManager.Count % 2 == 0)
                movementStackCube.transform.position = new Vector3(_stackCubeData.SpawnDotsX.x, movementStackCube.transform.position.y, _stackCubeOffsetZ);

            else
                movementStackCube.transform.position = new Vector3(_stackCubeData.SpawnDotsX.y, movementStackCube.transform.position.y, _stackCubeOffsetZ);


            _stackCubes.Add(movementStackCube);
            CoreGameSignals.Instance.onSetStackCubeTransform?.Invoke(_stackCubes[_stackCubes.Count - 2].transform);

            //Create Collectables
            if (stackCubeSpawnerManager.Count == (_maxCubeCount / 2))
            {
                var starObj = GetObject(PoolType.StarObject);
                starObj.transform.position = new Vector3(_stackCubes[_stackCubes.Count - 2].transform.position.x,
                   _stackCubes[_stackCubes.Count - 2].transform.position.y + 0.5f,
                   _stackCubes[_stackCubes.Count - 2].transform.position.z);
            }
            else
            {
                var coinObj = GetObject(PoolType.CoinObject);
                coinObj.transform.position = new Vector3(_stackCubes[_stackCubes.Count - 2].transform.position.x,
                    _stackCubes[_stackCubes.Count - 2].transform.position.y + 0.5f,
                    _stackCubes[_stackCubes.Count - 2].transform.position.z);
            }
            stackCubeSpawnerManager.Count++;
            _colorCount++;
        }

        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool(poolType);
        }
    } 
}
