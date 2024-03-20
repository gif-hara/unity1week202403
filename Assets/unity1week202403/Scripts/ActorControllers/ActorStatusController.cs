using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnitySequencerSystem;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActorStatusController
    {
        private readonly ActorStatus status;

        public ActorStatusController(ActorStatus status)
        {
            this.status = status;
        }

        public string Name => status.name;

        public int HitPoint => status.hitPoint;

        public int PhysicalAttack => status.physicalAttack;

        public int PhysicalDefense => status.physicalDefense;

        public int MagicalAttack => status.magicalAttack;

        public int MagicalDefense => status.magicalDefense;

        public int Speed => status.speed;

        /// <summary>
        /// 行動した回数
        /// </summary>
        public int PerformedActionCount { get; private set; }

        public bool IsDead => HitPoint <= 0;

        public void IncrementPerformedActionCount()
        {
            PerformedActionCount++;
        }

        public async UniTask<List<ISequence>> GetSkillSequence()
        {
            var index = status.skillIds.Count - 1 < PerformedActionCount ? status.skillIds.Count - 1 : PerformedActionCount;
            var skillSpec = TinyServiceLocator.Resolve<MasterData>().SkillSpecs.Get(status.skillIds[index]);
            return (await skillSpec.LoadSkillSequences()).Sequences;
        }
    }
}
