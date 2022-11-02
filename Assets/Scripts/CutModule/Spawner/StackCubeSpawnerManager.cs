using UnityEngine;
using PoolModule.Interfaces;
using PoolModule.Signals ;
using PoolModule.Enums;
using LevelModule.Signals;
using System.Collections.Generic;
using CutModule.Data;
using CutModule.Data.ScriptableObjects;
using CutModule;

public class StackCubeSpawnerManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
{
    #region Private Variables

    [SerializeField]
    private List<GameObject> _stackCubes;

    [SerializeField]
    private GameObject finishObject;

    private int _stackCubeOffsetZ = 0;
    private int _count;
    private int _colorCount;
    private int _maxCubeCount;

    #endregion

    private StackCubeData _stackCubeData;

    private void Awake()
    {
        _stackCubeData = GetData().StackCubeData;
        _maxCubeCount = GetData().StackCountsEachLevel[LevelSignals.Instance.onGetLevel.Invoke()];
        finishObject.transform.position = new Vector3(0, finishObject.transform.position.y, 
            (_maxCubeCount * _stackCubes[0].transform.localScale.z) + (finishObject.transform.localScale.z + (finishObject.transform.localScale.z / 2)));
    }
    private CD_StackCube GetData()
    {
        return Resources.Load<CD_StackCube>("Datas/CD_StackCube");
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
        if (_count >= _maxCubeCount)
        {
            return;
        }

        var obj = GetObject(PoolType.CubeObj);
        var objMat = obj.GetComponentInChildren<MeshRenderer>().material;
        float value = obj.transform.localScale.z;
        _stackCubeOffsetZ += (int)value;

        if (_colorCount >= _stackCubeData.CubeColors.Count)
            _colorCount = 0;

        objMat.color = _stackCubeData.CubeColors[_colorCount];

        if (_count % 2 == 0)
            obj.transform.position = new Vector3(_stackCubeData.SpawnDotsX.x, obj.transform.position.y, obj.transform.position.z + _stackCubeOffsetZ);
        else
            obj.transform.position = new Vector3(_stackCubeData.SpawnDotsX.y, obj.transform.position.y, obj.transform.position.z + _stackCubeOffsetZ);
        
        _stackCubes.Add(obj);
        _count++;
        _colorCount++;
    }
    private void OnRestart()
    {
        for (int i = 0; i < _stackCubes.Count; i++)
        {
            if(_stackCubes[i].GetComponent<StackCubeManager>() is not null)
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
