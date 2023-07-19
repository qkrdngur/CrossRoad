using CrossRoad.Util;
using CrossyRoad.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CrossRoad
{
 public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameState State { get; private set; }

        private readonly List<IGameComponent> _components = new();

        private void Awake()
        {
            Instance = this;

            _components.Add(new UIComponent(this));
            _components.Add(new FloorComponent(this));
            _components.Add(new PlayerCompnent(this));
            _components.Add(new InputComponent(this));
        }

        private void Start()
        {
            UpdateState(GameState.Init);
        }

        public void UpdateState(GameState state)
        {
            State = state;

            foreach(var component in _components)
                component.UpdateState(state);

            if(state == GameState.Init)
                UpdateState(GameState.Standby);
        }
    }
}
