using System;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ResultData
    {
        [field: SerializeField]
        public int BattleCount { get; private set; }

        [field: SerializeField]
        public string Word { get; private set; }

        public ResultData(int battleCount, string word)
        {
            BattleCount = battleCount;
            Word = word;
        }
    }
}
