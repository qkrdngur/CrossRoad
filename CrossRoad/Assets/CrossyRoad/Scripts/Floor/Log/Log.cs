using CrossyRoad.Util.Pool;
using DG.Tweening;
using UnityEngine;

namespace CrossyRoad.Floor
{
    public class Log : MonoBehaviour
    {

        [SerializeField] private PoolObjectType type;

        public void Reset()
        {
            transform.DOKill();
            
            ObjectPool.Instance.ReturnObject(type, gameObject);
        }

    }
}