using CrossyRoad.Score;
using CrossyRoad.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrossyRoad.UI
{
    public class RunningScreen : UIScreen
    {
        [SerializeField] private Button tapToOver;

        [SerializeField] private TextMeshProUGUI score;

        [SerializeField] private TextMeshProUGUI bestScore;
        
        public override void Initialize()
        {
            tapToOver.onClick.AddListener(() =>
            {
                GameManager.Instance.UpdateState(GameState.Over);
            });

            GameManager.Instance.GetGameComponent<ScoreComponent>()
                .OnScoreChanged += data =>
            {
                score.text = data.score.ToString();
                bestScore.text = $"Best : {data.bestScore}";
            };
            
            base.Initialize();
        }
    }
}