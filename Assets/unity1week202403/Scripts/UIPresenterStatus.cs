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
                actor.StatusController.Buffs[Define.BuffType.PhysicalStrength]
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Buff.PhysicalStrength.Text").SetText(GetBuffLevelText(x));
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.Buffs[Define.BuffType.PhysicalDefense]
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Buff.PhysicalDefense.Text").SetText(GetBuffLevelText(x));
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.Buffs[Define.BuffType.MagicalStrength]
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Buff.MagicalStrength.Text").SetText(GetBuffLevelText(x));
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.Buffs[Define.BuffType.MagicalDefense]
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Buff.MagicalDefense.Text").SetText(GetBuffLevelText(x));
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.Buffs[Define.BuffType.Speed]
                    .Subscribe(x =>
                    {
                        document.Q<TMP_Text>($"{prefix}.Buff.Speed.Text").SetText(GetBuffLevelText(x));
                    })
                    .RegisterTo(scope.Token);
            }

            static string GetBuffLevelText(int level)
            {
                var colorCode = level > 0
                    ? "#00FF00"
                    : level < 0
                    ? "#FF0000"
                    : "#FFFFFF";
                return $"<color={colorCode}>{level}</color>";
            }
        }
    }
}
