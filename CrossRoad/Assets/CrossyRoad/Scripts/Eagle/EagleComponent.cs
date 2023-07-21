using CrossyRoad.Player;
using CrossyRoad.Util;
using CrossyRoad.Util.Pool;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace CrossyRoad.Eagle
{
    public class EagleComponent : GameComponent
    {
        private GameObject _eagle;

        public EagleComponent(GameManager game) : base(game)
        {
        }

        protected override void Initialize()
        {
            game.GetGameComponent<PlayerComponent>().OnPlayerInvisible += PlayerKill;
        }

        private void PlayerKill(GameObject player)
        {
            _eagle = ObjectPool.Instance.GetObject(PoolObjectType.Eagle);

            _eagle.transform.position = player.transform.position 
                                        + (Vector3.up * 2) 
                                        + (Vector3.forward * 25);
            
            _eagle.transform.DOMoveZ(player.transform.position.z, .75f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    player.transform.SetParent(_eagle.transform);

                    _eagle.transform.DOMoveZ(player.transform.position.z - 25, .75f)
                        .SetEase(Ease.Linear).OnComplete(() =>
                        {
                            ObjectPool.Instance.ReturnObject(PoolObjectType.Eagle, _eagle);
                        });
                });
        }
    }
}