using System;
using System.Threading;
using Cysharp.Threading.Tasks;

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
            document.Q<TMPro.TMP_Text>("Message").text = message;
            document.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            document.gameObject.SetActive(false);
        }
    }
}
