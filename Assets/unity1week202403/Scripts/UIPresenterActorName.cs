using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterActorName
    {
        private HKUIDocument document;

        public async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, Actor player, Actor enemy, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            document = UnityEngine.Object.Instantiate(documentPrefab);
            BeginObserve(player, "Player");
            BeginObserve(enemy, "Enemy");
            var playbackButtonText = document.Q<TMP_Text>("PlaybackSpeedButton.Text");
            playbackButtonText.text = $"{Time.timeScale}倍速";
            document.Q<Button>("PlaybackSpeedButton").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Time.timeScale = Time.timeScale == 1.0f
                        ? 4.0f
                        : Time.timeScale == 4.0f
                        ? 8.0f
                        : 1.0f;
                    playbackButtonText.text = $"{Time.timeScale}倍速";
                })
                .RegisterTo(scope.Token);

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }

            void BeginObserve(Actor actor, string prefix)
            {
                var damageDocumentName = $"{actor.ActorType}.Damage";
                document.Q<TMP_Text>($"{actor.ActorType}.Name").text = actor.StatusController.Name;
                document.Q<CanvasGroup>(damageDocumentName).alpha = 0.0f;
                actor.StatusController.TakedDamageAsObservable()
                    .Subscribe(x =>
                    {
                        BeginDamageAnimationAsync(actor.ActorType, x).Forget();
                    })
                    .RegisterTo(scope.Token);
                actor.StatusController.RecoveryAsObservable()
                    .Subscribe(x =>
                    {
                        BeginNameRecoversAnimationAsync(actor.ActorType, x, scope.Token).Forget();
                    })
                    .RegisterTo(scope.Token);
                Observable.Merge(
                    actor.StatusController.Buffs[Define.BuffType.PhysicalStrength].Pairwise(),
                    actor.StatusController.Buffs[Define.BuffType.PhysicalDefense].Pairwise(),
                    actor.StatusController.Buffs[Define.BuffType.MagicalStrength].Pairwise(),
                    actor.StatusController.Buffs[Define.BuffType.MagicalDefense].Pairwise(),
                    actor.StatusController.Buffs[Define.BuffType.Speed].Pairwise()
                )
                    .Where(_ => !actor.StatusController.IsResetting)
                    .Subscribe(x =>
                    {
                        var diff = x.Current - x.Previous;
                        if (diff > 0)
                        {
                            BeginNameRecoversAnimationAsync(actor.ActorType, diff, scope.Token).Forget();
                        }
                        else
                        {
                            BeginNameDamageAnimationAsync(actor.ActorType, scope.Token).Forget();
                        }
                    })
                    .RegisterTo(scope.Token);
            }

            async UniTaskVoid BeginDamageAnimationAsync(Define.ActorType actorType, int damage)
            {
                var damageText = document.Q<TMP_Text>($"{actorType}.Damage");
                var canvasGroup = document.Q<CanvasGroup>($"{actorType}.Damage");
                canvasGroup.alpha = 1.0f;
                damageText.text = damage.ToString();
                BeginNameDamageAnimationAsync(actorType, scope.Token).Forget();
                await LMotion.Create(-30.0f, 30.0f, 0.5f)
                    .WithEase(Ease.OutCirc)
                    .BindToAnchoredPositionY(damageText.rectTransform)
                    .ToUniTask(cancellationToken: scope.Token);
                await LMotion.Create(1.0f, 0.0f, 0.5f)
                    .WithEase(Ease.OutCirc)
                    .BindToCanvasGroupAlpha(canvasGroup)
                    .ToUniTask(cancellationToken: scope.Token);
            }
        }

        public void SetEnemyName(string name)
        {
            document.Q<TMP_Text>($"Enemy.Name").text = name;
        }

        public async UniTask BeginSkillNameAnimationAsync(Define.ActorType actorType, string skillName, CancellationToken token)
        {
            document.Q<TMP_Text>($"{actorType}.SkillName.Text").text = skillName;
            var skillNameAnimationArea = document.Q<RectTransform>($"{actorType}.SkillName.AnimationArea");
            var skillNameCanvasGroup = document.Q<CanvasGroup>($"{actorType}.SkillName.AnimationArea");
            skillNameCanvasGroup.alpha = 0.0f;
            await UniTask.WhenAll(
                LMotion.Create(0.0f, 1.0f, 0.25f)
                    .WithEase(Ease.OutCirc)
                    .BindToCanvasGroupAlpha(skillNameCanvasGroup)
                    .ToUniTask(token),
                LMotion.Create(-60.0f, 0.0f, 0.25f)
                    .WithEase(Ease.OutCirc)
                    .BindToLocalPositionY(skillNameAnimationArea)
                    .ToUniTask(token)
            );
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
            await UniTask.WhenAll(
                LMotion.Create(1.0f, 0.0f, 0.25f)
                    .WithEase(Ease.OutCirc)
                    .BindToCanvasGroupAlpha(skillNameCanvasGroup)
                    .ToUniTask(token),
                LMotion.Create(0.0f, 60.0f, 0.25f)
                    .WithEase(Ease.OutCirc)
                    .BindToLocalPositionY(skillNameAnimationArea)
                    .ToUniTask(token)
            );
        }

        private async UniTask BeginNameRecoversAnimationAsync(Define.ActorType actorType, int recovers, CancellationToken token)
        {
            await LMotion.Create(0.0f, 60.0f, 0.25f)
                .WithEase(Ease.OutCirc)
                .BindToAnchoredPositionY(document.Q<RectTransform>($"{actorType}.Name"))
                .ToUniTask(cancellationToken: token);
            await LMotion.Create(60.0f, 0.0f, 0.25f)
                .WithEase(Ease.InCirc)
                .BindToAnchoredPositionY(document.Q<RectTransform>($"{actorType}.Name"))
                .ToUniTask(cancellationToken: token);
        }

        private UniTask BeginNameDamageAnimationAsync(Define.ActorType actorType, CancellationToken token)
        {
            return LMotion.Shake.Create(0.0f, 80.0f, 0.5f)
                .BindToAnchoredPositionX(document.Q<RectTransform>($"{actorType}.Name"))
                .ToUniTask(cancellationToken: token);
        }
    }
}
