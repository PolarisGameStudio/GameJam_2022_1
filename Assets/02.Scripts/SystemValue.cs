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


    public static int FOLLOWER_MAX_LEVEL  = (int) SYSTEM_VALUE.GetEntity("FOLLWER_MAX_LEVEL").Value;

    public static int RUNE_DAILY_LIMIT = 3;
    public static int RUNE_DURATUIN = 360 * 1;

    public static int SKILL_DISTANCE_BLOCK_SIZE = 1;
    public static int SKILL_MAX_SLOT_COUNT = 6;
}
