using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemValue
{
#if UNITY_AWS
    public const string DeviceType = "aws";
#elif UNITY_ANDROID
    public const string DeviceType = "android";
#else
    public const string DeviceType = "ios";
#endif
    public static Color DISABLE_TEXT_COLOR => new Color(224f / 256, 64f / 256, 56f / 256);
    public static Color ENABLE_TEXT_COLOR => new Color(1, 1, 1);


    public static int FOLLOWER_MAX_LEVEL  = (int) SYSTEM_VALUE.GetEntity("FOLLOWER_MAX_LEVEL").Value;
}
