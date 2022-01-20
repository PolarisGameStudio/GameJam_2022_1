using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;


public class UtilCode
{
    public static GameObject GetPrefab(string path)
    {
        GameObject prefab = Resources.Load(path) as GameObject;

        if (!prefab)
        {
            Debug.LogError(string.Format("Resources/Prefabs: ({0}) is null", path));
        }
        
        return prefab;
    }

    public static TextAsset GetTextAsset(string path)
    {
        TextAsset textAsset = Resources.Load(path) as TextAsset;

        if (!textAsset)
        {
            Debug.LogError(string.Format("Resources/TextAssets: ({0}) is null", path));
        }
        
        return textAsset;
    }

    public static AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = Resources.Load(path) as AudioClip;

        if (!audioClip)
        {
            Debug.LogError( string.Format("Resources/Sounds: ({0}) is null", path));
        }

        return audioClip;
    }
    
    public static string TimeToStringFormat(int second)
    {
        int minute = second / 60;
        second %= 60;
        
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    public static string MinutesToReconnectStringFormat(int minute)
    {
        int hour = minute / 60;
        minute %= 60;
        
        return $"{hour:D2}h {minute:D2}m";
    }

    public static (int,int) SecondToHoursAndDays(int second)
    {
        int days = second / 86400;
        second -= days * 86400;

        int hour = second / 3600;

        return (days, hour);
    }

    private const string REMAIN_TIME_HEX_CODE = "#AAFF00";
    public static string GetRemainTimeString(int second, string hexCode = REMAIN_TIME_HEX_CODE)
    {
        int day = second / 86400;
        second %= 86400;

        int hour = second / 3600;
        second  %= 3600;

        int min = second / 60;
        // second %= 60;
        //
        //
        // if (day > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Day_Refresh"),day,hour);
        // }
        // else if (hour > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Hour_Refresh"),hour,min);
        // }
        // else if (min > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Minute_Refresh"),min,second);
        // }
        // else
        // { 
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Second_Refresh"),second);
        // }

        return "";
    }

    public static string GetInitRemainTimeString(int second, string hexCode = REMAIN_TIME_HEX_CODE)
    {
        int day = second / 86400;
        second %= 86400;

        int hour = second / 3600;
        second  %= 3600;

        int min = second / 60;
        second %= 60;
        
        
        // if (day > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Day_Refresh"),day,hour);
        // }
        // else if (hour > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Hour_Refresh"),hour,min);
        // }
        // else if (min > 0)
        // {
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Minute_Refresh"),min,second);
        // }
        // else
        // { 
        //     return String.Format(LocalizeText.GetText("UI_Achievement_Second_Refresh"),second);
        // }
        return "";
    }
    
    public static string GetTimeString(int second)
    {
        int day = second / 86400;
        second %= 86400;

        int hour = second / 3600;
        second  %= 3600;

        int min = second / 60;
        second %= 60;
        
        //
        // if (day > 0)
        // {
        //     return LocalizeText.GetTextFormat("UtilCode_GetTimeString_Days", day, hour); // $"{day}일 {hour}시간";
        // }
        // else if (hour > 0)
        // {
        //     return LocalizeText.GetTextFormat("UtilCode_GetTimeString_Hours", hour, min); //$"{hour}시간 {min}분";
        // }
        // else if (min > 0)
        // {
        //     return LocalizeText.GetTextFormat("UtilCode_GetTimeString_Minutes", min, second); //$"{min}분 {second}초";
        // }
        // else
        // {
        //     return LocalizeText.GetTextFormat("UtilCode_GetTimeString_Seconds", second); //$"{second}초";
        // }
        return "";
    }
    
    public static long UnixTimeNow()
    {
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        return (long)timeSpan.TotalSeconds;
    }
    
    public static double UnixTimeNowMilliseconds()
    {
        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
        return timeSpan.TotalMilliseconds;
    }

    public static string UnixTimeToFromMailFormat(long unixTime)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime from = origin.AddSeconds(unixTime);

        return $"{from.Year}. {from.Month:D2}. {from.Day:D2}";
    }

    public static DateTime UnixTimeToDateTime(long unixTime)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0); 
        
        return origin.AddSeconds(unixTime);
    }
    
    public static string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public static bool GetChance(float chance)
    {
        if (chance <= 0) return false;
        if (chance >= 100) return true;
        
        var randomFloatValue = UnityEngine.Random.Range(0f, 100f);
        
        return chance >= randomFloatValue;
    }

    public static bool GetChance(int chance)
    {
        if (chance <= 0) return false;
        if (chance >= 100) return true;

        var randomIntValue = UnityEngine.Random.Range(0, 101);

        return chance >= randomIntValue;
    }

    public static Dictionary<T, int> ListToDictionary<T>(List<T> list)
    {
        Dictionary<T, int> dictionary = new Dictionary<T, int>(list.Count / 2);

        foreach (T item in list)
        {
            if (!dictionary.ContainsKey(item))
            {
                dictionary[item] = 1;
            }
            else
            {
                dictionary[item]++;
            }
        }

        return dictionary;
    }
    
    public static float Approach(float from, float to, float amount)
    {
        if (from < to)
        {
            from += amount;

            if (from > to)
            {
                return to;
            }
        }
        else
        {
            from -= amount;

            if (from < to)
            {
                return to;
            }
        }

        return from;
    }

    public static double Sigma(int level, int power)
    {
        double n = level;
        
        if (power == 1)
        {
            return (n * (n + 1)) / 2;
        }
        else if (power == 2)
        {
            return (n * (n + 1) * (2 * n + 1)) / 6;
        }
        
        return 0;
    }

    public static bool IsTablet()
    {
        float width = Screen.width;
        float height = Screen.height;
        
        return width / height >= 0.65f;
        
        /*var resolution = Screen.currentResolution;
        float aspectRatio = Mathf.Max(resolution.width, resolution.height) / Mathf.Min(resolution.width, resolution.height);
        float screenWidth = resolution.width / Screen.dpi;
        float screenHeight = resolution.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt (Mathf.Pow (screenWidth, 2) + Mathf.Pow (screenHeight, 2));

        Debug.LogError("width: " + Screen.width);
        Debug.LogError("height: " + Screen.height);
        Debug.LogError("aspectRatio: " + aspectRatio);
        Debug.LogError("screenWidth: " + screenWidth);
        Debug.LogError("screenHeight: " + screenHeight);
        Debug.LogError("diagonalInches: " + diagonalInches);
        
        bool result =  aspectRatio < 2f && diagonalInches > 6.5f;
        ;
        Debug.LogError("tablet: " + result);
        
        return aspectRatio < 2f && diagonalInches > 6.5f;*/
    }
   
    
    public int GetRandomStarCount()
    {
        int random = Random.Range(0, 100);

        if (random < 40)
        {
            return 0;
        }
        else if (random < 70)
        {
            return 1;   
        }        
        else if (random < 90)
        {
            return 2;   
        }
        else
        {
            return 3;
        }
    }
}