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

            var p = owner.StatusController.PhysicalAttack * power / 20;
            var d = target.StatusController.PhysicalDefense / 2;
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
