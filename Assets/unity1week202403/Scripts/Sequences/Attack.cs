using System.Threading;
using Cysharp.Threading.Tasks;
using UnitySequencerSystem;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Attack : ISequence
    {
        public UniTask PlayAsync(Container container, CancellationToken cancellationToken)
        {
            var owner = container.Resolve<Actor>("OwnerActor");
            var target = container.Resolve<Actor>("TargetActor");

            return UniTask.CompletedTask;
        }
    }
}
