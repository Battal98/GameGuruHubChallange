using RunnerCoreGameModule.Signals;
using RunnerLevelModule.Signals;
using RunnerPlayerModule.Commands;
using RunnerPlayerModule.Data;
using RunnerPlayerModule.Enums;
using RunnerPlayerModule.Data.ScriptableObjects;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace RunnerPlayerModule
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serializable Variables
        [SerializeField]
        private PlayerMovementController _playerMovementController;
        [SerializeField]
        private Rigidbody playerRigidbody;
        [SerializeField]
        private Animator playerAnimator;
        [SerializeField]
        private Transform target;
        #endregion

        #region Private Variables
        private PlayerData _playerData;
        private Vector3 _initialPosition;
        private Vector3 _spawnPosition;
        private ChangePlayerAnimationCommand _playerAnimationCommand;
        private bool _isWin = false;

        #endregion

        #endregion

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnPlay;
            CoreGameSignals.Instance.onSetPlayerSpawnPosition += OnSetPlayerSpawnPosition;
            LevelSignals.Instance.onRestartLevel += OnRestart;
            LevelSignals.Instance.onLevelSuccessful += OnLevelSuccesful;
            CoreGameSignals.Instance.onSetStackCubeTransform += OnSetStackCubeTransform;
        }
        private void UnsbscribeEvents()
        {
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnPlay;
            CoreGameSignals.Instance.onSetPlayerSpawnPosition -= OnSetPlayerSpawnPosition;
            LevelSignals.Instance.onRestartLevel -= OnRestart;
            LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccesful;
            CoreGameSignals.Instance.onSetStackCubeTransform -= OnSetStackCubeTransform;
        }

        private void OnDisable()
        {
            UnsbscribeEvents();
        }
        #endregion

        private void Awake()
        {
            _playerData = GetPlayerData();
            InitJobs();
        }
        private PlayerData GetPlayerData()
        {
            return Resources.Load<CD_Player>("Datas/CD_Player").PlayerData;
        }
        private void InitJobs()
        {
            _playerAnimationCommand = new ChangePlayerAnimationCommand(playerAnimator);

            this.transform.position = _spawnPosition;
            _isWin = false;

            CheckPlayerPosition(this.transform.position);
            CoreGameSignals.Instance.onSetCameraTarget?.Invoke(this.transform);
        }
        public void CheckPlayerPosition(Vector3 position)
        {
            _initialPosition = position;
        }

        [Button("Start Movement")]
        private void OnPlay()
        {
            _isWin = false;
            _playerMovementController.StartPlayerMovement();
            _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Run);
        }
        private void OnRestart()
        {
            this.transform.position = _initialPosition;
            this.transform.eulerAngles = Vector3.zero;
        }
        private void OnLevelSuccesful()
        {
            _isWin = true;
        }
        [Button("Stop Movement")]
        public void StopMovement()
        {
            _playerMovementController.StopPlayerMovement();
            if (playerRigidbody.velocity.y < 0f)
            {
                _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Fall);
                return;
            }
            if (_isWin)
            {
                _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Dance);
                return;
            }
            _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Idle);
        }
        private void OnSetPlayerSpawnPosition(Vector3 spawnPositionTarget)
        {
            _spawnPosition = spawnPositionTarget;
        }

        private void OnSetStackCubeTransform(Transform arg0)
        {
            target = arg0;
        }

        private void FixedUpdate()
        {
            _playerMovementController.PlayerMovement(playerRigidbody, _playerData.ForwardSpeed, target);
        } 
    }
}
