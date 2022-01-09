using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.LuisPedroFonseca.ProCamera2D;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

public class SkillTool_BlackoutPresetSetting : MonoBehaviour
{
    public Text txtPresetName;

    private BlackoutPreset _currentPreset;

    private const string Prefix_Preset = "Assets/12_Preset/BlackoutPreset/";
    private const string Postfix_Preset = ".asset";

    private SkillTool_AttackPresetSetting _attackPresetSetting;

    private List<BlackoutPreset> _loadedBlackoutPresetList;
    private int _loadedBlackoutPresetIndex = 0;

    public void Init(BlackoutPreset preset, SkillTool_AttackPresetSetting attackPresetSetting)
    {
        _attackPresetSetting = attackPresetSetting;

        InitializePanel(preset);

        LoadPresetFromPath();
    }

    private void InitializePanel(BlackoutPreset preset)
    {
        _currentPreset = preset;

        if (_currentPreset != null)
        {
            txtPresetName.text = _currentPreset.name;
        }
        else
        {
            txtPresetName.text = "등록된 프리셋이 없습니다.";
        }
    }

    public void AcceptToAttackPreset()
    {
        if (_currentPreset == null)
        {
            return;
        }

        _attackPresetSetting.AcceptBlackoutPreset(_currentPreset);
    }

    public void Focus()
    {
        if (_currentPreset == null)
        {
            return;
        }

#if UNITY_EDITOR
        StringBuilder builder = new StringBuilder();

        builder.Append(Prefix_Preset);
        builder.Append(_currentPreset.name);
        builder.Append(Postfix_Preset);
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(builder.ToString(), typeof(UnityEngine.Object));

        Selection.activeObject = obj;

        EditorGUIUtility.PingObject(obj);
#endif
    }

    public void LoadPresetFromPath()
    {
#if UNITY_EDITOR
        _loadedBlackoutPresetList = new List<BlackoutPreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_Preset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        foreach (var fileInfo in fileInfos)
        {
            if (fileInfo.Name.Contains(".meta"))
            {
                continue;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(Prefix_Preset);
            builder.Append(fileInfo.Name);
            //builder.Append(Postfix_Preset);

            var blackoutPreset = AssetDatabase.LoadAssetAtPath<BlackoutPreset>(builder.ToString());

            _loadedBlackoutPresetList.Add(blackoutPreset);
        }
#endif
    }

    public void MoveToPreviousPreset()
    {
        _loadedBlackoutPresetIndex--;
        _loadedBlackoutPresetIndex = Mathf.Clamp(_loadedBlackoutPresetIndex, 0, _loadedBlackoutPresetList.Count - 1);

        InitializePanel(_loadedBlackoutPresetList[_loadedBlackoutPresetIndex]);

        Focus();
    }

    public void MoveToNextPreset()
    {
        _loadedBlackoutPresetIndex++;
        _loadedBlackoutPresetIndex = Mathf.Clamp(_loadedBlackoutPresetIndex, 0, _loadedBlackoutPresetList.Count - 1);

        InitializePanel(_loadedBlackoutPresetList[_loadedBlackoutPresetIndex]);

        Focus();
    }

    public void PlayBlackout()
    {
        if (_currentPreset == null)
        {
            return;
        }

        BlackoutManager.Instance.PlayBlackOut(_currentPreset);
    }

    public void CreateBlackoutPresetAsset()
    {
#if UNITY_EDITOR
        BlackoutPreset preset = ScriptableObject.CreateInstance<BlackoutPreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_Preset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;

        AssetDatabase.CreateAsset(preset, $"{Prefix_Preset}NewAttackPreset{localPresetCount}.asset");
        AssetDatabase.SaveAssets();

        LoadPresetFromPath();

        InitializePanel(_loadedBlackoutPresetList[localPresetCount]);

        Focus();
#endif
    }
}