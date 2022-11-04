using RunnerCollectableModule.Interfaces;
using RunnerLevelModule.Signals;
using RunnerCollectableModule.Enums;
using RunnerCoreGameModule.Signals;
using UnityEngine;
using PoolModule.Interfaces;
using PoolModule.Signals;
using PoolModule.Enums;
using DG.Tweening;

namespace RunnerPlayerModule.Controllers
{
    public class PlayerPhysicsController : MonoBehaviour, IGetPoolObject, IReleasePoolObject
    {
        [SerializeField]
        private PlayerManager playerManager;

        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool(poolType);
        }

        public void ReleaseObject(GameObject obj, PoolType poolType)
        {
            PoolSignals.Instance.onReleaseObjectFromPool(obj, poolType);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FallBox"))
            {
                LevelSignals.Instance.onLevelFailed?.Invoke();
                playerManager.StopMovement();
            }
            if (other.CompareTag("Finish"))
            {
                LevelSignals.Instance.onLevelSuccessful?.Invoke();
                playerManager.StopMovement();
            }
            if (other.TryGetComponent(out ICollectable collectable))
            {
                switch (collectable.CollectableType())
                {
                    case CollectableType.Coin:
                        CoreGameSignals.Instance.onUpdateCoinScore?.Invoke(1);
                        var coinParticle = GetObject(PoolType.CoinParticle);
                        coinParticle.transform.position = other.transform.position;
                        DOVirtual.DelayedCall(0.5f, () => ReleaseObject(coinParticle, PoolType.CoinParticle));
                        break;
                    case CollectableType.Gem:
                        CoreGameSignals.Instance.onUpdateGemScore?.Invoke(1);
                        var gemParticle = GetObject(PoolType.GemParticle);
                        gemParticle.transform.position = other.transform.position;
                        DOVirtual.DelayedCall(0.5f, () => ReleaseObject(gemParticle, PoolType.GemParticle));
                        break;
                    case CollectableType.Star:
                        CoreGameSignals.Instance.onUpdateStarScore?.Invoke(1);
                        var starParticle = GetObject(PoolType.StarParticle);
                        starParticle.transform.position = other.transform.position;
                        DOVirtual.DelayedCall(0.5f, () => ReleaseObject(starParticle, PoolType.StarParticle));
                        break;
                }
                other.gameObject.SetActive(false);

            }
        }
    } 
}
