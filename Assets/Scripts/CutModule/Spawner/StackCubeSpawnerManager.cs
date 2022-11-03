using UnityEngine;
using PoolModule.Interfaces;
using PoolModule.Signals;
using PoolModule.Enums;
using LevelModule.Signals;
using System.Collections.Generic;
using CutModule.Data;
using CutModule.Data.ScriptableObjects;
using CutModule;
using CoreGameModule.Signals;

public class StackCubeSpawnerManager : MonoBehaviour, IGetPoolObject, IReleasePoolObject
{
    #region Private Variables

    [SerializeField]
    private List<GameObject> _stackCubes;

    [SerializeField]
    private GameObject finishObject;

    private float _stackCubeOffsetZ = 0;
    private int _count;
    private int _colorCount;
    private int _maxCubeCount;
    private bool _isFailed = false;

    #endregion

    private StackCubeData _stackCubeData;

    private void Awake()
    {
        Init();
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
    #region Event Subscriptions
    private void OnEnable()
    {
        InputSignals.Instance.onClick += OnClick;
        LevelSignals.Instance.onRestartLevel += OnRestart;
        LevelSignals.Instance.onLevelFailed += OnLevelFailed;
        CoreGameSignals.Instance.onReset += OnReset;
    }
    private void OnDisable()
    {
        InputSignals.Instance.onClick -= OnClick;
        LevelSignals.Instance.onRestartLevel -= OnRestart;
        LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        CoreGameSignals.Instance.onReset -= OnReset;
    }
    #endregion

    private void OnClick()
    {
        if (_isFailed)
            return;
        if (_count >= _maxCubeCount)
        {
            if (!finishObject.activeInHierarchy)
                GetFinishObject();
            return;
        }

        var movementStackCube = GetObject(PoolType.MovementStackCube);
        movementStackCube.transform.localScale = _stackCubes[0].transform.localScale;
        var objMat = movementStackCube.GetComponentInChildren<MeshRenderer>().material;
        float objScaleZ = movementStackCube.transform.localScale.z;
        float lastObjPosZ = _stackCubes[_stackCubes.Count - 1].transform.position.z;
        _stackCubeOffsetZ = lastObjPosZ + objScaleZ;

        if (_colorCount >= _stackCubeData.CubeColors.Count)
            _colorCount = 0;

        objMat.color = _stackCubeData.CubeColors[_colorCount];

        if (_count % 2 == 0)
            movementStackCube.transform.position = new Vector3(_stackCubeData.SpawnDotsX.x, movementStackCube.transform.position.y, _stackCubeOffsetZ);
        else
            movementStackCube.transform.position = new Vector3(_stackCubeData.SpawnDotsX.y, movementStackCube.transform.position.y, _stackCubeOffsetZ);

        _stackCubes.Add(movementStackCube);
        _count++;
        _colorCount++;
    }
    private void GetFinishObject()
    {
        finishObject.transform.position = new Vector3(0, finishObject.transform.position.y, _stackCubes[_stackCubes.Count - 1].transform.position.z + (finishObject.transform.localScale.z + (finishObject.transform.localScale.z / 2)));
        finishObject.SetActive(true);
    }
    private void OnLevelFailed() => _isFailed = true;
    private void OnRestart()
    {
        OnClick();
        ResetSpawner();
        _count = 0;

    }
    private void OnReset()
    {
        _stackCubes[0].transform.position = new Vector3(0, _stackCubes[0].transform.position.y, finishObject.transform.position.z);
        _stackCubes[0].GetComponentInChildren<MeshRenderer>().material.color = _stackCubes[_stackCubes.Count-1].GetComponentInChildren<MeshRenderer>().material.color;
        _count = 0;
        ResetSpawner();
        OnClick();
        Init();
    }

    private void Init()
    {
        _stackCubeData = GetData().StackCubeData;
        var lastMaxCubeCount = _maxCubeCount;
        _maxCubeCount = GetData().StackCountsEachLevel[LevelSignals.Instance.onGetLevel.Invoke()];
        finishObject.transform.position = new Vector3(0, finishObject.transform.position.y, 0);
        finishObject.SetActive(false);
        //finishObject.transform.position = new Vector3(0, finishObject.transform.position.y,
        //   (lastMaxCubeCount * _stackCubes[0].transform.localScale.z +(finishObject.transform.localScale.z + (finishObject.transform.localScale.z / 2))) 
        //   + ((_maxCubeCount * _stackCubes[0].transform.localScale.z) + (finishObject.transform.localScale.z + (finishObject.transform.localScale.z / 2))));
    }
    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
    }

    private void ResetSpawner()
    {
        for (int i = 1; i < _stackCubes.Count; i++)
        {
            ReleaseObject(_stackCubes[i], PoolType.MovementStackCube);
            _stackCubes[i].GetComponent<StackCubeManager>().enabled = true;
        }
        var obj = _stackCubes[0];
        _stackCubes.Clear();
        _stackCubes.TrimExcess();
        _stackCubes.Add(obj);
        _isFailed = false;
        _stackCubeOffsetZ = 0;
    }
}
