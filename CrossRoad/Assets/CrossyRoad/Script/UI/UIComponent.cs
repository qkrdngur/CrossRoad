using CrossRoad;
using CrossRoad.Util;
using CrossyRoad.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIComponent : GameComponent
{
    private List<UIScreen> _screen = new();

    public UIComponent(GameManager game) : base(game)//base = 상속 받은 클래스에 생성자다(부모를 지칭)
    {
        var screens = GameObject.FindGameObjectsWithTag("Screen");

        foreach(var screen in screens)
        {
            _screen.Add(screen.GetComponent<UIScreen>());
        }
    }

    public override void UpdateState(GameState state)
    {
        switch(state)
        {
            case GameState.Init:
                InitAllScreens();
                break;
        }

        foreach(var screen in _screen)
        {
            screen.UpdateState(state);
        }

        ActionScreen(state);
    }

    public void InitAllScreens()
    {
        foreach(var screen in _screen)
        {
            screen.Initialize();
        }
    }

    public void CloseAllScreens()
    {
        foreach (var screen in _screen)
        {
            screen.UpdateScreen(false);
        }
    }

    private void ActionScreen(GameState state)
    {
        CloseAllScreens();

        foreach(var screen in _screen.Where(s => s.screenState == state))
        {
            screen.UpdateScreen(true);
        }
    }
}
