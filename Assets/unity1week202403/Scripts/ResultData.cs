namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResultData
    {
        public int BattleCount { get; }

        public string Word { get; }

        public ResultData(int battleCount, string word)
        {
            BattleCount = battleCount;
            Word = word;
        }
    }
}
