using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterActorName
    {
        public static async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, Actor player, Actor enemy, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            var document = Object.Instantiate(documentPrefab);
            BeginObserve(player, "Player");
            BeginObserve(enemy, "Enemy");

            await UniTask.WaitUntilCanceled(scope.Token);

            Object.Destroy(document);

            void BeginObserve(Actor actor, string prefix)
            {
                document.Q<TMP_Text>($"{prefix}.Name").text = actor.StatusController.Name;
            }
        }
    }
}
