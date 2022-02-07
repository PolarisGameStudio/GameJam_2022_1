using System;
using System.Collections;
using System.Text;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Time;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;

public enum SERVER_CODE
{
    Success = 0,
    LoginBan     = 9998,
    LoginOverlap = 9999,
}

public class ServerManager : SingletonBehaviour<ServerManager>
{
    private string m_CA_UID = string.Empty;
    public string CA_UID => m_CA_UID;

    private string m_AUTH_TOKEN = string.Empty;
    public string AUTH_TOKEN => m_AUTH_TOKEN;

    private string m_NickName = string.Empty;
    public string NickName => m_NickName;

    private bool m_IsPlatform = false;
    public bool IsPlatform => m_IsPlatform;
    
    public int MIN_VERSION { get; private set; }
    public int MAX_VERSION { get; private set; }

    private Coroutine m_WaitForConnectionCoroutine;

    private static ObscuredFloat m_RefreshTime = 0;
    
    private static long m_ServerUnixTime = 0;
    public static long ServerUnixTime => m_ServerUnixTime;
    public static long ServerUnixTimeInterpolation => m_ServerUnixTime + (int)(SpeedHackProofTime.realtimeSinceStartup - m_RefreshTime);
    
    private static DateTime m_ServerDateTimeNow = new DateTime(1970, 1, 1, 0, 0, 0);
    public static DateTime ServerDateTimeNow => m_ServerDateTimeNow;

    private static DateTime m_ServerDateTimeToday = new DateTime(1970, 1, 1, 0, 0, 0);
    public static DateTime ServerDateTimeToday => m_ServerDateTimeToday;
    
    
    private bool m_LoginOverlapped = false;
    private bool m_Cheated = false;

    public async UniTaskVoid SendCreative(int score)
    {
        var form = new WWWForm();
        form.AddField("round",11);
        form.AddField("package",Application.identifier);
        form.AddField("device_id",SystemInfo.deviceUniqueIdentifier);
        form.AddField("score",score);
        
        var recv = UnityWebRequest.Post("https://appevent.cookappsgames.com/api/vote.php", form);

        await recv.SendWebRequest();

        switch (recv.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(recv.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(recv.error);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    
//     private void Start()
//     {
//         SpeedHackDetector.StartDetection(OnSpeedHackDetected);
//     }
//     
//     public async UniTask<ResponseLoginPacket> Login(PlatformLoginInfo platformLoginInfo = null)
//     {
// #if UNITY_ANDROID
//         var recv = await NetManager.Post<ResponseLoginPacket>(new RequestLoginPacket(platformLoginInfo, "", PackageSignature.apkHashKey));
// #else
//         var recv = await NetManager.Post<ResponseLoginPacket>(new RequestLoginPacket(platformLoginInfo));
// #endif
//         
//         if (recv.result)
//         {
//             if (m_PlatformLoginInfo != null && m_PlatformLoginInfo.PlatformLoginTypeType == PlatformLoginType.guest && recv.is_device > 0)
//             {
//                 OnAccountAlreadyLinking();
//             }
//             
//             if (platformLoginInfo != null)
//             {
//                 m_PlatformLoginInfo = platformLoginInfo;
//             }
//
//             
//             
//             if (recv.code == (int) SERVER_CODE.LoginBan)
//             {
//                 OnLoginBan();
//             }
//             
//             
//             m_CA_UID     = recv.ca_uid;
//             m_AUTH_TOKEN = recv.authToken;
//             m_NickName = string.Empty;
//
//             MIN_VERSION = recv.version.min_version;
//             MAX_VERSION = recv.version.max_version;
//             
//             int version = Convert.ToInt32(Application.version.Replace(".", string.Empty));
//             if (version < MIN_VERSION)
//             {
//                 string title = LocalizeText.GetText("UI_PopUpMessage_Update_Title");
//                 string decript = LocalizeText.GetText("UI_PopUpMessage_Update_Description");
//                 UI_Popup.Instance.OpenOK(title, decript, () =>
//                 {
//             
//                     Application.OpenURL("https://play.google.com/store/apps/details?id=com.cookapps.ShadowKnightsIdleRPG");
//                 });
//             }
//
//            
//
//             m_IsPlatform = recv.is_platform > 0;
//             
//             if (recv.is_nickname > 0)
//             {
//                 m_NickName = recv.nickname;
//             
//                 IntroScene.HaveNickname = true;
//                 
//                 FirebaseLogManager.Instance.SetCrashlyticsLoginInfo();
//             }
//
//             SetServerTime(recv.server_time);
//         }
//         else
//         {
//             if (recv.code == (int) SERVER_CODE.LoginBan)
//             {
//                 OnLoginBan();
//             }
//         }
//
//         return recv;
//     }
//
//     public async UniTask<ResponseNicknamePacket> ChangeNickname(string nickname, Action<int> onFailedAction)
//     {
//         var recv = await NetManager.Post<ResponseNicknamePacket>(new RequestNicknamePacket(nickname));
//
//         if (recv.result && recv.is_nickname > 0)
//         {
//             m_NickName = recv.nickname;
//
//             IntroScene.HaveNickname = true; 
//         }
//         else
//         {
//             onFailedAction?.Invoke(recv.code);
//         }
//
//         return recv;
//     }
//     
//     public async UniTask<bool> Save(params NetSaveData[] datas)
//     {
//         if (m_LoginOverlapped || m_Cheated)
//         {
//             return false;
//         }
//         
//         var recv = await NetManager.Post<ResponseSavePacket>(new RequestSavePacket(datas));
//
//         SetServerTime(recv.server_time);
//
//         if (recv.code == (int) SERVER_CODE.LoginOverlap)
//         {
//             OnLoginOverlapped();
//         }
//
//         if (recv.code == (int) SERVER_CODE.LoginBan)
//         {
//             OnLoginBan();
//         }
//         
//         return recv.result;
//     }
//
//     public void OnLoginOverlapped()
//     {
//         m_LoginOverlapped = true;
//
//         UI_Popup.Instance.OpenOKAll(LocalizeText.GetText("UI_Popup_Notification"),  LocalizeText.GetText("UI_Popup_DuplicateLogin"), () =>
//         {
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
//         });
//     }
//
//     public void OnLoginBan()
//     {
//         m_LoginOverlapped = true;
//         
//         UI_Popup.Instance.OpenOKAll(LocalizeText.GetText("UI_Popup_Notification"), LocalizeText.GetText("UI_PopUp_Server_Code_9998"), () =>
//         {
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
//         });
//     }
//
//     public void OnAccountAlreadyLinking()
//     {
//         m_LoginOverlapped = true;
//         
//         UI_Popup.Instance.OpenOKAll(LocalizeText.GetText("UI_Popup_Notification"),LocalizeText.GetText("UI_Popup_AlreadyRegistered"), () =>
//         {
//             PlatformLoginManager.Instance.LogoutAll();
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
//         });
//     }
//     
//     private void OnSpeedHackDetected()
//     {
//         m_Cheated = true;
//         
//         UI_Popup.Instance.OpenOKAll(LocalizeText.GetText("UI_Popup_Notification"),LocalizeText.GetText("UI_Popup_Hacked"), () =>
//         {
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//                 Application.Quit();
// #endif
//         });
//     }
//     
//     public async UniTask<ResponseLoadPacket> Load(string category)
//     {
//         var recv = await NetManager.Post<ResponseLoadPacket>(new RequestLoadPacket(category));
//
//         if (recv.isInternalError)
//         {
//             return new ResponseLoadPacket()
//             {
//                 datas = new NetLoadDatas(),
//                 isInternalError = true,
//             };
//         }
//         
//         
//         if (!recv.result)
//         {
//             return new ResponseLoadPacket()
//             {
//                 datas = new NetLoadDatas()
//             };
//         }
//         
//         return recv;
//     }
//
//     public void ConnectionCheck()
//     {
//         if (InternetReachabilityVerifier.Instance.status != InternetReachabilityVerifier.Status.NetVerified)
//         {
//             if (GameManager.Instance.IsMainScene)
//             {
//                 if (m_WaitForConnectionCoroutine != null)
//                 {
//                     StopCoroutine(m_WaitForConnectionCoroutine);
//                     m_WaitForConnectionCoroutine = null;
//                 }
//
//                 m_WaitForConnectionCoroutine = StartCoroutine(WaitForConnection());
//             }
//             
//             if (UIManager.Instance.Peek != UIType.Popup)
//             {
//                 string title = LocalizeText.GetText("UI_Popup_Title");
//                 string decript = LocalizeText.GetText("UI_PopUpMessage_InternetConnectionFail");
//                 UI_Popup.Instance.OpenOK(title, decript);
//             }
//         }
//     }
//    
//     private IEnumerator WaitForConnection()
//     {
//         yield return new WaitForEndOfFrame();
//         yield return StartCoroutine(InternetReachabilityVerifier.Instance.waitForNetVerifiedStatus());
//
//         if (m_LoginOverlapped || m_Cheated)
//         {
//             yield break;
//         }
//         
//         SaveManager.Instance.ForceSave();
//     }
//
//
//     private void SetServerTime(long unixTime)
//     {
//         if (unixTime <= 0)
//         {
//             return;
//         }
//
//         if (unixTime == m_ServerUnixTime)
//         {
//             return;
//         }
//
//         m_RefreshTime = SpeedHackProofTime.realtimeSinceStartup;
//
//         m_ServerUnixTime = unixTime;
//         m_ServerDateTimeNow   = UtilCode.UnixTimeToDateTime(m_ServerUnixTime);
//         m_ServerDateTimeToday = new DateTime(m_ServerDateTimeNow.Year, m_ServerDateTimeNow.Month, m_ServerDateTimeNow.Day);
//     }
 
}
