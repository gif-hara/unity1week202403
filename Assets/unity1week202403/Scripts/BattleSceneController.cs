using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BattleSceneController : MonoBehaviour
    {
        [SerializeField]
        private BattleStartData debugData;

        [SerializeField]
        private Actor actor;

        private void Start()
        {
            var player = actor.Spawn(debugData.PlayerStatus);
            var enemy = actor.Spawn(debugData.EnemyStatus);
        }
    }
}
