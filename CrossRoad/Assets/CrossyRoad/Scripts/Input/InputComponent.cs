using System;
using CrossyRoad.Player;
using CrossyRoad.Util;
using UniRx;

namespace CrossyRoad.Input
{
    public class InputComponent : GameComponent
    {
        public event Action<PlayerDirection> OnInputReceived;

        public InputComponent(GameManager game) : base(game)
        {
            Observable.EveryUpdate()
                .Where(stream => game.State == GameState.Running)
                .Where(stream => UnityEngine.Input.anyKeyDown)
                .ThrottleFirst(TimeSpan.FromMilliseconds(250))
                .Select(stream => GetDirection(UnityEngine.Input.inputString))
                .Where(direction => direction != PlayerDirection.None)
                .Subscribe(direction => OnInputReceived?.Invoke(direction))
                .AddTo(game);
        }
        
        private static PlayerDirection GetDirection(string keycode)
        {
            return keycode.ToUpper() switch
            {
                "W" => PlayerDirection.Forward,
                "A" => PlayerDirection.Left,
                "S" => PlayerDirection.Back,
                "D" => PlayerDirection.Right,
                _ => PlayerDirection.None
            };
        }

        public override void OnDisable()
        {
            OnInputReceived = null;
        }
    }
}