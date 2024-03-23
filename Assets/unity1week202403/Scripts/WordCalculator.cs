using System;
using System.Linq;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public static class WordCalculator
    {
        public static string Calculate(string word)
        {
            var masterData = TinyServiceLocator.Resolve<MasterData>();
            var scores = masterData.WordSpecs.List
                .Select(x => (x.Word, GetScore(word, x.Word)))
                .OrderByDescending(x => x.Item2);
            var max = scores.Max(x => x.Item2);
            var maxScores = scores
                .Where(x => x.Item2 == max)
                .ToList();
            return maxScores[UnityEngine.Random.Range(0, maxScores.Count)].Word;
        }

        private static int GetScore(string selectWord, string targetWord)
        {
            var score = 0;
            foreach (var c in selectWord)
            {
                if (targetWord.Contains(c))
                {
                    score++;
                }
            }
            return score;
        }
    }
}
