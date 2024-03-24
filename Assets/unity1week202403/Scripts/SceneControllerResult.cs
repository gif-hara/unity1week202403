using System;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerResult : MonoBehaviour
    {
        [SerializeField]
        private ResultData debugData;

        [SerializeField]
        private HKUIDocument resultDocumentPrefab;

        private async void Start()
        {
            try
            {
                await BootSystem.IsReady;

                var uiPresenterResult = new UIPresenterResult();
                var resultData = TinyServiceLocator.TryResolve<ResultData>();
                if (resultData == null)
                {
                    resultData = debugData;
                }
                uiPresenterResult.BeginAsync(resultDocumentPrefab, resultData, destroyCancellationToken).Forget();
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