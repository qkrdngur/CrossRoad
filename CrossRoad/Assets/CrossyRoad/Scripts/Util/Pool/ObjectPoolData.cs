using System;
using UnityEngine;

namespace CrossyRoad.Util.Pool
{
    [Serializable]
    public class ObjectPoolData
    {
        public GameObject prefab;

        public PoolObjectType objectType;

        public int prefabCount;
    }
}