using CrossyRoad.Util;
using UnityEngine;

namespace CrossyRoad.UI
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] internal GameState screenState;

        [SerializeField] internal CanvasGroup canvasGroup;

        public virtual void UpdateScreen(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        public virtual void UpdateState(GameState state)
        {
            
        }

        public virtual void Initialize()
        {
            
        }
    }
}