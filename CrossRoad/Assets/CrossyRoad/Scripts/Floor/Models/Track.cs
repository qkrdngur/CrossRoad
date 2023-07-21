using DG.Tweening;
using UnityEngine;

namespace CrossyRoad.Floor
{
    public class Track : Floor
    {
        [SerializeField] private GameObject train;

        [SerializeField] private Light trafficLight;

        [SerializeField] private GameObject bell;

        private const float Speed = 5f;

        public override void Generate()
        {
            TrainLoop();
        }

        private void TrainLoop()
        {
            var direction = Random.Range(0, 101) > 50 ? -1 : 1;

            train.transform.position = new Vector3(40 * direction, 0,
                transform.position.z);

            train.transform.GetChild(0).localEulerAngles = new Vector3(0,
                direction == 1 ? 0 : 180, 0);

            float delayTime = Random.Range(1, 3); 

            bell.transform.DOScale(Vector3.one * 1.5f, .16f)
                .SetLoops(6, LoopType.Yoyo).SetDelay(delayTime);

            trafficLight.DOIntensity(10, .16f)
                .SetLoops(6, LoopType.Yoyo)
                .SetDelay(delayTime).OnComplete(() =>
                {
                    train.transform.DOLocalMoveX(.4f * -direction, Speed)
                        .SetDelay(delayTime).OnComplete(TrainLoop);
                });
        }

        public override void GenerateCoin(int coinCount)
        {
            base.GenerateCoin(coinCount);

            foreach (var coin in coins)
            {
                var coinPosition = new Vector3(Random.Range(-10, 11), 0, transform.position.z);

                if(IsCoinOverLap(coinPosition))
                    continue;
                
                coin.transform.position = coinPosition;
                
                break;
            }
        }
        
        public override void Reset()
        {
            train.transform.DOKill();
            trafficLight.DOKill();
            bell.transform.DOKill();

            bell.transform.localScale = Vector3.one;
            trafficLight.intensity = 0;
            
            base.Reset();
        }
    }
}