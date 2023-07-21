namespace CrossyRoad.Util
{
    public abstract class GameComponent : IGameComponent
    {
        protected readonly GameManager game;

        protected GameComponent(GameManager game)
        {
            this.game = game;
        }
        
        public virtual void UpdateState(GameState state)
        {
            switch (state)
            {
                case GameState.Init:
                    Initialize();
                    break;
                case GameState.Standby:
                    OnStandby();
                    break;
                case GameState.Running:
                    OnRunning();
                    break;
                case GameState.Over:
                    OnOver();
                    break;
            }
        }

        protected virtual void Initialize()
        {
            
        }

        protected virtual void OnStandby()
        {
            
        }
        
        protected virtual void OnRunning()
        {
            
        }
        
        protected virtual void OnOver()
        {
            
        }

        public virtual void OnDisable()
        {
            
        }
    }
}