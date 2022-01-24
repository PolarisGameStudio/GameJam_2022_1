using System;
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


    public static int FOLLOWER_MAX_LEVEL = (int) SYSTEM_VALUE.GetEntity("FOLLWER_MAX_LEVEL").Value;

    public static int RUNE_DAILY_LIMIT = 3;
    public static int RUNE_DURATUIN = 1800;

    public static float SKILL_DISTANCE_BLOCK_SIZE = 1f;
    public static int SKILL_MAX_SLOT_COUNT = 6;
    public static int SKILL_MAX_LEVEL = 10;

    public static int GACHA_EQUIPMENT_SMALL_PRICE = 1000;
    public static int GACHA_EQUIPMENT_BIG_PRICE = 3000;

    public static int GACHA_RING_SMALL_PRICE = 1000;
    public static int GACHA_RING_BIG_PRICE = 3000;

    public static int GACHA_SKILL_SMALL_PRICE = 1000;
    public static int GACHA_SKILL_BIG_PRICE = 3000;

    public static int MINIMUM_SAVE_PERIOD = 1;
    public static float BOSS_DUNGEON_LIMIT_TIME = 10f;
    public static float TREASURE_DUNGEON_LIMIT_TIME = 10f;
    
    public static int DUNGEON_DAILY_TICKET_AMOUNT = 3;
    
    
    public static int STAT_RESET_PRICE = 2000;
}

public static class ColorValue
{
    public static Color DISABLE_TEXT_COLOR => new Color(224f / 256, 64f / 256, 56f / 256);
    public static Color ENABLE_TEXT_COLOR => new Color(1, 1, 1);
    public static Color GRADE_GREEN_COLOR => new Color(74f / 256, 233f / 256, 24f / 256);
    public static Color GRADE_BLUE_COLOR => new Color(38f / 256, 129f / 256, 254f / 256);
    public static Color GRADE_PURPLE_COLOR => new Color(184f / 256, 17f / 256, 255f / 256);
    public static Color GRADE_RED_COLOR => new Color(234f / 256, 51f / 256, 24f / 256);
    public static Color GRADE_GOLD_COLOR => new Color(255f / 256, 213f / 256, 39f / 256);
    public static Color GRADE_SKY_COLOR => new Color(41f / 256, 243f / 256, 255f / 256);
    public static Color GRADE_PINK_COLOR => new Color(255f / 256, 39f / 256, 177f / 256);

    public static Color GetColorByGrade(Enum_ItemGrade grade)
    {
        switch (grade)
        {
            case Enum_ItemGrade.Common:
                return GRADE_GREEN_COLOR;
            case Enum_ItemGrade.Uncommon:
                return GRADE_BLUE_COLOR;

            case Enum_ItemGrade.Rare:
                return GRADE_PURPLE_COLOR;
            case Enum_ItemGrade.Epic:
                return GRADE_RED_COLOR;
            case Enum_ItemGrade.Legendary:
                return GRADE_GOLD_COLOR;
            case Enum_ItemGrade.Myth:
                return GRADE_SKY_COLOR;
        }

        return GRADE_GREEN_COLOR;
    }
}

public static class StringValue
{
    public static string GetGradeName(Enum_ItemGrade grade)
    { 
        switch (grade)
        {
            case Enum_ItemGrade.Common:
                return "흔한";
            case Enum_ItemGrade.Uncommon:
                return "일반";

            case Enum_ItemGrade.Rare:
                return "희귀";
            case Enum_ItemGrade.Epic:
                return "영웅";
            case Enum_ItemGrade.Legendary:
                return "전설";
            case Enum_ItemGrade.Myth:
                return "신화";

            default:
                return "";
        }
    }
    
    public static string GetStatName(Enum_StatType stat)
    {
        switch (stat)
        {
            case Enum_StatType.Damage:
                return "공격력";
            case Enum_StatType.MaxHealth:
                return "최대 체력";
            case Enum_StatType.CriticalChance:
                return "치명타 확률";
            case Enum_StatType.CriticalDamage:
                return "치명타 공격력";
            case Enum_StatType.SuperCriticalChance:
                return "회심의 일격 확률";
            case Enum_StatType.SuperCriticalDamage:
                return "회심의 일격 공격력";
            case Enum_StatType.AttackSpeed:
                return "공격 속도";
            case Enum_StatType.MoveSpeed:
                return "이동 속도";
            case Enum_StatType.MoreGold:
                return "골드 추가 획득";
            case Enum_StatType.MoreExp:
                return "경험치 추가 획득";
            case Enum_StatType.Accuracy:
                return "명중";
            case Enum_StatType.Evasion:
                return "회피";
            case Enum_StatType.HealthRecovery:
                return "체력 회복";
            case Enum_StatType.ReduceCoolTime:
                return "재사용 대기시간 감소";
        }
        
        return "";
    }
}