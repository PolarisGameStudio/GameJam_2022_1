using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.LuisPedroFonseca.ProCamera2D;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class SkillTool_ShakePresetSetting : MonoBehaviour
{
    public Text txtPresetName;

    private ShakePreset _currentPreset;

    private const string Prefix_Preset = "Assets/12_Preset/ShakePreset/";
    private const string Postfix_Preset = ".asset";

    private SkillTool_AttackPresetSetting _attackPresetSetting;

    private List<ShakePreset> _loadedShakePresetList;
    private int _loadedShakePresetIndex = 0;

    public void Init(ShakePreset preset, SkillTool_AttackPresetSetting attackPresetSetting)
    {
        _attackPresetSetting = attackPresetSetting;

        InitializePanel(preset);

        LoadPresetFromPath();
    }

    private void InitializePanel(ShakePreset preset)
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


    public void AcceptToAttackPreset()
    {
        if (_currentPreset == null)
        {
            return;
        }

        _attackPresetSetting.AcceptShakePreset(_currentPreset);
    }

    public void PlayShake()
    {
        if (_currentPreset == null)
        {
            return;
        }

        BattleCamera.Instance.Shake(_currentPreset);
    }

    public void LoadPresetFromPath()
    {
#if UNITY_EDITOR
        _loadedShakePresetList = new List<ShakePreset>();

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

            var shakePreset = AssetDatabase.LoadAssetAtPath<ShakePreset>(builder.ToString());

            _loadedShakePresetList.Add(shakePreset);
        }
#endif
    }

    public void MoveToPreviousPreset()
    {
        _loadedShakePresetIndex--;
        _loadedShakePresetIndex = Mathf.Clamp(_loadedShakePresetIndex, 0, _loadedShakePresetList.Count - 1);

        InitializePanel(_loadedShakePresetList[_loadedShakePresetIndex]);

        Focus();
    }

    public void MoveToNextPreset()
    {
        _loadedShakePresetIndex++;
        _loadedShakePresetIndex = Mathf.Clamp(_loadedShakePresetIndex, 0, _loadedShakePresetList.Count - 1);

        InitializePanel(_loadedShakePresetList[_loadedShakePresetIndex]);

        Focus();
    }


    public void CreateShakePresetAsset()
    {
#if UNITY_EDITOR
        ShakePreset preset = ScriptableObject.CreateInstance<ShakePreset>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Prefix_Preset);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        var localPresetCount = fileInfos.Length / 2;
        AssetDatabase.CreateAsset(preset, $"{Prefix_Preset}NewShakePreset{localPresetCount}.asset");
        AssetDatabase.SaveAssets();

        LoadPresetFromPath();

        InitializePanel(_loadedShakePresetList[localPresetCount]);

        Focus();
#endif
    }
}