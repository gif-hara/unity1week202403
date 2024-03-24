using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// ブートシステム
    /// </summary>
    public sealed class BootSystem
    {
        /// <summary>
        /// ブートシステムが初期化完了したか返す
        /// </summary>
        public static UniTask IsReady
        {
            get
            {
                return UniTask.WaitUntil(() => initializeState == InitializeState.Initialized);
            }
        }

        /// <summary>
        /// 初期化の状態
        /// </summary>
        private enum InitializeState
        {
            None,
            Initializing,
            Initialized,
        }

        private static InitializeState initializeState = InitializeState.None;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            InitializeInternalAsync().Forget();
        }

        private static async UniTask InitializeInternalAsync()
        {
            initializeState = InitializeState.Initializing;
            await UniTask.WhenAll(
                InitializeMasterDataAsync(),
                InitializeAudioControllerAsync()
            );
            initializeState = InitializeState.Initialized;
        }

        private static async UniTask InitializeMasterDataAsync()
        {
            var masterData = await AssetLoader.LoadAsync<MasterData>("MasterData");
            TinyServiceLocator.Register(masterData);
        }

        private static async UniTask InitializeAudioControllerAsync()
        {
            var audioControllerPrefab = await AssetLoader.LoadAsync<AudioController>("AudioController");
            var audioController = Object.Instantiate(audioControllerPrefab);
            Object.DontDestroyOnLoad(audioController.gameObject);
            TinyServiceLocator.Register(audioController);
        }
    }
}