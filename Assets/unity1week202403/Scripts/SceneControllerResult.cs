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

        private async void Start()
        {
            try
            {
                await BootSystem.IsReady;
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