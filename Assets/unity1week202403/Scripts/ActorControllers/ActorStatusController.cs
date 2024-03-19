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

        public bool IsDead => HitPoint <= 0;
    }
}
