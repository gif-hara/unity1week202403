using System;
using System.Collections.Generic;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BattleSceneController : MonoBehaviour
    {
        [SerializeField]
        private BattleStartData debugData;

        [SerializeField]
        private Actor actor;

        private async void Start()
        {
            var player = actor.Spawn(debugData.PlayerStatus);
            var enemy = actor.Spawn(debugData.EnemyStatus);

            var actorQueue = new Queue<Actor>();

            if (player.StatusController.Speed == enemy.StatusController.Speed)
            {
                var random = UnityEngine.Random.Range(0, 2);
                actorQueue.Enqueue(random == 0 ? player : enemy);
                actorQueue.Enqueue(random == 0 ? enemy : player);
            }
            else
            {
                actorQueue.Enqueue(player.StatusController.Speed > enemy.StatusController.Speed ? player : enemy);
                actorQueue.Enqueue(player.StatusController.Speed < enemy.StatusController.Speed ? player : enemy);
            }

            while (!player.StatusController.IsDead && !enemy.StatusController.IsDead)
            {
                var currentActor = actorQueue.Dequeue();
                var target = currentActor == player ? enemy : player;

                try
                {
                    await currentActor.PerformActionAsync(target, destroyCancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }

                if (target.StatusController.IsDead)
                {
                    break;
                }

                actorQueue.Enqueue(currentActor);
            }
        }
    }
}