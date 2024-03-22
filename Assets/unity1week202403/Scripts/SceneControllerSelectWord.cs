using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneControllerSelectWord : MonoBehaviour
    {
        private async void Start()
        {
            await BootSystem.IsReady;
        }
    }
}