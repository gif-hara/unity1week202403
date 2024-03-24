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
            word = word.ToUpper();
            var masterData = TinyServiceLocator.Resolve<MasterData>();
            var scores = masterData.WordSpecs.List
                .Select(x => (x.Word, GetScore(word, x.Word)));
            var max = scores.Max(x => x.Item2);
            var maxScores = scores
                .Where(x => x.Item2 == max)
                .ToList();
            return maxScores[UnityEngine.Random.Range(0, maxScores.Count)].Word.ToUpper();
        }

        private static int GetScore(string selectWord, string targetWord)
        {
            targetWord = targetWord.ToUpper();
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

        public static ActorStatus ToActorStatus(string word)
        {
            var masterData = TinyServiceLocator.Resolve<MasterData>();
            var result = new ActorStatus();
            var index = 0;
            var hitPointRate = new[] { 50, 58, 67, 77, 88, 100 };
            var otherRate = new[] { 5, 7, 10, 14, 19, 25 };
            foreach (var c in word)
            {
                var spec = masterData.CharacterSpecs.Get(c.ToString());
                result.name = word;
                result.hitPoint += hitPointRate[spec.HitPoint - 1];
                result.physicalStrength += otherRate[spec.PhysicalStrength - 1];
                result.physicalDefense += otherRate[spec.PhysicalDefense - 1];
                result.magicalStrength += otherRate[spec.MagicalStrength - 1];
                result.magicalDefense += otherRate[spec.MagicalDefense - 1];
                result.speed += otherRate[spec.Speed - 1];
                result.skillIds.Add(spec.GetSkill(index));
                index++;
            }

            return result;
        }
    }
}