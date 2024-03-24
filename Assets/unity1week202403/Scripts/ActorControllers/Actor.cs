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
        private Define.ActorType actorType;

        public ActorStatusController StatusController { get; private set; }

        public Actor Spawn(Define.ActorType actorType, ActorStatus status)
        {
            var instance = Instantiate(this);
            instance.actorType = actorType;
            instance.StatusController = new ActorStatusController(status);
            return instance;
        }

        public async UniTask PerformActionAsync(Actor target, Container container, CancellationToken token)
        {
            var skillSpec = StatusController.GetCurrentSkillSpec();
            await container.Resolve<UIPresenterActorName>().BeginSkillNameAnimationAsync(actorType, skillSpec.Name, token);
            var sequences = (await skillSpec.LoadSkillSequencesAsync()).Sequences;
            var sequencer = new Sequencer(container, sequences);
            await sequencer.PlayAsync(token);
            StatusController.IncrementPerformedActionCount();
        }
    }
}
