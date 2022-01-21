using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class IRequestPacket
{
    [JsonIgnore]
    public string url { get; }

    // public string auth_id { get; private set; }             // [Required] 인증 플랫폼의 고유 ID   (guest -> "")    
    //
    // public string auth_platform { get; private set; }       // [Required] 인증 플랫폼:  'guest', 'google', 'facebook', 'ios' or 'apple'
    //
    public string ca_uid { get; private set; }
    public string device_id { get; private set; }
    public int version { get; private set; }
    public string device { get; private set; }
    
    
    protected IRequestPacket(string url)
    {
        this.url = url;

        // ca_uid = ServerManager.Instance.CA_UID;
        //
        device_id = SystemInfo.deviceUniqueIdentifier;
        
        #if UNITY_EDITOR
        device_id = "Creative_Test";
        #endif
        
        // version = Convert.ToInt32(Application.version.Replace(".", string.Empty));
        //
        // device = NetDefine.PLATFORM;
    }
}

public class ResponsePacket
{
    [JsonProperty] public int result { get; private set; }
    [JsonProperty] public string msg { get; private set; } // 게임잼떄 추가
    [JsonProperty] public bool isInternalError { get; set; }
}

public class DataError
{
    [JsonProperty]
    public int code { get; private set; }
    
    [JsonProperty]
    public string message { get; private set; }
}

public class PacketErrorJson
{
    [JsonProperty]
    public bool result { get; private set; }
    [JsonProperty]
    public bool isInternalError { get; private set; }

    public static string CreateJson()
    {
        var inst = new PacketErrorJson()
        {
            result = false,
            isInternalError = true,
        };

        var json = JsonConvert.SerializeObject(inst);
        return json;
    }
}