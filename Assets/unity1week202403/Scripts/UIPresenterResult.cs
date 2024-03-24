using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterResult
    {
        public async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, ResultData resultData, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            var document = UnityEngine.Object.Instantiate(documentPrefab);

            document.Q<TMP_Text>("Word").text = resultData.Word;
            var battleCountValueText = document.Q<TMP_Text>("BattleCount.Value");
            var buttonAnimationArea = document.Q<RectTransform>("ButtonAnimationArea");
            buttonAnimationArea.localPosition = new Vector3(0.0f, -200.0f, 0.0f);

            await LMotion.Create(0, resultData.BattleCount, 1.0f)
                .WithEase(Ease.Linear)
                .BindToText(battleCountValueText)
                .ToUniTask(token);
            await LMotion.Create(Vector3.one, Vector3.one * 2.0f, 0.5f)
                .WithEase(Ease.OutCirc)
                .BindToLocalScale(battleCountValueText.rectTransform)
                .ToUniTask(token);
            await LMotion.Create(Vector3.one * 2.0f, Vector3.one, 0.5f)
                .WithEase(Ease.OutBounce)
                .BindToLocalScale(battleCountValueText.rectTransform)
                .ToUniTask(token);
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
            await LMotion.Create(-200.0f, 0.0f, 0.5f)
                .WithEase(Ease.OutCirc)
                .BindToLocalPositionY(buttonAnimationArea)
                .ToUniTask(token);

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }
    }
}
