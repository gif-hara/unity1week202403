using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using TweetWithScreenShot;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UIPresenterResult
    {
        public async UniTaskVoid BeginAsync(
            HKUIDocument documentPrefab,
            ResultData resultData,
            AudioClip se1,
            AudioClip se2,
            AudioClip bgm,
            CancellationToken token
            )
        {
            var scope = CancellationTokenSource.CreateLinkedTokenSource(token);
            var document = UnityEngine.Object.Instantiate(documentPrefab);

            document.Q<TMP_Text>("Word").text = resultData.Word;
            var battleCountValueText = document.Q<TMP_Text>("BattleCount.Value");
            var buttonAnimationArea = document.Q<RectTransform>("ButtonAnimationArea");
            buttonAnimationArea.localPosition = new Vector3(0.0f, -200.0f, 0.0f);
            document.Q<Button>("GotoTitleButton").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SceneManager.LoadScene("Title");
                })
                .RegisterTo(scope.Token);
            document.Q<Button>("TweetButton").OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var text = $"{resultData.Word}で{resultData.BattleCount}回戦いました！ https://unityroom.com/games/word-battle";
                    var hashTags = new string[] { "unity1week", "unityroom", "wordBattle" };
                    TweetManager.Tweet(text, hashTags);
                })
                .RegisterTo(scope.Token);
            TinyServiceLocator.Resolve<AudioController>().PlayOneShot(se1);

            await LMotion.Create(0, resultData.BattleCount, 1.5f)
                .WithEase(Ease.Linear)
                .BindToText(battleCountValueText)
                .ToUniTask(token);
            TinyServiceLocator.Resolve<AudioController>().PlayOneShot(se2);
            await LMotion.Create(Vector3.one, Vector3.one * 2.0f, 0.5f)
                .WithEase(Ease.OutCirc)
                .BindToLocalScale(battleCountValueText.rectTransform)
                .ToUniTask(token);
            await LMotion.Create(Vector3.one * 2.0f, Vector3.one, 0.5f)
                .WithEase(Ease.OutBounce)
                .BindToLocalScale(battleCountValueText.rectTransform)
                .ToUniTask(token);
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
            TinyServiceLocator.Resolve<AudioController>().PlayBgm(bgm);
            await LMotion.Create(-200.0f, 0.0f, 0.5f)
                .WithEase(Ease.OutCirc)
                .BindToLocalPositionY(buttonAnimationArea)
                .ToUniTask(token);

            await UniTask.WaitUntilCanceled(scope.Token);

            if (document != null && document.gameObject != null)
            {
                UnityEngine.Object.Destroy(document.gameObject);
            }
        }
    }
}
