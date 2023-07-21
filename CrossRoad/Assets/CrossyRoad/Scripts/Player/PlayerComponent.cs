using System;
using System.Collections.Generic;
using CrossyRoad.Input;
using CrossyRoad.Util;
using CrossyRoad.Util.Pool;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Pool;

namespace CrossyRoad.Player
{
    public class PlayerComponent : GameComponent
    {
        public event Action<Vector3> OnPlayerMove;

        public event Action<GameObject> OnPlayerInvisible;

        private GameObject _player;

        private readonly Queue<PlayerDirection> _playerDirections = new();

        private IDisposable _becameInvisibleDisposable;

        public PlayerComponent(GameManager game) : base(game)
        {
            Observable.EveryUpdate()
                .Where(_ => game.State == GameState.Running)
                .Select(_ => _player.transform.position)
                .DistinctUntilChanged()
                .Subscribe(position => OnPlayerMove?.Invoke(position))
                .AddTo(game);
        }

        protected override void Initialize()
        {
            _player = ObjectPool.Instance.GetObject(PoolObjectType.Player);

            _player.OnTriggerEnterAsObservable()
                .Where(collider => game.State == GameState.Running)
                .Subscribe(HandleTriggerEnter).AddTo(game);

            _becameInvisibleDisposable =
                _player.transform.GetChild(0).OnBecameInvisibleAsObservable()
                    .Where(stream => game.State == GameState.Running)
                    .Subscribe(_ =>
                    {
                        Debug.Log("Invisible");

                        OnPlayerInvisible?.Invoke(_player);

                        game.UpdateState(GameState.Over);
                    }).AddTo(_player).AddTo(game);

            game.GetGameComponent<InputComponent>().OnInputReceived += direction =>
                _playerDirections.Enqueue(direction);

            Observable.EveryUpdate()
                .Where(stream => _playerDirections.Count > 0)
                .Where(stream => _player != null && !DOTween.IsTweening(_player.transform))
                .Select(stream => _playerDirections.Dequeue())
                .Subscribe(PerformPlayerMovement).AddTo(game);
        }

        private void HandleTriggerEnter(Collider collider)
        {
            switch (collider.tag)
            {
                case "Car" or "Train":
                    _playerDirections.Clear();

                    GameManager.Instance.UpdateState(GameState.Over);

                    _player.transform.DOKill();
                    _player.transform.DOMoveY(0, .16f);
                    _player.transform.DOScaleY(.1f, .16f);
                    break;
                case "DeepWater":
                    _playerDirections.Clear();
                    
                    game.UpdateState(GameState.Over);

                    _player.transform.DOKill();
                    break;
                case "Coin":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Coin, collider.gameObject);
                    break;
            }
        }

        private void PerformPlayerMovement(PlayerDirection direction)
        {
            _player.transform.DOScaleY(.8f, .08f)
                .OnComplete(() =>
                {
                    _player.transform.DOScaleY(1, .32f);

                    var jumpPosition = _player.transform.position;

                    if (Physics.Raycast(
                            _player.transform.position + Vector3.up, GetConvertedDirectionToV3(direction),
                            out var hit, 1))
                    {
                        HandleRaycastCollision(hit, direction);

                        return;
                    }

                    if (_player.transform.parent != null)
                    {
                        switch (_player.transform.parent.tag)
                        {
                            case "Log":
                                _player.transform.SetParent(null);

                                var movePos = _player.transform.position;
                                movePos += GetConvertedDirectionToV3(direction);

                                movePos = new Vector3((float)Math.Round(movePos.x, MidpointRounding.AwayFromZero),
                                    0, (float)Math.Round(movePos.z, MidpointRounding.AwayFromZero));

                                _player.transform.DOJump(movePos, 1, 1, .32f)
                                    .OnComplete(HandleDownDirectionRaycastCollision);
                                _player.transform.DOLocalRotate(GetConvertedDirectionToRotateV3(direction), .32f);
                                return;
                        }
                    }

                    jumpPosition += GetConvertedDirectionToV3(direction);

                    _player.transform.DOLocalJump(jumpPosition, 1, 1, .32f)
                        .OnComplete(HandleDownDirectionRaycastCollision);

                    _player.transform.DOLocalRotate(GetConvertedDirectionToRotateV3(direction)
                        , .32f);
                });
        }

        private void HandleRaycastCollision(RaycastHit hit, PlayerDirection direction)
        {
            switch (hit.collider.tag)
            {
                case "Log":
                    _player.transform.DOKill();

                    _player.transform.SetParent(hit.collider.transform);

                    _player.transform.DOLocalJump(new Vector3(0, .3f, 0), 1, 1, .32f);
                    _player.transform.DOLocalRotate(GetConvertedDirectionToRotateV3(direction), .32f);

                    _player.transform.DOScaleY(1, .32f);
                    break;
            }
        }

        private void HandleDownDirectionRaycastCollision()
        {
            if (!Physics.Raycast(_player.transform.position + Vector3.up, Vector3.down,
                    out var hit, 2))
                return;

            switch (hit.collider.tag)
            {
                case "Water":
                    _player.transform.DOMoveY(-2, .32f);
                    game.UpdateState(GameState.Over);

                    break;
            }
        }

        protected override void OnStandby()
        {
            _player.transform.SetParent(null);
            _player.transform.position = Vector3.zero;
            _player.transform.localScale = Vector3.one;
            _player.transform.localEulerAngles = Vector3.zero;
        }

        private static Vector3 GetConvertedDirectionToV3(PlayerDirection playerDirection)
        {
            return playerDirection switch
            {
                PlayerDirection.Forward => Vector3.forward,
                PlayerDirection.Back => Vector3.back,
                PlayerDirection.Left => Vector3.left,
                PlayerDirection.Right => Vector3.right,
                _ => Vector3.zero
            };
        }

        private static Vector3 GetConvertedDirectionToRotateV3(PlayerDirection playerDirection)
        {
            return playerDirection switch
            {
                PlayerDirection.Forward => Vector3.zero,
                PlayerDirection.Back => Vector3.down * 180,
                PlayerDirection.Left => Vector3.down * 90,
                PlayerDirection.Right => Vector3.up * 90,
                _ => Vector3.zero
            };
        }

        public override void OnDisable()
        {
            _becameInvisibleDisposable.Dispose();

            OnPlayerMove = null;
            OnPlayerInvisible = null;

            base.OnDisable();
        }
    }
}