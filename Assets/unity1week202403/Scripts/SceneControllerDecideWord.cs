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

        [SerializeField]
        private AudioClip decideSe1;

        [SerializeField]
        private AudioClip decideSe2;

        private async void Start()
        {
            await BootSystem.IsReady;

            TinyServiceLocator.Resolve<AudioController>().StopLoop();
            var selectedCharacters = TinyServiceLocator.TryResolve<string>("SelectedCharacters");
            if (selectedCharacters == null)
            {
                selectedCharacters = debugSelectedCharacters;
            }
            var decideWord = WordCalculator.Calculate(selectedCharacters);
            var actorStatus = WordCalculator.ToActorStatus(decideWord);
            var wordSpec = TinyServiceLocator.Resolve<MasterData>().WordSpecs.Get(decideWord);
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
                wordSpec.Description,
                actorStatus,
                decideSe1,
                decideSe2,
                destroyCancellationToken
                );
        }
    }
}
