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

        public string Name => status.Name;

        public int HitPoint => status.HitPoint;

        public int PhysicalAttack => status.PhysicalAttack;

        public int PhysicalDefense => status.PhysicalDefense;

        public int MagicalAttack => status.MagicalAttack;

        public int MagicalDefense => status.MagicalDefense;

        public int Speed => status.Speed;
    }
}
