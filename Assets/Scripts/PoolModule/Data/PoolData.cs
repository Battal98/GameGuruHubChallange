using System;
using UnityEngine;

namespace PoolModule.Data
{
    [Serializable]
    public struct PoolData
    {
        public GameObject ObjectType;
        public int initalAmount;
        public bool isDynamic;
    } 
}
