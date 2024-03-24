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
    public sealed class AddBuff : ISequence
    {
        [SerializeField]
        private Define.TargetType targetType;

        [SerializeField]
        private Define.BuffType buffType;

        [SerializeField]
        private int addValue;

        [SerializeField]
        private AudioClip se;

        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            TinyServiceLocator.Resolve<AudioController>().PlayOneShot(se);
            switch (targetType)
            {
                case Define.TargetType.Self:
                    var owner = container.Resolve<Actor>("OwnerActor");
                    owner.StatusController.AddBuff(buffType, addValue);
                    break;
                case Define.TargetType.Opponent:
                    var target = container.Resolve<Actor>("TargetActor");
                    target.StatusController.AddBuff(buffType, addValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return UniTask.CompletedTask;
        }
    }
}
