using System;
using CrossyRoad.Player;
using CrossyRoad.Util;
using UnityEngine;

namespace CrossyRoad.Score
{
    public class ScoreComponent : GameComponent
    {
        public event Action<Score> OnScoreChanged;

        private const string BestScoreKey = "bestScoreKey";
        
        private int _score;

        private int _bestScore;
        
        public ScoreComponent(GameManager game) : base(game)
        { 
            if (!PlayerPrefs.HasKey(BestScoreKey))
            {
                PlayerPrefs.SetInt(BestScoreKey, 0);
            }

            _bestScore = PlayerPrefs.GetInt(BestScoreKey);
        }
        
        protected override void Initialize()
        {
            game.GetGameComponent<PlayerComponent>().OnPlayerMove
                += playerPosition =>
                {
                    if(!(_score < playerPosition.z))
                        return;

                    _score++;

                    if (_score > _bestScore)
                        _bestScore = _score;
                    
                    OnScoreChanged?.Invoke(new Score(_score, _bestScore));
                };
        }

        protected override void OnStandby()
        {
            _score = 0;
            
            OnScoreChanged?.Invoke(new Score(_score, _bestScore));
        }

        public override void OnDisable()
        {
            OnScoreChanged = null;
        }
    }
}