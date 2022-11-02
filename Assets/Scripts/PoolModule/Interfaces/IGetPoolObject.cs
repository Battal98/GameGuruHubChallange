using UnityEngine;
using PoolModule.Enums;

namespace PoolModule.Interfaces
{
    public interface IGetPoolObject
    {
        GameObject GetObject(PoolType poolType);
    }
}
