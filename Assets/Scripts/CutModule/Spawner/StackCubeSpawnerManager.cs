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
    #region Serializable Variables

    [SerializeField]
    private List<GameObject> _stackCubes;

    [SerializeField]
    private GameObject finishObject;

    #endregion

    #region Private Variables

    private float _stackCubeOffsetZ = 0;
    private int _count;
    private int _colorCount;
    private int _maxCubeCount;
    private bool _isFailed = false;
    private bool isLeft;
    private StackCubeData _stackCubeData;

    #endregion

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
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        //InputSignals.Instance.onClick += CutObject;
        InputSignals.Instance.onClick += OnClick;
        LevelSignals.Instance.onRestartLevel += OnRestart;
        LevelSignals.Instance.onLevelFailed += OnLevelFailed;
        LevelSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
        CoreGameSignals.Instance.onReset += OnReset;
    }
    private void UnsbscribeEvents()
    {
        //InputSignals.Instance.onClick -= CutObject;
        InputSignals.Instance.onClick -= OnClick;
        LevelSignals.Instance.onRestartLevel -= OnRestart;
        LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        CoreGameSignals.Instance.onReset -= OnReset;
    }

    private void OnDisable()
    {
        UnsbscribeEvents();
    }
    #endregion

    private void OnClick()
    {
        if (_isFailed)
            return;
        if (_stackCubes.Count > 1)
        {
            _stackCubes[_stackCubes.Count - 1].transform.localScale = _stackCubes[_stackCubes.Count - 2].transform.localScale;
            CutObject();
        }
            
        if (_count >= _maxCubeCount)
        {
            if (finishObject.activeInHierarchy)
                GetFinishObject();
            return;
        }

        var movementStackCube = GetObject(PoolType.MovementStackCube);
        movementStackCube.transform.localScale = _stackCubes[_stackCubes.Count-1].transform.localScale;
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
    private void OnLevelSuccessful()
    {
        _stackCubes[0].transform.position = new Vector3(_stackCubes[0].transform.position.x,
            _stackCubes[0].transform.position.y,
            finishObject.transform.position.z + ((finishObject.transform.localScale.z + _stackCubes[0].transform.localScale.z) / 2));

        _stackCubes[0].GetComponentInChildren<MeshRenderer>().material.color = _stackCubes[_stackCubes.Count - 1].GetComponentInChildren<MeshRenderer>().material.color;
    }
    private void GetFinishObject()
    {
        finishObject.transform.position = new Vector3(0, finishObject.transform.position.y, _stackCubes[_stackCubes.Count - 1].transform.position.z + (finishObject.transform.localScale.z + (finishObject.transform.localScale.z / 2)));
        finishObject.SetActive(true);
    }
    private void OnLevelFailed() => _isFailed = true;

    #region Reset and Restart Jobs

    private void ResetSpawner()
    {
        for (int i = 1; i < _stackCubes.Count; i++)
        {
            ReleaseObject(_stackCubes[i], PoolType.MovementStackCube);
        }

        var obj = _stackCubes[0];
        _stackCubes.Clear();
        _stackCubes.TrimExcess();
        _stackCubes.Add(obj);

        _isFailed = false;
        _stackCubeOffsetZ = 0;
    }
    private void OnRestart()
    {
        OnClick();
        ResetSpawner();
        _count = 0;

    }
    private void OnReset()
    {
        _count = 0;
        ResetSpawner();
        OnClick();
        Init();
    } 
    #endregion

    private void Init()
    {
        _stackCubeData = GetData().StackCubeData;
        var lastMaxCubeCount = _maxCubeCount;
        _maxCubeCount = GetData().StackCountsEachLevel[LevelSignals.Instance.onGetLevel.Invoke()];
    }
    public void ReleaseObject(GameObject obj, PoolType poolType)
    {
        PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
    }

    private void CutObject()
    {   
        float cutEdge = GetCutEdge();
        float direction = cutEdge > 0 ? 1f : -1f;

        float stackCubeXSize = _stackCubes[_stackCubes.Count - 2].transform.localScale.x - Mathf.Abs(cutEdge);
        float cuttedCubeSize = _stackCubes[_stackCubes.Count - 1].transform.localScale.x - stackCubeXSize;

        float stackCubeXPosition = _stackCubes[_stackCubes.Count - 2].transform.position.x + (cutEdge / 2);

        _stackCubes[_stackCubes.Count - 1].transform.localScale = new Vector3(stackCubeXSize,
            _stackCubes[_stackCubes.Count - 1].transform.localScale.y,
            _stackCubes[_stackCubes.Count - 1].transform.localScale.z);

        _stackCubes[_stackCubes.Count - 1].transform.position = new Vector3(stackCubeXPosition,
            _stackCubes[_stackCubes.Count - 1].transform.position.y,
            _stackCubes[_stackCubes.Count - 1].transform.position.z);

        float cuttedCubeEdge = _stackCubes[_stackCubes.Count - 1].transform.position.x + (stackCubeXSize / 2 * direction);
        float cuttedCubeXPosition = cuttedCubeEdge + cuttedCubeSize / 2f * direction;


        SpawnCuttedCube(cuttedCubeXPosition, cuttedCubeSize);

    }

    private float GetCutEdge()
    {
        return _stackCubes[_stackCubes.Count - 1].transform.position.x - _stackCubes[_stackCubes.Count - 2].transform.position.x;
    }

    private void SpawnCuttedCube(float cuttedCubeXPosition, float cuttedCubeSize)
    {
        var cuttedObj = GetObject(PoolType.CuttedCubes);

        cuttedObj.transform.localScale = new Vector3(cuttedCubeSize,
            _stackCubes[_stackCubes.Count - 1].transform.localScale.y,
            _stackCubes[_stackCubes.Count - 1].transform.localScale.z);
        cuttedObj.transform.position = new Vector3(cuttedCubeXPosition,
            _stackCubes[_stackCubes.Count - 1].transform.position.y,
            _stackCubes[_stackCubes.Count - 1].transform.position.z);

        cuttedObj.GetComponentInChildren<MeshRenderer>().material.color = _stackCubes[_stackCubes.Count - 1].GetComponentInChildren<MeshRenderer>().material.color;

    }

    private void TestObj(float value)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.localScale = Vector3.one * 0.1f;
        obj.transform.position = new Vector3(value, _stackCubes[_stackCubes.Count - 2].transform.position.y, _stackCubes[_stackCubes.Count - 2].transform.position.z);
    }
}
