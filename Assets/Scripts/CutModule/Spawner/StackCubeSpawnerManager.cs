using UnityEngine;
using PoolModule.Interfaces;
using PoolModule.Signals ;
using PoolModule.Enums;
using LevelModule.Signals;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using CutModule.Data;
using CutModule.Data.ScriptableObjects;

public class StackCubeSpawnerManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
{
    #region Private Variables

    [ShowInInspector]
    private List<GameObject> _stackCubes;
    private int _stackCubeOffsetZ = 0;
    private int _count;

    #endregion
    private StackCubeData _stackCubeData;

    private void Awake()
    {
        _stackCubeData = GetData();
    }
    private StackCubeData GetData()
    {
        return Resources.Load<CD_StackCube>("Datas/CD_StackCube").StackCubeData;
    }

    private void Start()
    {
        OnClick();
    }
    public GameObject GetObject(PoolType poolType)
    {
        return PoolSignals.Instance.onGetObjectFromPool(poolType);
    }
    private void OnEnable()
    {
        InputSignals.Instance.onClick += OnClick;
        LevelSignals.Instance.onRestartLevel += OnRestart;
    }

    private void OnDisable()
    {
        InputSignals.Instance.onClick -= OnClick;
        LevelSignals.Instance.onRestartLevel -= OnRestart;
    }

    private void OnClick()
    {
        var obj = GetObject(PoolType.CubeObj);
        float value = obj.transform.localScale.z;
        _stackCubeOffsetZ += (int) value;
        if (_count % 2 == 0)
            obj.transform.position = new Vector3(_stackCubeData.SpawnDotsX.x, obj.transform.position.y, obj.transform.position.z + _stackCubeOffsetZ);
        else
            obj.transform.position = new Vector3(_stackCubeData.SpawnDotsX.y, obj.transform.position.y, obj.transform.position.z + _stackCubeOffsetZ);
        _stackCubes.Add(obj);
        _count++;
    }
    private void OnRestart()
    {
        for (int i = 0; i < _stackCubes.Count; i++)
        {
            ReleaseObject(_stackCubes[i], PoolType.CubeObj);
        }
        OnClick();
        _count = 0;
    }

    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
    }
}
