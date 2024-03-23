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
    public sealed class UIPresenterSelectWord
    {
        private HKUIDocument document;

        private readonly Subject<char> onSelectedCharacter = new();

        public Observable<char> OnSelected => onSelectedCharacter;

        public async UniTask BeginAsync(HKUIDocument documentPrefab, CancellationToken token)
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
            document.Q<TMP_Text>("SelectedCharactersText").text = word;
        }

        public async UniTask BeginDecideAnimationAsync(CancellationToken token)
        {
            var SelectCharactersAnimationArea = document.Q<RectTransform>("SelectedCharacters.AnimationArea");
            await LMotion.Create(0.0f, 100.0f, 0.25f)
                .WithEase(Ease.OutCirc)
                .BindToLocalPositionY(SelectCharactersAnimationArea)
                .ToUniTask(token);
            await LMotion.Create(100.0f, 0.0f, 0.25f)
                .WithEase(Ease.InCirc)
                .BindToLocalPositionY(SelectCharactersAnimationArea)
                .ToUniTask(token);
        }
    }
}
