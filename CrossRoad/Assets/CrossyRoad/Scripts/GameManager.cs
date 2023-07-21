using System.Collections.Generic;
using CrossyRoad.CrossyCamera;
using CrossyRoad.Eagle;
using CrossyRoad.Floor;
using CrossyRoad.Input;
using CrossyRoad.Player;
using CrossyRoad.Score;
using CrossyRoad.UI;
using CrossyRoad.Util;
using UnityEngine;

namespace CrossyRoad
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
            _components.Add(new PlayerComponent(this));
            _components.Add(new InputComponent(this));
            _components.Add(new CameraComponent(this));
            _components.Add(new EagleComponent(this));
            _components.Add(new ScoreComponent(this));
        }

        private void Start()
        {
            UpdateState(GameState.Init);
        }

        public void UpdateState(GameState state)
        {
            State = state;

            foreach (var component in _components)
                component.UpdateState(state);

            if (state == GameState.Init)
                UpdateState(GameState.Standby);
        }

        public T GetGameComponent<T>() where T : GameComponent
        {
            var component = default(T); 

            foreach (var gameComponent in _components)
            {
                if (gameComponent is not T tComponent) continue;

                component = tComponent;

                break;
            }

            return component;
        }

        public void OnApplicationQuit()
        {
            foreach (var component in _components)
            {
                component.OnDisable();
            }
            
            _components.Clear();
        }
    }
}