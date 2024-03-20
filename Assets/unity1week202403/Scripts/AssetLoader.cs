using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssetLoader
    {
        public static UniTask<T> LoadAsync<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path).ToUniTask();
        }
    }
}
