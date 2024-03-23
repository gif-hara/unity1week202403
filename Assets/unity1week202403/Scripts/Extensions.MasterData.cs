using Cysharp.Threading.Tasks;
using UnitySequencerSystem;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        public static UniTask<ScriptableSequences> LoadSkillSequences(this MasterData.SkillSpec self)
        {
            return AssetLoader.LoadAsync<ScriptableSequences>($"Skill.{self.Id}");
        }

        public static int GetSkill(this MasterData.CharacterSpec self, int index)
        {
            switch (index)
            {
                case 0:
                    return self.Skill1;
                case 1:
                    return self.Skill2;
                default:
                    return self.Skill3;
            }
        }
    }
}
