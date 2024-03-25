using System;
using TweetWithScreenShot;
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

        [SerializeField]
        private AudioClip bgm;

        [SerializeField]
        private AudioClip resultSe1;

        [SerializeField]
        private AudioClip resultSe2;

        private async void Start()
        {
            try
            {
                await BootSystem.IsReady;

                TinyServiceLocator.Resolve<AudioController>().StopBgm();
                var uiPresenterResult = new UIPresenterResult();
                var resultData = TinyServiceLocator.TryResolve<ResultData>();
                if (resultData == null)
                {
                    resultData = debugData;
                }
                uiPresenterResult.BeginAsync(
                    resultDocumentPrefab,
                    resultData,
                    resultSe1,
                    resultSe2,
                    bgm,
                    destroyCancellationToken
                    )
                    .Forget();
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
