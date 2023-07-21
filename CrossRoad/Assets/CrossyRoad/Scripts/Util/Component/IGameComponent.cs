namespace CrossyRoad.Util
{
    public interface IGameComponent
    {
        void UpdateState(GameState state);

        void OnDisable();
    }
}