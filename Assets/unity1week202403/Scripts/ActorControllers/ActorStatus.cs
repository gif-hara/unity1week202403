using System;
using System.Collections.Generic;
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
        public string name;

        [SerializeField]
        public int hitPoint;

        [SerializeField]
        public int physicalAttack;

        [SerializeField]
        public int physicalDefense;

        [SerializeField]
        public int magicalAttack;

        [SerializeField]
        public int magicalDefense;

        [SerializeField]
        public int speed;

        [SerializeField]
        public List<int> skillIds;

        public ActorStatus(
            string name,
            int hitPoint,
            int physicalAttack,
            int physicalDefense,
            int magicalAttack,
            int magicalDefense,
            int speed,
            List<int> skillIds
            )
        {
            this.name = name;
            this.hitPoint = hitPoint;
            this.physicalAttack = physicalAttack;
            this.physicalDefense = physicalDefense;
            this.magicalAttack = magicalAttack;
            this.magicalDefense = magicalDefense;
            this.speed = speed;
            this.skillIds = skillIds;
        }
    }
}
