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
            var masterData = await AssetLoader.LoadAsync<MasterData>("Assets/unity1week202403/Database/MasterData.asset");
            TinyServiceLocator.Register(masterData);
            initializeState = InitializeState.Initialized;
        }
    }
}