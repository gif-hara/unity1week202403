using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerSelectWord : MonoBehaviour
    {
        [SerializeField]
        private HKUIDocument selectWordDocumentPrefab;

        private string selectedCharacters;

        private async void Start()
        {
            await BootSystem.IsReady;

            var uiPresenterSelectWord = new UIPresenterSelectWord();
            uiPresenterSelectWord.BeginAsync(selectWordDocumentPrefab, destroyCancellationToken).Forget();
            uiPresenterSelectWord.SetWord("");
            var inputScope = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            uiPresenterSelectWord.OnSelected
                .Subscribe(character =>
                {
                    selectedCharacters += character;
                    uiPresenterSelectWord.SetWord(selectedCharacters);
                    if (selectedCharacters.Length == 4)
                    {
                        inputScope.Cancel();
                        inputScope.Dispose();
                    }
                })
                .RegisterTo(inputScope.Token);
            uiPresenterSelectWord.BeginInput(inputScope.Token);
            await inputScope.Token.ToUniTask().Item1;
            await uiPresenterSelectWord.BeginDecideAnimationAsync(destroyCancellationToken);
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: destroyCancellationToken);

            const string selectedCharactersKey = "SelectedCharacters";
            if (TinyServiceLocator.Contains<string>(selectedCharactersKey))
            {
                TinyServiceLocator.Remove<string>(selectedCharactersKey);
            }
            TinyServiceLocator.Register(selectedCharactersKey, selectedCharacters);
            SceneManager.LoadScene("DecideWord");
        }
    }
}