using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Networking;

public static class NetManager
{
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
    {
        NullValueHandling      = NullValueHandling.Ignore,
        ContractResolver       = new NetJsonContractResolver(),
        ObjectCreationHandling = ObjectCreationHandling.Replace
    };

    public static IObservable<T> Post<T>(IRequestPacket requestPacket) where T : ResponsePacket
    {
        return AsyncPost<T>(requestPacket).ToObservable().PublishLast().RefCount();
    }

    public static void PostForget(IRequestPacket requestPacket)
    {
        AsyncPost<ResponsePacket>(requestPacket).Forget();
    }

    public static void PostForget(string url, string json)
    {
        AsyncPost<ResponsePacket>(url, json).Forget();
    }
    
    public static async UniTask<T> AsyncPost<T>(string url, string json) where T : ResponsePacket
    {
        try
        {
            var fullURL = NetDefine.NET_SERVER_ADDR + url;

            using var req = UnityWebRequest.Put(fullURL, Encoding.UTF8.GetBytes(json));
            req.method = UnityWebRequest.kHttpVerbPOST;
            req.useHttpContinue = false;
            req.timeout = NetDefine.NET_TIMEOUT;
            req.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(ServerManager.Instance.AUTH_TOKEN))
            {
                req.SetRequestHeader("Authorization", $"Bearer {ServerManager.Instance.AUTH_TOKEN}");
            }

            Debug.Log($"[Network|Send|{url}]\n{json}");

            await req.SendWebRequest();

            /**if (url == "/data/load")
            {
                Debug.LogError($"[Network|Recv|{url}] Error \n {req.error}");
                return CreateError();
            }**/
            
            if (req.result == UnityWebRequest.Result.Success)
            {
                var text = req.downloadHandler.text;
                Debug.Log($"[Network|Recv|{url}]\n{text}");
                var obj = JsonConvert.DeserializeObject<T>(text, JsonSettings);
                return obj;
            }
            else
            {
                //ServerManager.Instance.ConnectionCheck();

                Debug.LogError($"[Network|Recv|{url}] Error \n {req.error}");
                return CreateError();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Network|Recv|{url}] Error \n {e.Message}");
            
          //  ServerManager.Instance.ConnectionCheck();
            
            return CreateError();
        }

        static T CreateError()
        {
            var json = PacketErrorJson.CreateJson();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    private static async UniTask<T> AsyncPost<T>(IRequestPacket requestPacket) where T : ResponsePacket
    {
        var json = JsonConvert.SerializeObject(requestPacket);
        var url = requestPacket.url;

        return await AsyncPost<T>(url, json);
    }
}
