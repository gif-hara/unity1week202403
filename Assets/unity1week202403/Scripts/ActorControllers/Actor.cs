using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnitySequencerSystem;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Actor : MonoBehaviour
    {
        public ActorStatusController StatusController { get; private set; }

        public Actor Spawn(ActorStatus status)
        {
            var instance = Instantiate(this);
            instance.StatusController = new ActorStatusController(status);
            return instance;
        }

        public async UniTask PerformActionAsync(Actor target, CancellationToken token)
        {
            var container = new Container();
            container.Register("OwnerActor", this);
            container.Register("TargetActor", target);
            var sequences = await StatusController.GetSkillSequence();
            var sequencer = new Sequencer(container, sequences);
            await sequencer.PlayAsync(token);
            StatusController.IncrementPerformedActionCount();
        }
    }
}
