using System;
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
        private ActorStatus status;

        public string Name => status.name;

        private ReactiveProperty<int> hitPoint = new();

        public int HitPoint => hitPoint.Value;

        public float HitPointRate => (float)HitPoint / status.hitPoint;

        public Observable<int> HitPointAsObservable() => hitPoint;

        private ReactiveProperty<int> physicalStrength = new();

        public int PhysicalStrength => physicalStrength.Value;

        public float PhysicalStrengthRate => (float)PhysicalStrength / Define.ParameterMax;

        public Observable<int> PhysicalStrengthAsObservable() => physicalStrength;

        private ReactiveProperty<int> physicalDefense = new();

        public int PhysicalDefense => physicalDefense.Value;

        public float PhysicalDefenseRate => (float)PhysicalDefense / Define.ParameterMax;

        public Observable<int> PhysicalDefenseAsObservable() => physicalDefense;

        private ReactiveProperty<int> magicalStrength = new();

        public int MagicalStrength => magicalStrength.Value;

        public float MagicalStrengthRate => (float)MagicalStrength / Define.ParameterMax;

        public Observable<int> MagicalStrengthAsObservable() => magicalStrength;

        private ReactiveProperty<int> magicalDefense = new();

        public int MagicalDefense => magicalDefense.Value;

        public float MagicalDefenseRate => (float)MagicalDefense / Define.ParameterMax;

        public Observable<int> MagicalDefenseAsObservable() => magicalDefense;

        private ReactiveProperty<int> speed = new();

        public int Speed => speed.Value;

        public float SpeedRate => (float)Speed / Define.ParameterMax;

        public Observable<int> SpeedAsObservable() => speed;

        /// <summary>
        /// 行動した回数
        /// </summary>
        public int PerformedActionCount { get; private set; }

        public bool IsDead => HitPoint <= 0;

        public Dictionary<Define.BuffType, int> Buffs { get; } = new();

        private readonly Subject<int> takedDamageSubject = new();

        public Observable<int> TakedDamageAsObservable() => takedDamageSubject;

        public ActorStatusController(ActorStatus status)
        {
            this.status = status;
            hitPoint.Value = status.hitPoint;
            physicalStrength.Value = status.physicalStrength;
            physicalDefense.Value = status.physicalDefense;
            magicalStrength.Value = status.magicalStrength;
            magicalDefense.Value = status.magicalDefense;
            speed.Value = status.speed;
            Buffs[Define.BuffType.PhysicalAttack] = 0;
            Buffs[Define.BuffType.PhysicalDefense] = 0;
            Buffs[Define.BuffType.MagicalAttack] = 0;
            Buffs[Define.BuffType.MagicalDefense] = 0;
            Buffs[Define.BuffType.Speed] = 0;
        }

        public void IncrementPerformedActionCount()
        {
            PerformedActionCount++;
        }

        public async UniTask<List<ISequence>> GetSkillSequence()
        {
            var index = PerformedActionCount % status.skillIds.Count;
            var skillSpec = TinyServiceLocator.Resolve<MasterData>().SkillSpecs.Get(status.skillIds[index]);
            return (await skillSpec.LoadSkillSequences()).Sequences;
        }

        public void TakeDamage(int damage)
        {
            hitPoint.Value -= damage;
            takedDamageSubject.OnNext(damage);
            Debug.Log($"{Name}は{damage}のダメージを受けた");
        }

        public void AddBuff(Define.BuffType type, int value)
        {
            Buffs[type] = Mathf.Clamp(Buffs[type] + value, -4, 4);
            Debug.Log($"{Name}の{type}が{Buffs[type]}になった");
        }

        public float GetBuffedValue(Define.BuffType type)
        {
            return 1.0f + Buffs[type] * 0.5f;
        }

        internal void Recovery(int value)
        {
            hitPoint.Value = Mathf.Clamp(hitPoint.Value + value, 0, status.hitPoint);
            Debug.Log($"{Name}は{value}回復した");
        }

        public void Reset(ActorStatus status)
        {
            this.status = status;
            ResetAll();
        }

        public void ResetAll()
        {
            hitPoint.Value = status.hitPoint;
            physicalStrength.Value = status.physicalStrength;
            physicalDefense.Value = status.physicalDefense;
            magicalStrength.Value = status.magicalStrength;
            magicalDefense.Value = status.magicalDefense;
            speed.Value = status.speed;
            PerformedActionCount = 0;
            Buffs[Define.BuffType.PhysicalAttack] = 0;
            Buffs[Define.BuffType.PhysicalDefense] = 0;
            Buffs[Define.BuffType.MagicalAttack] = 0;
            Buffs[Define.BuffType.MagicalDefense] = 0;
            Buffs[Define.BuffType.Speed] = 0;
        }
    }
}
