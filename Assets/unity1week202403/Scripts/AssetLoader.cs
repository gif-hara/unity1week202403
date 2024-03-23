using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssetLoader
    {
        public static async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(path).ToUniTask() as T;
        }
    }
}
