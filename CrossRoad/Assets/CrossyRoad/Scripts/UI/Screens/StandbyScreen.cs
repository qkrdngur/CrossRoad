using CrossyRoad.Util;
using UnityEngine;
using UnityEngine.UI;

namespace CrossyRoad.UI
{
    public class StandbyScreen : UIScreen
    {
        [SerializeField] private Button tapToRunning;

        private void Awake()
        {
            tapToRunning.onClick.AddListener(()
                =>
            {
                GameManager.Instance.UpdateState(GameState.Running);
            });
        }
    }
}