using System;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerTitle : MonoBehaviour
    {
        [SerializeField]
        private HKUIDocument titleDocumentPrefab;

        private async void Start()
        {
            try
            {
                await BootSystem.IsReady;
                var uiPresenterTitle = new UIPresenterTitle();
                uiPresenterTitle.BeginAsync(titleDocumentPrefab, destroyCancellationToken).Forget();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}