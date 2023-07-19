using CrossRoad.Util;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private ObjectPoolData[] poolData;

    private readonly Dictionary<PoolObjectType, ObjectPoolData> _objectPoolDataMap = new();

    private readonly Dictionary<PoolObjectType, Queue<GameObject>> _pool = new();

    private readonly Queue<GameObject> _queue = new();

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        foreach(var data in poolData)
        {
            _objectPoolDataMap.Add(data.objectType, data);
        }

        foreach(var data in _objectPoolDataMap)
        {
            _pool.Add(data.Key, new Queue<GameObject>());

            var objectPoolData = data.Value;

            for(int i = 0; i < objectPoolData.prefabCount; i++)
            {
                var poolObject = CreateNewObject(data.Key);
                _pool[data.Key].Enqueue(poolObject);
            }
        }
    }

    private GameObject CreateNewObject(PoolObjectType type)
    {
        var newObj = Instantiate(_objectPoolDataMap[type].prefab, transform);
        newObj.SetActive(false);

        return newObj;
    }

    public GameObject GetObject(PoolObjectType type)
    {
        if (_pool[type].Count > 0)
        {
            var obj = _pool[type].Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(type);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(PoolObjectType type, GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        _pool[type].Enqueue(obj);
    }
}
