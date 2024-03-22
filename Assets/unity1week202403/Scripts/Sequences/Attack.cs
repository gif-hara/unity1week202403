using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnitySequencerSystem;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class Attack : ISequence
    {
        [SerializeField]
        private Define.AttackAttribute attackAttribute;

        [SerializeField]
        private int power;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var owner = container.Resolve<Actor>("OwnerActor");
            var target = container.Resolve<Actor>("TargetActor");
            var os = owner.StatusController;
            var ts = target.StatusController;

            var p = attackAttribute == Define.AttackAttribute.Physical ? os.PhysicalStrength : os.MagicalStrength;
            var buffType = attackAttribute == Define.AttackAttribute.Physical ? Define.BuffType.PhysicalAttack : Define.BuffType.MagicalAttack;
            p = Mathf.FloorToInt(p * os.GetBuffedValue(buffType));
            p *= power / 20;
            var d = attackAttribute == Define.AttackAttribute.Physical ? ts.PhysicalDefense : ts.MagicalDefense;
            buffType = attackAttribute == Define.AttackAttribute.Physical ? Define.BuffType.PhysicalDefense : Define.BuffType.MagicalDefense;
            d = Mathf.FloorToInt(d * ts.GetBuffedValue(buffType));
            d /= 2;
            var damage = p - d;
            if (damage < 1)
            {
                damage = 1;
            }
            target.StatusController.TakeDamage(damage);

            return UniTask.CompletedTask;
        }
    }
}
