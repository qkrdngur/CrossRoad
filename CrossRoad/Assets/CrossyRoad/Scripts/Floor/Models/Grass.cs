using System.Collections.Generic;
using System.Linq;
using CrossyRoad.Util.Pool;
using UnityEngine;
using UnityEngine.Pool;

namespace CrossyRoad.Floor
{
    public class Grass : Floor
    {
        private readonly List<GameObject> _trees = new();

        public override void Generate()
        {
            CreateTree();
        }

        private void CreateTree()
        {
            var treeCount = Random.Range(3, 5);

            for (var i = -13; i <= -11; i++)
            {
                var tree = ObjectPool.Instance.GetObject(GetRandomTreeType());
                
                _trees.Add(tree);
                
                _trees[^1].transform.SetParent(transform, true);
                _trees[^1].transform.position = new Vector3(i, 0, transform.position.z);
            }

            for (var i = 11; i <= 13; i++)
            {
                var tree = ObjectPool.Instance.GetObject(GetRandomTreeType());
                
                _trees.Add(tree);
                
                _trees[^1].transform.SetParent(transform, true);
                _trees[^1].transform.position = new Vector3(i, 0, transform.position.z);
            }
            
            for (var i = 0; i < treeCount; i++)
            {
                var tree = ObjectPool.Instance.GetObject(GetRandomTreeType());

                _trees.Add(tree);

                _trees[^1].transform.SetParent(transform, true);
                _trees[^1].transform.position = GetRandomTreePosition();
            }
        }

        public override void GenerateCoin(int coinCount)
        {
            base.GenerateCoin(coinCount);

            foreach (var coin in coins)
            {
                var coinPosition = new Vector3(Random.Range(-10, 11), 0, transform.position.z);
                
                if(IsTreeOverlap(coinPosition))
                    continue;
                
                if(IsCoinOverLap(coinPosition))
                    continue;
                
                coin.transform.position = coinPosition;
                
                break;
            }
        }

        private Vector3 GetRandomTreePosition()
        {
            while (true)
            {
                var treePosition = new Vector3(Random.Range(-10, 11), 0, transform.position.z);
                
                if (treePosition is { x : 0, z : 0 })
                    continue;

                if(IsTreeOverlap(treePosition))
                    continue;

                return treePosition;
            }
        }

        private bool IsTreeOverlap(Vector3 treePosition)
        {
            return _trees.Any(tree => Mathf.Approximately(tree.transform.position.x,
                treePosition.x));
        }

        private static PoolObjectType GetRandomTreeType()
        {
            return Random.Range(0, 3) switch
            {
                0 => PoolObjectType.TreeType0,
                1 => PoolObjectType.TreeType1,
                2 => PoolObjectType.TreeType2,
                _ => PoolObjectType.TreeType0
            };
        }
        
        public override void Reset()
        {
            foreach (var tree in _trees)
            {
                switch (tree.tag)
                {
                    case "TreeType0":
                        ObjectPool.Instance.ReturnObject(PoolObjectType.TreeType0, tree);
                        break;
                    case "TreeType1":
                        ObjectPool.Instance.ReturnObject(PoolObjectType.TreeType1, tree);
                        break;
                    case "TreeType2":
                        ObjectPool.Instance.ReturnObject(PoolObjectType.TreeType2, tree);
                        break;
                }
            }

            _trees.Clear();

            base.Reset();
        }
    }
}