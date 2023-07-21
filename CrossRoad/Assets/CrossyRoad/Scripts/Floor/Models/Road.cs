using System.Collections.Generic;
using CrossyRoad.Util.Pool;
using DG.Tweening;
using UnityEngine;

namespace CrossyRoad.Floor
{
    public class Road : Floor
    {
        private readonly List<Car> _cars = new();

        private int _direction;

        private float _speed;
        
        public override void Generate()
        {
            _direction = Random.Range(0, 101) > 50 ? -1 : 1;
            _speed = Random.Range(40, 81) * .1f;

            CarLoop();
        }

        private void CarLoop()
        {
            var car = ObjectPool.Instance.GetObject(GetRandomCarType()).GetComponent<Car>();
            
            _cars.Add(car);

            _cars[^1].transform.position = new Vector3(15 * _direction, 0,
                transform.position.z);
            _cars[^1].transform.GetChild(0).localEulerAngles = new Vector3(
                0, _direction == 1 ? 0 : 180, 0);

            _cars[^1].transform.DOLocalMoveX(-15 * _direction, _speed)
                .SetDelay(Random.Range(10, 41) * .1f).SetEase(Ease.Linear)
                .OnPlay(CarLoop)
                .OnComplete(() =>
                {
                    _cars[0].Reset();
                    _cars.RemoveAt(0);
                });
        }

        public override void GenerateCoin(int coinCount)
        {
            base.GenerateCoin(coinCount);

            foreach (var coin in coins)
            {
                var coinPosition = new Vector3(Random.Range(-10, 11), 0, transform.position.z);

                if(IsCoinOverLap(coinPosition))
                    continue;
                
                coin.transform.position = coinPosition;
                
                break;
            }
        }
        
        private PoolObjectType GetRandomCarType()
        {
            return Random.Range(0, 4) switch
            {
                0 => PoolObjectType.CarType0,
                1 => PoolObjectType.CarType1,
                2 => PoolObjectType.CarType2,
                3 => PoolObjectType.CarType3,
                _ => PoolObjectType.CarType0
            };
        }
        
        public override void Reset()
        {
            foreach (var car in _cars)
            {
                car.Reset();
            }
            
            _cars.Clear();
            
            base.Reset();
        }
        
    }
}