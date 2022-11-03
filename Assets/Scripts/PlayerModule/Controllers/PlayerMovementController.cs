using DG.Tweening;
using UnityEngine;

namespace PlayerModule.Commands
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private bool _readyToMove = false;
        private float otherSpeed = 0; 
        private float direction;
        private const float Xspeed = 35f;

        #endregion

        #endregion
        public void StopPlayerMovement()
        {
            _readyToMove = false;
        }

        public void StartPlayerMovement()
        {
            _readyToMove = true;
        }

        public void PlayerMovement(Rigidbody rigidbody, float speed, Transform target)
        {
            if (_readyToMove)
            {
                var posX = target.position.x - rigidbody.position.x;
                direction = posX < 0 ? -1f : 1f;
                if (Mathf.Abs(posX) >= 0.05f)
                    otherSpeed = Mathf.Lerp(otherSpeed, Xspeed, 0.7f);
                else
                    otherSpeed = 0;

                var velocity = rigidbody.velocity;
                velocity = new Vector3(Time.fixedDeltaTime * otherSpeed * direction, Mathf.Clamp(velocity.y,
                        -9.81f,
                        0.25f), Time.fixedDeltaTime * speed);
                rigidbody.velocity = velocity;

               RotatePlayer(target, rigidbody);

            }
            else if (rigidbody.velocity != Vector3.zero)
            {
                rigidbody.velocity = new Vector3(0,
                    Mathf.Clamp(rigidbody.velocity.y,
                    -9.81f,
                    0.25f),
                    0);
            }
        }
        private void RotatePlayer(Transform target, Rigidbody rb)
        {
            if (target)
            {
                Vector3 movementDirection = new Vector3(target.position.x, 0, target.position.z);
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up * 3f);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, 30);
            }
        }
    } 
}
