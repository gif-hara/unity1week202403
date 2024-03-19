using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace unity1week202403
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GoogleSpreadSheetDownloader
    {
        const string url = "https://script.google.com/macros/s/AKfycbw86nk5EUsbKJAjhJAmlLA7slJCxhsKlgt74pBo3wxDD_Ce3SZ6a_5Ly7ve7Yi2xsMM/exec";

        public static async UniTask<string> DownloadAsync(string sheetName)
        {
            var request = UnityWebRequest.Get(url + "?sheetName=" + sheetName);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                // エラー処理
                UnityEngine.Debug.LogError(request.error);
                return null;
            }
            else
            {
                return request.downloadHandler.text;
            }
        }
    }
}
