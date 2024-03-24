using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterMainMessage
    {
        private HKUIDocument document;

        public async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            document = UnityEngine.Object.Instantiate(documentPrefab);

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }

        public async UniTask PlayAnimationAsync(string message, CancellationToken token)
        {
            SetMessage(message);
            var messageArea = document.Q<RectTransform>("MessageArea");
            var messageAreaCanvasGroup = document.Q<CanvasGroup>("MessageArea");
            var backgroundCanvasGroup = document.Q<CanvasGroup>("Background");
            messageAreaCanvasGroup.alpha = 1.0f;
            backgroundCanvasGroup.alpha = 1.0f;
            await LMotion.Create(0.0f, 1.0f, 0.5f)
                .WithEase(Ease.OutCirc)
                .BindToLocalScaleY(messageArea)
                .ToUniTask(token);
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
            await UniTask.WhenAll(
                LMotion.Create(1.0f, 0.0f, 0.5f)
                    .WithEase(Ease.OutCirc)
                    .BindToCanvasGroupAlpha(messageAreaCanvasGroup)
                    .ToUniTask(token),
                LMotion.Create(1.0f, 0.0f, 0.5f)
                    .WithEase(Ease.OutCirc)
                    .BindToCanvasGroupAlpha(backgroundCanvasGroup)
                    .ToUniTask(token)
            );
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        }

        private void SetMessage(string message)
        {
            document.Q<TMPro.TMP_Text>("Message").text = message;
        }
    }
}
