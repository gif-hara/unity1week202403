using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
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
            var document = UnityEngine.Object.Instantiate(documentPrefab);

            document.Q<Button>("GotoStartButton").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("SelectWord");
                })
                .RegisterTo(scope.Token);

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }
    }
}
