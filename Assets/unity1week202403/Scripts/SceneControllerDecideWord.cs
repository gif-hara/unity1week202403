using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerDecideWord : MonoBehaviour
    {
        [SerializeField]
        private string debugDecideWord;

        [SerializeField]
        private HKUIDocument decideWordDocumentPrefab;

        private async void Start()
        {
            await BootSystem.IsReady;

            var decideWord = TinyServiceLocator.TryResolve<string>("DecideWord");
            if (decideWord == null)
            {
                decideWord = debugDecideWord;
            }

            var uiPresenterDecideWord = new UIPresenterDecideWord();
            uiPresenterDecideWord.BeginAsync(decideWordDocumentPrefab, destroyCancellationToken).Forget();
            await uiPresenterDecideWord.BeginDecideAnimationAsync(debugDecideWord, WordCalculator.Calculate(decideWord), destroyCancellationToken);
        }
    }
}