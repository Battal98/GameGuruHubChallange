using UnityEngine;
using UnityEngine.Events;
using PoolModule.Enums;
using Extentions;
using System;

namespace PoolModule.Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolType,GameObject> onGetObjectFromPool = delegate { return null; };
        public UnityAction<GameObject, PoolType> onReleaseObjectFromPool = delegate { };
    } 
}
