using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySequencerSystem.StandardSequences;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterDecideWord
    {
        private HKUIDocument document;

        public async UniTask BeginAsync(HKUIDocument documentPrefab, CancellationToken token)
        {
            document = UnityEngine.Object.Instantiate(documentPrefab);
            await UniTask.WaitUntilCanceled(token);
            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }

        public async UniTask BeginDecideAnimationAsync(string beforeText, string afterText, string descriptionText, ActorStatus actorStatus, CancellationToken token)
        {
            var word = document.Q<TMP_Text>("Word");
            var playerArea = document.Q<CanvasGroup>("PlayerArea");
            var description = document.Q<TMP_Text>("Description");
            var descriptionCanvasGroup = document.Q<CanvasGroup>("Description");
            var skillSpecsCanvasGroup = document.Q<CanvasGroup>("SkillSpecs");
            var buttonAnimationArea = document.Q<RectTransform>("Button.AnimationArea");
            description.text = descriptionText;
            descriptionCanvasGroup.alpha = 0.0f;
            playerArea.alpha = 0.0f;
            skillSpecsCanvasGroup.alpha = 0.0f;
            buttonAnimationArea.localPosition = new Vector3(0.0f, -160.0f, 0.0f);
            document.Q<Slider>("HitPoint.Slider").value = actorStatus.hitPoint;
            document.Q<TMP_Text>("HitPoint.ValueText").text = actorStatus.hitPoint.ToString();
            document.Q<Slider>("PhysicalStrength.Slider").value = actorStatus.physicalStrength / Define.ParameterMax;
            document.Q<TMP_Text>("PhysicalStrength.ValueText").text = actorStatus.physicalStrength.ToString();
            document.Q<Slider>("PhysicalDefense.Slider").value = actorStatus.physicalDefense / Define.ParameterMax;
            document.Q<TMP_Text>("PhysicalDefense.ValueText").text = actorStatus.physicalDefense.ToString();
            document.Q<Slider>("MagicalStrength.Slider").value = actorStatus.magicalStrength / Define.ParameterMax;
            document.Q<TMP_Text>("MagicalStrength.ValueText").text = actorStatus.magicalStrength.ToString();
            document.Q<Slider>("MagicalDefense.Slider").value = actorStatus.magicalDefense / Define.ParameterMax;
            document.Q<TMP_Text>("MagicalDefense.ValueText").text = actorStatus.magicalDefense.ToString();
            document.Q<Slider>("Speed.Slider").value = actorStatus.speed / Define.ParameterMax;
            document.Q<TMP_Text>("Speed.ValueText").text = actorStatus.speed.ToString();
            var sb = new StringBuilder();
            sb.AppendLine("スキル");
            foreach (var skillId in actorStatus.skillIds)
            {
                var skill = TinyServiceLocator.Resolve<MasterData>().SkillSpecs.Get(skillId);
                sb.AppendLine($"    {skill.Name}");
            }
            document.Q<TMP_Text>("SkillSpecText").text = sb.ToString();
            word.text = beforeText;
            document.Q<Button>("GotoBattleButton")
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("Battle");
                })
                .RegisterTo(token);
            document.Q<Button>("GotoSelectWordButton")
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("SelectWord");
                })
                .RegisterTo(token);
            await LMotion.Shake.Create(0.0f, 30.0f, 1.0f)
                .BindToLocalPositionX(word.rectTransform)
                .ToUniTask(cancellationToken: token);
            await LMotion.Shake.Create(0.0f, 100.0f, 0.5f)
                .BindToLocalPositionX(word.rectTransform)
                .ToUniTask(cancellationToken: token);
            word.text = afterText;
            await LMotion.Create(Vector3.one, Vector3.one * 1.5f, 0.1f)
                .WithEase(Ease.OutQuad)
                .BindToLocalScale(word.rectTransform)
                .ToUniTask(cancellationToken: token);
            await LMotion.Create(Vector3.one * 1.5f, Vector3.one, 0.5f)
                .WithEase(Ease.OutBounce)
                .BindToLocalScale(word.rectTransform)
                .ToUniTask(cancellationToken: token);
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
            await UniTask.WhenAll(
                LMotion.Create(new Vector2(0.0f, 0.0f), new Vector2(0.0f, 0.5f), 0.5f)
                    .WithEase(Ease.OutQuad)
                    .BindToAnchorMin(word.rectTransform)
                    .ToUniTask(cancellationToken: token),
                LMotion.Create(new Vector2(1.0f, 1.0f), new Vector2(0.5f, 1.0f), 0.5f)
                    .WithEase(Ease.OutQuad)
                    .BindToAnchorMax(word.rectTransform)
                    .ToUniTask(cancellationToken: token),
                LMotion.Create(0.0f, 1.0f, 0.5f)
                    .BindToCanvasGroupAlpha(descriptionCanvasGroup)
                    .ToUniTask(cancellationToken: token),
                LMotion.Create(0.0f, 1.0f, 0.5f)
                    .BindToCanvasGroupAlpha(playerArea)
                    .ToUniTask(cancellationToken: token),
                LMotion.Create(0.0f, 1.0f, 0.5f)
                    .BindToCanvasGroupAlpha(skillSpecsCanvasGroup)
                    .ToUniTask(cancellationToken: token)
            );
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            await LMotion.Create(-160.0f, 0.0f, 0.5f)
                .WithEase(Ease.OutBack)
                .BindToLocalPositionY(buttonAnimationArea)
                .ToUniTask(cancellationToken: token);
        }
    }
}
