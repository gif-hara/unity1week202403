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
    public sealed class Recovery : ISequence
    {
        [SerializeField]
        private Define.TargetType targetType;

        [SerializeField]
        private int addValue;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            switch (targetType)
            {
                case Define.TargetType.Self:
                    var owner = container.Resolve<Actor>("OwnerActor");
                    owner.StatusController.Recovery(addValue);
                    break;
                case Define.TargetType.Opponent:
                    var target = container.Resolve<Actor>("TargetActor");
                    target.StatusController.Recovery(addValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return UniTask.CompletedTask;
        }
    }
}
