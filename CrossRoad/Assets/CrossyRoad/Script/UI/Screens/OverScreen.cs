using CrossRoad;
using CrossRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverScreen : UIScreen
{
    [SerializeField] private Button tapToStandby;

    private void Awake()
    {
        tapToStandby.onClick.AddListener(() =>
        {
            GameManager.Instance.UpdateState(GameState.Standby);
        });
    }
}
