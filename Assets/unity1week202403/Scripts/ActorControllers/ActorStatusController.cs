using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
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
            hitPoint.Value = status.hitPoint;
            physicalAttack.Value = status.physicalAttack;
            physicalDefense.Value = status.physicalDefense;
            magicalAttack.Value = status.magicalAttack;
            magicalDefense.Value = status.magicalDefense;
            speed.Value = status.speed;
        }

        public string Name => status.name;

        private ReactiveProperty<int> hitPoint = new();

        public int HitPoint => hitPoint.Value;

        public Observable<int> HitPointAsObservable() => hitPoint;

        private ReactiveProperty<int> physicalAttack = new();

        public int PhysicalAttack => physicalAttack.Value;

        public Observable<int> PhysicalAttackAsObservable() => physicalAttack;

        private ReactiveProperty<int> physicalDefense = new();

        public int PhysicalDefense => physicalDefense.Value;

        public Observable<int> PhysicalDefenseAsObservable() => physicalDefense;

        private ReactiveProperty<int> magicalAttack = new();

        public int MagicalAttack => magicalAttack.Value;

        public Observable<int> MagicalAttackAsObservable() => magicalAttack;

        private ReactiveProperty<int> magicalDefense = new();

        public int MagicalDefense => magicalDefense.Value;

        public Observable<int> MagicalDefenseAsObservable() => magicalDefense;

        private ReactiveProperty<int> speed = new();

        public int Speed => speed.Value;

        public Observable<int> SpeedAsObservable() => speed;

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

        public void TakeDamage(int damage)
        {
            hitPoint.Value -= damage;
            Debug.Log($"{Name}は{damage}のダメージを受けた");
        }
    }
}
