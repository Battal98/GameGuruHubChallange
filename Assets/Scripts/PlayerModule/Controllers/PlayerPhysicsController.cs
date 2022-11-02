using CollectableModule.Interfaces;
using LevelModule.Signals;
using CollectableModule.Enums;
using CoreGameModule.Signals;
using UnityEngine;

namespace PlayerModule.Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField]
        private PlayerManager playerManager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FallBox"))
            {
                playerManager.StopMovement();
                LevelSignals.Instance.onLevelFailed?.Invoke();
            }
            if (other.CompareTag("Finish"))
            {
                playerManager.StopMovement();
                LevelSignals.Instance.onLevelSuccessful?.Invoke();
            }
            if (other.TryGetComponent(out ICollectable collectable))
            {
                switch (collectable.CollectableType())
                {
                    case CollectableType.Coin:
                        CoreGameSignals.Instance.onUpdateCoinScore?.Invoke(1);
                        //particle oynat
                        break;
                    case CollectableType.Gem:
                        CoreGameSignals.Instance.onUpdateGemScore?.Invoke(1);
                        break;
                    case CollectableType.Star:
                        CoreGameSignals.Instance.onUpdateStarScore?.Invoke(1);
                        break;
                }
                other.gameObject.SetActive(false);

            }
        }
    } 
}
