using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerSelectWord : MonoBehaviour
    {
        [SerializeField]
        private HKUIDocument selectWordDocumentPrefab;

        private string selectedWord;

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
                    selectedWord += character;
                    uiPresenterSelectWord.SetWord(selectedWord);
                    if (selectedWord.Length == 4)
                    {
                        inputScope.Cancel();
                        inputScope.Dispose();
                    }
                })
                .RegisterTo(inputScope.Token);
            uiPresenterSelectWord.BeginInput(inputScope.Token);
            await UniTask.WaitUntilCanceled(inputScope.Token);
            Debug.Log(selectedWord);
        }
    }
}