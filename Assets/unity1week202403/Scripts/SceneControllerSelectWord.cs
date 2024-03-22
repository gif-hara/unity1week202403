using System;
using System.Collections.Generic;
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
            uiPresenterSelectWord.Begin(selectWordDocumentPrefab, destroyCancellationToken).Forget();
            uiPresenterSelectWord.SetWord("");
            uiPresenterSelectWord.OnSelected
                .Subscribe(character =>
                {
                    selectedWord += character;
                    uiPresenterSelectWord.SetWord(selectedWord);
                });
            uiPresenterSelectWord.BeginInput(destroyCancellationToken);
        }
    }
}