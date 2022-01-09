using System;
using System.Globalization;
using UnityEngine;

namespace PCG.String
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s, bool trim = true)
        {
            if (s == null)
                return true;

            return trim ? s.Trim().Length == 0 : s.Length == 0;
        }

        public static bool VersionIsMinorThan(this string s, string version)
        {
            string[] strArray1 = s.Split('.');
            string[] strArray2 = version.Split('.');
            
            if (strArray1.Length != strArray2.Length)
                throw new ArgumentException("Invalid");

            for (int index = 0; index < strArray1.Length; ++index)
            {
                int int32_1 = Convert.ToInt32(strArray1[index]);
                int int32_2 = Convert.ToInt32(strArray2[index]);
                
                if (index == strArray1.Length - 1)
                    return int32_1 < int32_2;
                
                if (int32_1 != int32_2)
                {
                    if (int32_1 < int32_2)
                        return true;
                    if (int32_1 > int32_2)
                        return false;
                }
            }

            throw new ArgumentException("Invalid");
        }

        public static string FileOrDirectoryName(this string path)
        {
            if (path.Length <= 1)
                return path;

            int length1 = path.LastIndexOfAnySlash();
            int length2 = path.LastIndexOf('.');
            if (length2 != -1 && length2 > length1)
                path = path.Substring(0, length2);

            if (length1 == -1)
                return path;

            if (length1 == path.Length - 1)
            {
                path = path.Substring(0, length1);
                length1 = path.LastIndexOfAnySlash();

                if (length1 == -1)
                    return path;
            }

            return path.Substring(length1 + 1);
        }

        public static Color HexToColor(this string hex)
        {
            if (hex[0] == '#')
                hex = hex.Substring(1);

            int length = hex.Length;
            if (length < 6)
            {
                double num1 = (HexToInt(hex[0]) + HexToInt(hex[0]) * 16.0) / byte.MaxValue;
                float num2 = (float)((HexToInt(hex[1]) + HexToInt(hex[1]) * 16.0) / byte.MaxValue);
                float num3 = (float)((HexToInt(hex[2]) + HexToInt(hex[2]) * 16.0) / byte.MaxValue);
                float num4 = length == 4 ? (float)((HexToInt(hex[3]) + HexToInt(hex[3]) * 16.0) / byte.MaxValue) : 1f;

                return new Color((float)num1, num2, num3, num4);
            }

            double num8 = (HexToInt(hex[1]) + HexToInt(hex[0]) * 16.0) / byte.MaxValue;
            float num9 = (float)((HexToInt(hex[3]) + HexToInt(hex[2]) * 16.0) / byte.MaxValue);
            float num10 = (float)((HexToInt(hex[5]) + HexToInt(hex[4]) * 16.0) / byte.MaxValue);
            float num11 = length == 8 ? (float)((HexToInt(hex[7]) + HexToInt(hex[6]) * 16.0) / byte.MaxValue) : 1f;

            return new Color((float)num8, num9, num10, num11);
        }

        private static int HexToInt(char hexVal) => int.Parse(hexVal.ToString(), NumberStyles.HexNumber);

        private static int LastIndexOfAnySlash(this string str)
        {
            int num = str.LastIndexOf('/');
            if (num == -1)
                num = str.LastIndexOf('\\');
            return num;
        }
    }
}