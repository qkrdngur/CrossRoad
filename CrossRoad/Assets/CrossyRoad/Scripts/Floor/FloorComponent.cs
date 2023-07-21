using System.Collections.Generic;
using CrossyRoad.Player;
using CrossyRoad.Util;
using CrossyRoad.Util.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrossyRoad.Floor
{
    public class FloorComponent : GameComponent
    {
        private readonly List<Floor> _floors = new();

        private const int Offset = -20;

        public FloorComponent(GameManager game) : base(game)
        {
        }

        protected override void Initialize()
        {
            game.GetGameComponent<PlayerComponent>().OnPlayerMove += position =>
            {
                if (!(position.z > _floors[20].transform.position.z)) return;

                CreateFloor();

                ClearFloor();
            };

            base.Initialize();
        }

        protected override void OnStandby()
        {
            ClearFloors();

            for (var i = 0; i < 40; i++)
            {
                CreateFloor();
            }
        }

        private void CreateFloor()
        {
            var floorType = GetRandomFloorType();
            var floor = ObjectPool.Instance.GetObject(floorType).GetComponent<Floor>();

            floor.transform.position = GetNextFloorPosition();
            floor.Generate();

            switch (floorType)
            {
                case PoolObjectType.GrassType0 or PoolObjectType.GrassType1:
                    floor.GenerateCoin(1);
                    break;
                case PoolObjectType.Track or PoolObjectType.Road:
                    floor.GenerateCoin(Random.Range(1, 3));
                    break;
            }

            _floors.Add(floor);
        }

        private Vector3 GetNextFloorPosition()
        {
            return _floors.Count == 0
                ? new Vector3(0, -.5f, Offset)
                : new Vector3(0, -.5f, _floors[^1].transform.position.z + 1);
        }

        private void ClearFloors()
        {
            foreach (var floor in _floors)
            {
                floor.Reset();
            }

            _floors.Clear();
        }

        private void ClearFloor()
        {
            _floors[0].Reset();

            _floors.RemoveAt(0);
        }

        private PoolObjectType GetRandomFloorType()
        {
            if (_floors.Count == 0)
                return PoolObjectType.GrassType0;

            if (_floors[^1].transform.position.z < 5)
            {
                return _floors[^1].transform.position.z % 2 == 0
                    ? PoolObjectType.GrassType0
                    : PoolObjectType.GrassType1;
            }

            return Random.Range(0, 4) switch
            {
                0 => _floors[^1].transform.position.z % 2 == 0 ? PoolObjectType.GrassType0 : PoolObjectType.GrassType1,
                1 => PoolObjectType.Road,
                2 => PoolObjectType.Track,
                3 => PoolObjectType.River,
                _ => _floors[^1].transform.position.z % 2 == 0 ? PoolObjectType.GrassType0 : PoolObjectType.GrassType1,
            };
        }
    }
}