using CoreGameModule.Signals;
using PoolModule.Enums;
using PoolModule.Signals;
using PoolModule.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CutModule.Commands
{
    public class CalculateFinishCommand : IGetPoolObject
    {
        private GameObject _finishObject;

        public CalculateFinishCommand(GameObject finishObject)
        {
            _finishObject = finishObject;
        }
        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool(poolType);
        }

        public void GetFinishObject(List<GameObject> _stackCubes)
        {
            _finishObject.transform.position = new Vector3(0, _finishObject.transform.position.y, _stackCubes[_stackCubes.Count - 1].transform.position.z + (_finishObject.transform.localScale.z + (_finishObject.transform.localScale.z / 2)));
            _finishObject.SetActive(true);
            var gemObject = GetObject(PoolType.GemObject);
            gemObject.transform.position = new Vector3(_finishObject.transform.position.x,
                _finishObject.transform.position.y + 0.5f,
                _finishObject.transform.position.z);
            CoreGameSignals.Instance.onSetStackCubeTransform?.Invoke(_finishObject.transform);
        }
    } 
}
