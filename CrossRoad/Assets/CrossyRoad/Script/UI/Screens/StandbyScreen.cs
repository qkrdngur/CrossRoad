using CrossRoad;
using CrossRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandbyScreen : UIScreen
{
    [SerializeField] private Button tapToRunning;

    private void Awake()
    {
        tapToRunning.onClick.AddListener(() =>
        {
            GameManager.Instance.UpdateState(GameState.Running);
        });
    }
}
