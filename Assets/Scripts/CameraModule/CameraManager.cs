using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Sirenix.OdinInspector;
using CoreGameModule.Signals;
using CameraModule.Enums;
using LevelModule.Signals;

namespace CameraModule
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables

        [SerializeField]
        private CameraStatesType cameraStates;

        [SerializeField] 
        private CinemachineStateDrivenCamera stateDrivenCamera;
        [SerializeField] 
        private CinemachineVirtualCamera winCamera;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform target;
        #endregion

        #region Private Variables
        [Space]
        [ShowInInspector] private Vector3 _initialPosition;

        #endregion

        #endregion

        private void Awake()
        {
            GetInitialPosition();
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onSetCameraTarget += OnSetCameraTarget;

            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccesful;
            LevelSignals.Instance.onLevelFailed += OnLevelFailed;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onSetCameraTarget -= OnSetCameraTarget;

            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccesful;
            LevelSignals.Instance.onLevelFailed -= OnLevelFailed;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void GetInitialPosition()
        {
            _initialPosition = transform.GetChild(0).localPosition;
        }

        private void OnSetCameraTarget(Transform _target)
        {
            target = _target;
            stateDrivenCamera.Follow = target;
            stateDrivenCamera.m_LookAt = null;
            DOTween.KillAll();
        }

        private void SetCameraState(CameraStatesType _cameraState)
        {
            cameraStates = _cameraState;
            winCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = -360;
            DOTween.KillAll();
            animator.Play(cameraStates.ToString());
        }

        public void OnLevelSuccesful()
        {
            SetCameraState(CameraStatesType.WinCamera);
            winCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "";
            winCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "";
            winCamera.m_LookAt = target.transform;
            stateDrivenCamera.m_Follow = target.transform;
            DOTween.To(() =>
            winCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value,
            x => winCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = x, 360, 10f)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        }

        public void OnLevelFailed()
        {
            SetCameraState(CameraStatesType.FailedCamera);
            stateDrivenCamera.m_LookAt = null;
            DOVirtual.DelayedCall(1f,()=> stateDrivenCamera.Follow = null);
        }

        private void OnPlay()
        {
            GetInitialPosition();
            SetCameraState(CameraStatesType.GameCamera);
            stateDrivenCamera.Follow = target.transform;
            stateDrivenCamera.m_LookAt = null;
        }

        private void OnReset()
        {
            SetCameraState(CameraStatesType.GameCamera);
            OnSetCameraTarget(target);

        }

#if UNITY_EDITOR

        [Button]
        public void ChangeCameraState(CameraStatesType cameraStatesType)
        {
            SetCameraState(cameraStatesType);
        }

#endif
    } 
}
