using System.Collections.Generic;
using System.Linq;
using CrossyRoad.Util.Pool;
using UnityEngine;
using UnityEngine.Pool;

namespace CrossyRoad.Floor
{
    public abstract class Floor : MonoBehaviour
    {
        [SerializeField] private PoolObjectType poolObjectType;

        protected readonly List<GameObject> coins = new();

        public abstract void Generate();

        public virtual void GenerateCoin(int coinCount)
        {
            for (var i = 0; i < coinCount; i++)
            {
                var coin = ObjectPool.Instance.GetObject(PoolObjectType.Coin);
                
                coins.Add(coin);
                
                coins[^1].transform.SetParent(transform);
            }
        }

        protected bool IsCoinOverLap(Vector3 coinPosition)
        {
            return coins.Any(coin => Mathf.Approximately(coin
                .transform.position.x, coinPosition.x));
        }
        
        public virtual void Reset()
        {
            foreach (var coin in coins)
            {
                ObjectPool.Instance.ReturnObject(PoolObjectType.Coin, coin);
            }
            
            coins.Clear();
            
            ObjectPool.Instance.ReturnObject(poolObjectType, gameObject);
        }
    }
}