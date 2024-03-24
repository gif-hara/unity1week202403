namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public static class Define
    {
        public enum ActorType
        {
            Player,
            Enemy,
        }

        public enum AttackAttribute
        {
            Physical,
            Magical,
        }

        public enum BuffType
        {
            PhysicalAttack,
            PhysicalDefense,
            MagicalAttack,
            MagicalDefense,
            Speed,
        }

        public enum TargetType
        {
            Self,
            Opponent,
        }

        public const float ParameterMax = 100;
    }
}
