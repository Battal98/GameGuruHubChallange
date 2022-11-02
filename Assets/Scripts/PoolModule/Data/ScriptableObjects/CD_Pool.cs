using UnityEngine;
using UnityEngine.Rendering;
using PoolModule.Enums;

namespace PoolModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "GameName/CD_Pool",
           order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public SerializedDictionary<PoolType, PoolData> PoolDataDic = new SerializedDictionary<PoolType, PoolData>();
    } 
}
