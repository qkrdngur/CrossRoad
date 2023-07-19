using CrossRoad;
using CrossRoad.Util;
using CrossyRoad.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class InputComponent : GameComponent
{
    private readonly IObservable<PlayerDirection> _clickStream;

    public InputComponent(GameManager game) : base(game)
    {
        _clickStream = Observable.EveryUpdate()
            .Where(Stream => UnityEngine.Input.anyKeyDown)
            .ThrottleFirst(TimeSpan.FromSeconds(250))
            .Where(Stream => game.State == GameState.Running)
            .Select(Stream => GetDirection(UnityEngine.Input.inputString))
            .Where(direction => direction != PlayerDirection.None);

        _clickStream.Subscribe(direction =>
        {
            Debug.Log(direction);
        });
    }

    private PlayerDirection GetDirection(string keycode)
    {
        return keycode.ToUpper() switch
        {
            "W" => PlayerDirection.Forward,
            "A" => PlayerDirection.Left,
            "S" => PlayerDirection.Back,
            "D" => PlayerDirection.Right,
            _ => PlayerDirection.None
        } ;
    }

    public void Subscribe(Action<PlayerDirection> action)
    {
        _clickStream.Subscribe(action).AddTo(game);
    }
}
