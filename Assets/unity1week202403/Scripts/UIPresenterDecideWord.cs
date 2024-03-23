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

        public async UniTask BeginDecideAnimationAsync(string beforeText, string afterText, CancellationToken token)
        {
            var word = document.Q<TMP_Text>("Word");
            word.text = beforeText;
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
        }
    }
}
