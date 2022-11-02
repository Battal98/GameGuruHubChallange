using PlayerModule.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerModule.Commands
{
    public class ChangePlayerAnimationCommand
    {
        #region Self Variables

        #region Private Variables

        private Animator _animator;
        private PlayerAnimationType _playerAnimationType;

        #endregion

        #endregion
        public ChangePlayerAnimationCommand(Animator animator, PlayerAnimationType playerAnimationType)
        {
            _animator = animator;
            _playerAnimationType = playerAnimationType;
        }
        public void ChangePlayerAnimation(PlayerAnimationType playerAnimationType)
        {
            _playerAnimationType = playerAnimationType;
            _animator.SetTrigger(_playerAnimationType.ToString());
        }
    } 
}
