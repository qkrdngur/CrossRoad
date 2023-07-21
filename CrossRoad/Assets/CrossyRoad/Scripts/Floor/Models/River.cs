using System.Collections.Generic;
using CrossyRoad.Util.Pool;
using DG.Tweening;
using UnityEngine;

namespace CrossyRoad.Floor
{
    public class River : Floor
    {
        private readonly List<Log> _logs = new();

        private int _direction;

        private float _speed;

        public override void Generate()
        {
            _direction = Random.Range(0, 101) > 50 ? -1 : 1;
            _speed = Random.Range(70, 150) * .1f;

            LogLoop();
        }

        private void LogLoop()
        {
            var log = ObjectPool.Instance.GetObject(GetRandomLogType()).GetComponent<Log>();

            _logs.Add(log);

            _logs[^1].transform.position = new Vector3(15 * _direction, 0
                , transform.position.z);
            _logs[^1].transform.localEulerAngles = Vector3.zero;

            _logs[^1].transform.DOLocalMoveX(15 * -_direction, _speed)
                .SetDelay(Random.Range(2, 4)).SetEase(Ease.Linear)
                .OnPlay(LogLoop)
                .OnComplete(() =>
                {
                    _logs[0].Reset();
                    _logs.RemoveAt(0);
                });
        }

        private PoolObjectType GetRandomLogType()
        {
            return Random.Range(0, 3) switch
            {
                0 => PoolObjectType.LogType0,
                1 => PoolObjectType.LogType1,
                2 => PoolObjectType.LogType2,
                _ => PoolObjectType.LogType0
            };
        }
        
        public override void Reset()
        {
            foreach (var log in _logs)
            {
                log.transform.DOKill();
                
                log.Reset();
            }
            
            _logs.Clear();
            
            base.Reset();
        }
    }
}