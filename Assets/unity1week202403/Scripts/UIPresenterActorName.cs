using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
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
        private HKUIDocument document;

        public async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, Actor player, Actor enemy, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            document = UnityEngine.Object.Instantiate(documentPrefab);
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
                document.Q<CanvasGroup>(damageDocumentName).alpha = 0.0f;
                actor.StatusController.TakedDamageAsObservable()
                    .Subscribe(x =>
                    {
                        BeginDamageAnimationAsync(prefix, x).Forget();
                    })
                    .RegisterTo(scope.Token);
            }

            async UniTaskVoid BeginDamageAnimationAsync(string prefix, int damage)
            {
                var damageText = document.Q<TMP_Text>($"{prefix}.Damage");
                var canvasGroup = document.Q<CanvasGroup>($"{prefix}.Damage");
                canvasGroup.alpha = 1.0f;
                damageText.text = damage.ToString();
                LMotion.Shake.Create(0.0f, 80.0f, 0.5f)
                    .BindToAnchoredPositionX(document.Q<RectTransform>($"{prefix}.Name"))
                    .ToUniTask(cancellationToken: scope.Token)
                    .Forget();
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
    }
}
