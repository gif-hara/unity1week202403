using System;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ActorStatus
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private int hitPoint;

        [SerializeField]
        private int physicalAttack;

        [SerializeField]
        private int physicalDefense;

        [SerializeField]
        private int magicalAttack;

        [SerializeField]
        private int magicalDefense;

        [SerializeField]
        private int speed;

        public string Name => name;

        public int HitPoint => hitPoint;

        public int PhysicalAttack => physicalAttack;

        public int PhysicalDefense => physicalDefense;

        public int MagicalAttack => magicalAttack;

        public int MagicalDefense => magicalDefense;

        public int Speed => speed;
    }
}
