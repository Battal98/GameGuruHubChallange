using UnityEngine;
using UnityEngine.Rendering;
using PoolModule.Signals;
using PoolModule.Data;
using PoolModule.Data.ScriptableObjects;
using PoolModule.Enums;
using PoolModule.Extentions;

namespace PoolModule
{
    public class PoolManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        private SerializedDictionary<PoolType, PoolData> _data;
        private int _listCountCache;
        private ObjectPoolExtention _extention;
        #endregion

        #endregion

        private void Awake()
        {
            _extention = new ObjectPoolExtention();
            _data = GetData();
            InitializePools();
        }

        #region Event Subscription
        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            PoolSignals.Instance.onGetObjectFromPool += OnGetObjectFromPool;
            PoolSignals.Instance.onReleaseObjectFromPool += OnReleaseObjectFromPool;

        }
        private void UnsubscribeEvents()
        {
            PoolSignals.Instance.onGetObjectFromPool -= OnGetObjectFromPool;
            PoolSignals.Instance.onReleaseObjectFromPool -= OnReleaseObjectFromPool;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void ResetObject(GameObject obj)
        {
            obj.transform.parent = this.transform;
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = new Quaternion(0, 0, 0, 0).normalized;
        }

        private GameObject OnGetObjectFromPool(PoolType poolType)
        {
            _listCountCache = (int)poolType;
            return _extention.GetObject<GameObject>(poolType);
        }
        private void OnReleaseObjectFromPool(GameObject obj, PoolType poolType)
        {
            _listCountCache = (int)poolType;
            ResetObject(obj);
            _extention.ReturnObject<GameObject>(obj, poolType);
        }

        private SerializedDictionary<PoolType, PoolData> GetData() => Resources.Load<CD_Pool>("Datas/CD_Pool").PoolDataDic;

        private void InitializePools()
        {
            for (int index = 0; index < _data.Count; index++)
            {
                _listCountCache = index;
                InitPool(((PoolType)index), _data[((PoolType)index)].initalAmount, _data[((PoolType)index)].isDynamic);
            }
        }

        public void InitPool(PoolType poolType, int initalAmount, bool isDynamic)
        {
            _extention.AddObjectPool<GameObject>(FactoryMethod, TurnOnObject, TurnOffObject, poolType, initalAmount, isDynamic);
        }

        public void TurnOnObject(GameObject obj)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        public void TurnOffObject(GameObject obj)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        public GameObject FactoryMethod()
        {
            var go = Instantiate(_data[((PoolType)_listCountCache)].ObjectType, this.transform);
            return go;
        }
    }

}