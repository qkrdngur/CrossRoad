using CrossyRoad.Util;
using UnityEngine;
using UnityEngine.UI;

namespace CrossyRoad.UI
{
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
}