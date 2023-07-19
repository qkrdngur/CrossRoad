
using CrossRoad;
using CrossRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunningScreen : UIScreen
{
    [SerializeField] private Button tapToOver;

    private void Awake()
    {
        tapToOver.onClick.AddListener(() =>
        {
            GameManager.Instance.UpdateState(GameState.Over);
        });
    }
}
