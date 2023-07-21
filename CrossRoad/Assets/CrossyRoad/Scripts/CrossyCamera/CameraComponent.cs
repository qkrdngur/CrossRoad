using CrossyRoad.Player;
using CrossyRoad.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace CrossyRoad.CrossyCamera
{
    public class CameraComponent : GameComponent
    {
        private Camera _camera;

        private GameObject _target;

        private readonly Vector3 _offset = new (3, 10, -5);
        
        private readonly Vector3 _defaultVelocity = Vector3.forward * .02f;
        
        public CameraComponent(GameManager game) : base(game)
        {
            
        }

        protected override void Initialize()
        {
            _camera = Camera.main;

            _target = GameObject.FindWithTag("Player");

            Observable.EveryUpdate()
                .Where(stream => game.State == GameState.Running)
                .Subscribe(stream => Move()).AddTo(game);

            game.GetGameComponent<PlayerComponent>().OnPlayerInvisible += Focus;
        }

        private void Move()
        {
            var movePosition = _target.transform.position;
            movePosition += _offset;

            if (_camera.transform.position.z > movePosition.z)
                movePosition.z = _camera.transform.position.z;

            if (Mathf.Abs(movePosition.x) > 4)
                movePosition.x = _camera.transform.position.x;

            movePosition += _defaultVelocity;
                    
            _camera.transform.position =
                Vector3.Lerp(_camera.transform.position, movePosition, Time.deltaTime * 10);
        }

        private void Focus(GameObject target)
        {
            var movePosition = target.transform.position;

            _camera.DOOrthoSize(6, 1f);
            _camera.transform.DOMove(movePosition + _offset, 1f);
        }
        
        protected override void OnStandby()
        {
            _camera.transform.position = _offset;
            _camera.orthographicSize = 7.5f;
        }
    }
}