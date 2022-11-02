using UnityEngine;

namespace PlayerModule.Commands
{
    public class PlayerMovementCommand
    {
        #region Self Variables

        #region Private Variables

        private float _speed;
        private Rigidbody _rigidbody;
        private bool _readyToMove = false;

        #endregion

        #endregion

        public PlayerMovementCommand(Rigidbody rigidbody, float speed)
        {
            _speed = speed;
            _rigidbody = rigidbody;
        }
        public void StopPlayerMovement()
        {
            _readyToMove = false;
        }

        public void StartPlayerMovement()
        {
            _readyToMove = true;
        }

        public void PlayerMovement()
        {
            if (_readyToMove)
            {
                var velocity = _rigidbody.velocity;
                velocity = new Vector3(0, Mathf.Clamp(velocity.y,
                        -9.81f,
                        0.25f), Time.fixedDeltaTime * _speed);
                _rigidbody.velocity = velocity;
            }

        }
    } 
}
