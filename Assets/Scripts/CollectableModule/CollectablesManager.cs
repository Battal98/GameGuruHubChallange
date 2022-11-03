using CollectableModule.Enums;
using CollectableModule.Interfaces;
using CoreGameModule.Signals;
using LevelModule.Signals;
using UnityEngine;

public class CollectablesManager : MonoBehaviour, ICollectable
{
    [SerializeField]
    private CollectableType collectableType;
    public CollectableType CollectableType()
    {
        return collectableType;
    }

    #region Event Subscription

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        LevelSignals.Instance.onRestartLevel += OnRestart;
        CoreGameSignals.Instance.onReset += OnRestart;
    }
    private void UnsbscribeEvents()
    {
        LevelSignals.Instance.onRestartLevel -= OnRestart;
        CoreGameSignals.Instance.onReset -= OnRestart;
    }

    private void OnDisable()
    {
        UnsbscribeEvents();
    }

    private void OnRestart()
    {
        if(!this.gameObject.activeInHierarchy)
            gameObject.SetActive(true);
    }

    #endregion
}
