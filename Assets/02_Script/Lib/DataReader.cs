using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, string>> ReadFromLocal(Enum_DataTableType type)
    {
        var list = new List<Dictionary<string, string>>();

        TextAsset data = Resources.Load(type.ToString()) as TextAsset;

        if (data == null)
        {
            Debug.LogError($"[X] {type.ToString()} 데이터 파일이 존재하지 않습니다.");

            return null;
        }

        return ParseCSV(data.text);
    }
    
    public static List<Dictionary<string, string>> ReadFromWeb(Enum_DataTableType type)
    {
        var list = new List<Dictionary<string, string>>();
        //string data = SheetManager.Instance.GetSheetText(type);
        

        return list;
    }

    private static List<Dictionary<string, string>> ParseCSV(string text)
    {
        var list = new List<Dictionary<string, string>>();

        var lines = Regex.Split(text, LINE_SPLIT_RE);

        int length = lines.Length;
        
        if (length <= 1)
        {
            return list;
        }
        
        var header = Regex.Split(lines[0], SPLIT_RE);

        for (int i = 1; i < length; ++i)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
            {
                continue;
            }

            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; ++j)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                string finalValue = value;

                
                /*if (int.TryParse(value, out var intNumber))
                {
                    finalValue = intNumber;
                }
                else if (float.TryParse(value, out var floatNumber))
                {
                    finalValue = floatNumber;
                }*/

                entry[header[j]] = finalValue;
            }

            list.Add(entry);
        }

        return list;
    }
}