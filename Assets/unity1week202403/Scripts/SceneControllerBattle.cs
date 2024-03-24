using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerBattle : MonoBehaviour
    {
        [SerializeField]
        private BattleStartData debugData;

        [SerializeField]
        private Actor actor;

        [SerializeField]
        private HKUIDocument actorNameDocumentPrefab;

        [SerializeField]
        private HKUIDocument statusDocumentPrefab;

        [SerializeField]
        private HKUIDocument mainMessageDocumentPrefab;

        private async void Start()
        {
            try
            {
                await BootSystem.IsReady;
                var playerStatus = TinyServiceLocator.TryResolve<ActorStatus>("PlayerStatus");
                if (playerStatus == null)
                {
                    playerStatus = debugData.PlayerStatus;
                }
                var player = actor.Spawn(playerStatus);
                var enemyStatus = GetRandomActorStatus();
                var enemy = actor.Spawn(enemyStatus);

                var uiPresenterActorName = new UIPresenterActorName();
                uiPresenterActorName.BeginAsync(actorNameDocumentPrefab, player, enemy, destroyCancellationToken).Forget();
                UIPresenterStatus.BeginAsync(statusDocumentPrefab, player, enemy, destroyCancellationToken).Forget();
                var uiPresenterMainMessage = new UIPresenterMainMessage();
                uiPresenterMainMessage.BeginAsync(mainMessageDocumentPrefab, destroyCancellationToken).Forget();

                await uiPresenterMainMessage.PlayAnimationBattleStartAsync();

                // バトル処理
                while (true)
                {
                    await ProccessTurnAsync();
                    if (player.StatusController.IsDead)
                    {
                        await uiPresenterMainMessage.PlayAnimationAsync("You Lose...", destroyCancellationToken);
                        break;
                    }
                    else
                    {
                        await uiPresenterMainMessage.PlayAnimationPlayerWinAsync();
                        player.StatusController.ResetAll();
                        enemyStatus = GetRandomActorStatus();
                        enemy.StatusController.Reset(enemyStatus);
                        uiPresenterActorName.SetEnemyName(enemy.StatusController.Name);
                    }
                }

                // 終了処理

                static ActorStatus GetRandomActorStatus()
                {
                    var wordSpecs = TinyServiceLocator.Resolve<MasterData>().WordSpecs;
                    var wordSpec = wordSpecs.List[UnityEngine.Random.Range(0, wordSpecs.List.Count)];
                    return WordCalculator.ToActorStatus(wordSpec.Word);
                }

                async UniTask ProccessTurnAsync()
                {
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
                            await UniTask.Delay(TimeSpan.FromSeconds(1));
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
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}