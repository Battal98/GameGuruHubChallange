using System;
using System.Collections.Generic;
using PoolModule.Enums;

namespace PoolModule.Extentions
{
    public class ObjectPoolExtention
    {
        private readonly Dictionary<PoolType, AbstractObjectPool> _pools;

        public ObjectPoolExtention()
        {
            _pools = new Dictionary<PoolType, AbstractObjectPool>();
        }

        public void AddObjectPool<T>(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, PoolType poolName, int initialStock = 0, bool isDynamic = true)
        {
            if (!_pools.ContainsKey(poolName))
                _pools.Add(poolName, new ObjectPool<T>(factoryMethod, turnOnCallback, turnOffCallback, initialStock, isDynamic));
        }
        
        public ObjectPool<T> GetObjectPool<T>(PoolType poolName)
        {
            return (ObjectPool<T>)_pools[poolName];
        }

        public T GetObject<T>(PoolType poolName)
        {
            return ((ObjectPool<T>)_pools[poolName]).GetObject();
        }
        
        public void ReturnObject<T>(T o, PoolType poolName)
        {
            ((ObjectPool<T>)_pools[poolName]).ReturnObject(o);
        }
        public void RemovePool(PoolType poolName)
        {
            _pools[poolName] = null;
        }
    }
}