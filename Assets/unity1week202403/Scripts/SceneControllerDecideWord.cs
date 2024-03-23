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
        private string debugSelectedCharacters;

        [SerializeField]
        private HKUIDocument decideWordDocumentPrefab;

        private async void Start()
        {
            await BootSystem.IsReady;

            var selectedCharacters = TinyServiceLocator.TryResolve<string>("SelectedCharacters");
            if (selectedCharacters == null)
            {
                selectedCharacters = debugSelectedCharacters;
            }
            var decideWord = WordCalculator.Calculate(selectedCharacters);
            var actorStatus = WordCalculator.ToActorStatus(decideWord);
            const string playerStatusKey = "PlayerStatus";
            if (TinyServiceLocator.Contains<ActorStatus>(playerStatusKey))
            {
                TinyServiceLocator.Remove<ActorStatus>(playerStatusKey);
            }
            TinyServiceLocator.Register(playerStatusKey, actorStatus);

            var uiPresenterDecideWord = new UIPresenterDecideWord();
            uiPresenterDecideWord.BeginAsync(decideWordDocumentPrefab, destroyCancellationToken).Forget();
            await uiPresenterDecideWord.BeginDecideAnimationAsync(
                selectedCharacters,
                decideWord,
                "TODO",
                actorStatus,
                destroyCancellationToken
                );
        }
    }
}
