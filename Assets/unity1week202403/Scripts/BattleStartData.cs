using System;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class BattleStartData
    {
        [SerializeField]
        private ActorStatus playerStatus;

        [SerializeField]
        private ActorStatus enemyStatus;

        public ActorStatus PlayerStatus => playerStatus;

        public ActorStatus EnemyStatus => enemyStatus;
    }
}
