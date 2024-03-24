using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterTitle
    {
        public async UniTaskVoid BeginAsync(HKUIDocument documentPrefab, CancellationToken token)
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            var document = Object.Instantiate(documentPrefab);

            document.Q<Button>("GotoStartButton").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("SelectWord");
                })
                .RegisterTo(scope.Token);

            LMotion.Create(-10.0f, 10.0f, 1.0f)
                .WithEase(Ease.InOutQuad)
                .WithLoops(-1, LoopType.Yoyo)
                .BindToLocalEulerAnglesZ(document.Q<RectTransform>("EffectArea"))
                .ToUniTask(scope.Token)
                .Forget();

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                Object.Destroy(document.gameObject);
            }
        }
    }
}
