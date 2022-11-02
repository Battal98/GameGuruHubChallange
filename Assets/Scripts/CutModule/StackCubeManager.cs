using CutModule.Data;
using UnityEngine;
using CutModule.Data.ScriptableObjects;

namespace CutModule
{
    public class StackCubeManager : MonoBehaviour
    {
        [SerializeField]
        private StackCubeMovementController _stackCubeMovementController;

        private StackCubeData _stackCubeData;
        private bool _isMoveCube = true;

        private void Awake()
        {
            _stackCubeData = GetData();
        }
        private StackCubeData GetData()
        {
            return Resources.Load<CD_StackCube>("Datas/CD_StackCube").StackCubeData;
        }

        #region Event Subscriptions
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onClick += OnClick;
        }
        private void UnsubscribeEvents()
        {
            InputSignals.Instance.onClick -= OnClick;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Update()
        {
            if (_isMoveCube)
                _stackCubeMovementController.XAxisMovement(this.transform, _stackCubeData.MinMaxPushValueX, _stackCubeData.StackCubeSpeed);
        }
        private void OnClick()
        {
            _isMoveCube = false;
            this.enabled = false;
        }
    } 
}
