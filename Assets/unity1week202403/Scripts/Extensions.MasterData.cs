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
            return AssetLoader.LoadAsync<ScriptableSequences>($"Assets/unity1week202403/Database/Skill.{self.Id}.asset");
        }
    }
}
