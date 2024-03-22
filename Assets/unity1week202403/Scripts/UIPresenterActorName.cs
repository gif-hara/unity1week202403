using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
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
            var document = UnityEngine.Object.Instantiate(documentPrefab);
            BeginObserve(player, "Player");
            BeginObserve(enemy, "Enemy");

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }

            void BeginObserve(Actor actor, string prefix)
            {
                var damageDocumentName = $"{prefix}.Damage";
                document.Q<TMP_Text>($"{prefix}.Name").text = actor.StatusController.Name;
                document.Q<TMP_Text>(damageDocumentName).gameObject.SetActive(false);
                actor.StatusController.TakedDamageAsObservable()
                    .Subscribe(x =>
                    {
                        BeginDamageAnimationAsync(damageDocumentName, x).Forget();
                    })
                    .RegisterTo(scope.Token);
            }

            async UniTaskVoid BeginDamageAnimationAsync(string documentName, int damage)
            {
                var text = document.Q<TMP_Text>(documentName);
                text.gameObject.SetActive(true);
                text.text = damage.ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: scope.Token);
                text.gameObject.SetActive(false);
            }
        }
    }
}
