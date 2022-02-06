using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DoubleExtension
{
    private const string Zero = "0";
    static readonly string[] Units = new string[] { "",  "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극", "항", "아", "나", "불", "무", "홍", "몽" };
   
    public static string ToCurrencyString(this double value)
    { 
        if (value <= 0) return Zero;
        
        string showString = string.Empty;
        string showDecimalString = string.Empty;
        double decima = 0d;
        string unit = string.Empty;
        string prevUnit = string.Empty; 
        string significant = (value < 0) ? "-" : string.Empty;

        string[] parts = value.ToString("E").Split('+');
        if (parts.Length < 2)
        {
            Debug.LogError($"[X] - ToCurrencyString({value})");
            return Zero;
        }

        // 지수
        if (!int.TryParse(parts[1], out int exp))
        {
            Debug.LogError($"[X] - ToCurrencyString({value}) : parts[1] = {parts[1]}");
            return Zero;
        }
        
        int quot = exp / 4;      // 몫
        int remainder = exp % 4; // 나머지
     
        if (exp < 4)
        {
            showString = $"{Math.Truncate(value)}";
            //showString = $"{System.Math.Truncate(value):####}";//System.Math.Truncate(value).ToString("f0");
        }
        else
        {
            double temp = double.Parse(parts[0].Replace("E", "")) * System.Math.Pow(10, remainder);
            //showString = $"{temp:#,0.###}";//temp.ToString("F3").Replace(".000", ""));
            //showString = temp.ToString("F3").Replace(".000", "");
            double leftValue = Math.Truncate(temp);
            
            showString = $"{leftValue}";

            decima = (temp - leftValue) * 10000;
        }

        unit = Units[quot];

        if (decima > 0)
        {
            showDecimalString = $"{Mathf.RoundToInt((float)decima)}";
            
            if (quot > 1)
            {
                prevUnit = Units[quot - 1];
            }
        }
        
        return $"{significant}{showString}{unit}{showDecimalString}{prevUnit}";
    }
    
    public static string ToPriceString(this double value)
    {
        var currentCulture = CultureInfo.CurrentCulture;
        return string.Format(currentCulture, "{0:##,##0}", value);
    }

    public static string ToDamageString(this double value)
    {
        return value.ToCurrencyString();
    }

    public static string ToHealthString(this double value)
    {
        return value.ToCurrencyString();
    }

    public static string ToStatString(this double value)
    {
        return value.ToCurrencyString();
    }
}