using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterSelectWord
    {
        private HKUIDocument document;

        private readonly Subject<char> onSelectedCharacter = new();

        public Observable<char> OnSelected => onSelectedCharacter;

        public async UniTask Begin(HKUIDocument documentPrefab, CancellationToken token)
        {
            document = UnityEngine.Object.Instantiate(documentPrefab);
            await UniTask.WaitUntilCanceled(token);
            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }

        public void BeginInput(CancellationToken token)
        {
            for (var i = 'A'; i <= 'Z'; i++)
            {
                var button = document.Q<UnityEngine.UI.Button>($"UI.Parts.Button.{i}");
                var c = i;
                button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        button.interactable = false;
                        onSelectedCharacter.OnNext(c);
                    })
                    .RegisterTo(token);
            }
        }

        public void SetWord(string word)
        {
            document.Q<TMP_Text>("SelectedCharacters").text = word;
        }
    }
}
