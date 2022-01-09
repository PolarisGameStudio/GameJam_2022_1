using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEditor;
using UnityEngine;

public static class PresetCreator
{
    private const string Prefix_AttackPreset = "Assets/12_Preset/AttackPreset/";
    private const string Prefix_ShakePreset = "Assets/12_Preset/ShakePreset/";
    private const string Prefix_BlackoutPreset = "Assets/12_Preset/BlackoutPreset/";
    private const string Prefix_SkillAttackPreset = "Assets/12_Preset/AttackPreset/SkillPreset/";
    private const string Postfix_Preset = ".asset";
    
    
    public static AttackPreset CreateAttackPresetAsset(string name = "NewAttackPreset" , bool highlight = false)
    {
#if UNITY_EDITOR
        AttackPreset preset = ScriptableObject.CreateInstance<AttackPreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_AttackPreset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;

        string path = $"{Prefix_AttackPreset}{name}{Postfix_Preset}";
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();

        if (highlight)
        {
            Focus(path);
        }

        return preset;
#endif
        return null;
    }    
        
    public static AttackPreset CreateSkillAttackPresetAsset(string name = "NewSkillPreset" , bool highlight = false)
    {
#if UNITY_EDITOR
        AttackPreset preset = ScriptableObject.CreateInstance<AttackPreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_SkillAttackPreset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;

        string path = $"{Prefix_SkillAttackPreset}{name}{Postfix_Preset}";
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();

        if (highlight)
        {
            Focus(path);
        }

        return preset;
#endif
        return null;
    }    
    

    public static ShakePreset CreateShakePresetAsset()
    {
#if UNITY_EDITOR
        ShakePreset preset = ScriptableObject.CreateInstance<ShakePreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_ShakePreset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;

        string path = $"{Prefix_ShakePreset}NewShakePreset{localPresetCount}{Postfix_Preset}";
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();

        Focus(path);

        return preset;
#endif
        return null;
    }

    public static BlackoutPreset CreateBlackoutPresetAsset()
    {
#if UNITY_EDITOR
        BlackoutPreset preset = ScriptableObject.CreateInstance<BlackoutPreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_BlackoutPreset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;

        string path = $"{Prefix_BlackoutPreset}NewShakePreset{localPresetCount}{Postfix_Preset}";
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();

        Focus(path);

        return preset;
#endif
        return null;
    }

    public static void Focus(string path)
    {
#if UNITY_EDITOR

        Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

        Selection.activeObject = obj;

        EditorGUIUtility.PingObject(obj);
#endif
    }
}