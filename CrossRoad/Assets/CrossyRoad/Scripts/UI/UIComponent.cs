using System.Collections.Generic;
using System.Linq;
using CrossyRoad.Util;
using UnityEngine;

namespace CrossyRoad.UI
{
    public class UIComponent : GameComponent
    {
        private readonly List<UIScreen> _screens = new();

        public UIComponent(GameManager game) : base(game)
        {
            var screens = GameObject.FindGameObjectsWithTag("Screen");

            foreach (var screen in screens)
            {
                _screens.Add(screen.GetComponent<UIScreen>());
            }
        }

        public override void UpdateState(GameState state)
        {
            switch (state)
            {
                case GameState.Init:
                    InitAllScreens();
                    break;
            }

            foreach (var screen in _screens)
            {
                screen.UpdateState(state);
            }

            ActiveScreen(state);
        }

        private void ActiveScreen(GameState state)
        {
            CloseAllScreens();

            foreach (var screen in _screens.Where(s => s.screenState == state))
            {
                screen.UpdateScreen(true);
            }
        }

        private void CloseAllScreens()
        {
            foreach (var screen in _screens)
            {
                screen.UpdateScreen(false);
            }
        }

        private void InitAllScreens()
        {
            foreach (var screen in _screens)
            {
                screen.Initialize();
            }
        }
    }
}