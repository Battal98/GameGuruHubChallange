using CoreGameModule.Signals;
using LevelModule.Signals;
using PlayerModule.Commands;
using PlayerModule.Data;
using PlayerModule.Enums;
using PlayerModule.Data.ScriptableObjects;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerManager : MonoBehaviour
{
    #region Self Variables

    #region Serializable Variables
    [SerializeField]
    private Rigidbody playerRigidbody;
    [SerializeField]
    private Animator playerAnimator;
    #endregion

    #region Private Variables
    [ShowInInspector]
    private PlayerAnimationType _playerAnimationType;
    private PlayerData _playerData;
    private Vector3 _initialPosition;
    private Vector3 _spawnPosition;
    private PlayerMovementCommand _playerMovementCommand;
    private ChangePlayerAnimationCommand _playerAnimationCommand;

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
        CoreGameSignals.Instance.onSetPlayerSpawnPosition += OnSetPlayerSpawnPosition;
        LevelSignals.Instance.onRestartLevel += OnRestart;
        LevelSignals.Instance.onLevelSuccessful += OnLevelSuccesful;
    }
    private void UnsbscribeEvents()
    {
        CoreGameSignals.Instance.onPlay -= OnPlay;
        CoreGameSignals.Instance.onSetPlayerSpawnPosition -= OnSetPlayerSpawnPosition;
        LevelSignals.Instance.onRestartLevel -= OnRestart;
        LevelSignals.Instance.onLevelSuccessful -= OnLevelSuccesful;
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
        _playerMovementCommand = new PlayerMovementCommand(playerRigidbody, _playerData.ForwardSpeed);
        _playerAnimationCommand = new ChangePlayerAnimationCommand(playerAnimator, _playerAnimationType);

        this.transform.position = _spawnPosition;

        CheckPlayerPosition(this.transform.position);
        CoreGameSignals.Instance.onSetCameraTarget?.Invoke(this.transform);
    }
    private void CheckPlayerPosition(Vector3 position)
    {
        _initialPosition = position;
    }
    [Button("Start Movement")]
    private void OnPlay()
    {
        _playerMovementCommand.StartPlayerMovement();
        _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Run);
    }
    private void OnRestart()
    {
        this.transform.position = _initialPosition;
        this.transform.eulerAngles = Vector3.zero;
    }
    private void OnLevelSuccesful()
    {
        _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Dance);
    }
    [Button("Stop Movement")]
    public void StopMovement()
    {
        _playerMovementCommand.StopPlayerMovement();
        if (playerRigidbody.velocity.y < 0f)
        {
            _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Fall);
            return;
        }
        _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Idle);
    }
    private void OnSetPlayerSpawnPosition(Vector3 spawnPositionTarget)
    {
        _spawnPosition = spawnPositionTarget;
    }

    private void FixedUpdate()
    {
        _playerMovementCommand.PlayerMovement();
    }
}
