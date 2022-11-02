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
    private PlayerMovementCommand _playerMovementCommand;
    private ChangePlayerAnimationCommand _playerAnimationCommand;

    #endregion

    #endregion

    private void Awake()
    {
        _playerData = GetPlayerData();
        InitCommands();
        CheckPlayerPosition(this.transform.position);
    }

    #region Event Subscriptions

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onPlay += OnPlay;
        LevelSignals.Instance.onRestartLevel += OnRestart;
    }
    private void UnsbscribeEvents()
    {
        CoreGameSignals.Instance.onPlay -= OnPlay;
        LevelSignals.Instance.onRestartLevel -= OnRestart;
    }

    private void OnDisable()
    {
        UnsbscribeEvents();
    }
    #endregion

    private PlayerData GetPlayerData()
    {
        return Resources.Load<CD_Player>("Datas/CD_Player").PlayerData;
    }
    private void InitCommands()
    {
        _playerMovementCommand = new PlayerMovementCommand(playerRigidbody, _playerData.ForwardSpeed);
        _playerAnimationCommand = new ChangePlayerAnimationCommand(playerAnimator, _playerAnimationType);
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
    }

    [Button("Stop Movement")]
    private void StopMovement()
    {
        _playerMovementCommand.StopPlayerMovement();
        _playerAnimationCommand.ChangePlayerAnimation(PlayerAnimationType.Idle);
    }

    private void FixedUpdate()
    {
        _playerMovementCommand.PlayerMovement();
    }
}
