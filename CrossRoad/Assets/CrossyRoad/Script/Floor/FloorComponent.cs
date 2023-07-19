using CrossRoad;
using CrossRoad.Util;
using CrossyRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorComponent : GameComponent
{
    private List<GameObject> _floor = new();
    GameState state;

    public FloorComponent(GameManager game) : base(game)
    {

    }

    protected override void Initialize()
    {
        for(var i = 0; i < 40; i++)
        {
            var floorType = i % 2 == 0? PoolObjectType.GrassType0 : PoolObjectType.GrassType1;
            var floor = ObjectPool.Instance.GetObject(floorType);
            floor.transform.position = new Vector3(0, -.5f, i - 20);
            _floor.Add(floor);
        }

        base.Initialize();
    }
}
