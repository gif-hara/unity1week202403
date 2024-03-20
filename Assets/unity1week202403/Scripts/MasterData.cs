using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "MasterData", menuName = "unity1week202403/MasterData")]
    public sealed class MasterData : ScriptableObject
    {
        [SerializeField]
        private CharacterSpec.DictionaryList characterSpecs;

        public CharacterSpec.DictionaryList CharacterSpecs => characterSpecs;

        [SerializeField]
        private WordSpec.DictionaryList wordSpecs;

        public WordSpec.DictionaryList WordSpecs => wordSpecs;

#if UNITY_EDITOR
        [ContextMenu("Update")]
        public async void Test()
        {
            Debug.Log("Begin MasterData Update");
            var database = await UniTask.WhenAll(
                GoogleSpreadSheetDownloader.DownloadAsync("CharacterSpec"),
                GoogleSpreadSheetDownloader.DownloadAsync("WordSpec")
            );

            characterSpecs.Set(JsonHelper.FromJson<CharacterSpec>(database.Item1));
            wordSpecs.Set(JsonHelper.FromJson<WordSpec>(database.Item2));

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("End MasterData Update");
        }
#endif

        [Serializable]
        public class CharacterSpec
        {
            public string Character;

            public int HitPoint;

            public int PhysicalAttack;

            public int PhysicalDefense;

            public int MagicalAttack;

            public int MagicalDefense;

            public int Speed;

            public int Skill1;

            public int Skill2;

            public int Skill3;

            [Serializable]
            public sealed class DictionaryList : DictionaryList<string, CharacterSpec>
            {
                public DictionaryList() : base(x => x.Character) { }
            }
        }

        [Serializable]
        public sealed class WordSpec
        {
            public string Word;

            public int Number;

            [Serializable]
            public sealed class DictionaryList : DictionaryList<string, WordSpec>
            {
                public DictionaryList() : base(x => x.Word) { }
            }
        }
    }
}
