using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterStatus
    {
        public static async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, Actor player, Actor enemy, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            var document = Object.Instantiate(documentPrefab);
            BeginObserve(player, "Player");
            BeginObserve(enemy, "Enemy");

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                Object.Destroy(document.gameObject);
            }

            void BeginObserve(Actor actor, string prefix)
            {
                actor.StatusController.HitPointAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.HitPoint.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.HitPoint.Slider").value = actor.StatusController.HitPointRate;
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.PhysicalStrengthAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.PhysicalStrength.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.PhysicalStrength.Slider").value = actor.StatusController.PhysicalStrengthRate;
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.PhysicalDefenseAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.PhysicalDefense.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.PhysicalDefense.Slider").value = actor.StatusController.PhysicalDefenseRate;
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.MagicalStrengthAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.MagicalStrength.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.MagicalStrength.Slider").value = actor.StatusController.MagicalStrengthRate;
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.MagicalDefenseAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.MagicalDefense.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.MagicalDefense.Slider").value = actor.StatusController.MagicalDefenseRate;
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.SpeedAsObservable()
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Speed.ValueText").SetText(x.ToString());
                        document.Q<Slider>($"{prefix}.Speed.Slider").value = actor.StatusController.SpeedRate;
                    })
                    .RegisterTo(scope.Token);
            }
        }
    }
}
