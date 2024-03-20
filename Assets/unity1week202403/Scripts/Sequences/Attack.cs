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
        private int power;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var owner = container.Resolve<Actor>("OwnerActor");
            var target = container.Resolve<Actor>("TargetActor");
            var os = owner.StatusController;
            var ts = target.StatusController;

            var p = Mathf.FloorToInt(os.PhysicalAttack * os.GetBuffedValue(Define.BuffType.PhysicalAttack));
            p *= power / 20;
            var d = Mathf.FloorToInt(ts.PhysicalDefense * ts.GetBuffedValue(Define.BuffType.PhysicalDefense));
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
