using CrossRoad;
using CrossyRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompnent : GameComponent
{
    private GameObject _player;

    public PlayerCompnent(GameManager game) : base(game)
    {

    }

    protected override void Initialize()
    {
        _player = ObjectPool.Instance.GetObject(PoolObjectType.Player);
    }
}
