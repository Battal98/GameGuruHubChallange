using UnityEngine;
using PoolModule.Enums;

namespace PoolModule.Interfaces
{
    public interface IReleasePoolObject
    {
        void ReleaseObject(GameObject obj, PoolType poolType);

    } 
}
