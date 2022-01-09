using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension
{
    public static string ToCurrencyString(this float value) => ((double) value).ToCurrencyString();
    public static string ToPriceString(this float value) => ((double) value).ToPriceString();
    public static string ToDamageString(this float value) => ((double) value).ToDamageString();
    public static string ToHealthString(this float value) => ((double) value).ToHealthString();
    public static string ToStatString(this float value) => ((double) value).ToStatString();

}
