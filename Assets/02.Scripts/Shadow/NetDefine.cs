using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetDefine
{
#if UNITY_EDITOR || __DEV	
        public const string NET_SERVER_ADDR = "https://appevent.cookappsgames.com";
        //public const string NET_SERVER_ADDR = "https://shadownights.cookappsgames.com";

#else
        public const string NET_SERVER_ADDR = "https://shadownights.cookappsgames.com";
#endif
        
    public const int NET_TIMEOUT = 20;
        
#if UNITY_AWS
        public const string PLATFORM = "aws";
#elif UNITY_ANDROID
    public const string PLATFORM = "android";
#else
        public const string PLATFORM = "ios";
#endif
}
