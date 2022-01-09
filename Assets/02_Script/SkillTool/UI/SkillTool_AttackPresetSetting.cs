using System.IO;
using System.Text;
using Com.LuisPedroFonseca.ProCamera2D;
using Spine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;


public class SkillTool_AttackPresetSetting : MonoBehaviour
{
    public SkillTool_ShakePresetSetting ShakePresetSetting;
    public SkillTool_BlackoutPresetSetting BlackoutPresetSetting;
    public Text txtPresetName;

    [Header("애니메이션")] public InputField ipfAnimationName;
    public Dropdown dropDownAnimationName;

    [Header("데미지 딜레이")] public Slider sliderDamageDelay;
    public Text txtDamageDelay;

    private AttackPreset _currentAttackPreset;

    private const string Prefix_AttackPreset = "Assets/12_Preset/AttackType/";
    private const string Postfix_Preset = ".asset";

    private Tool_PlayerObject _toolPlayerObject;

    private AttackPreset _initializedPreset;

    public void Init(AttackPreset preset, Tool_PlayerObject toolPlayerObject)
    {
        SaveInitializedPreset(preset);

        _currentAttackPreset = preset;
        _toolPlayerObject = toolPlayerObject;

        ShakePresetSetting.Init(preset.ShakePreset, this);
        BlackoutPresetSetting.Init(preset.BlackoutPreset, this);

        InitializeDropDown();
        InitailizePanel();
    }

    private void SaveInitializedPreset(AttackPreset preset)
    {
        _initializedPreset = ScriptableObject.CreateInstance<AttackPreset>();

        _initializedPreset.AnimtaionName = preset.AnimtaionName;
        _initializedPreset.DamageDelay = preset.DamageDelay;
        _initializedPreset.ShakePreset = preset.ShakePreset;
        _initializedPreset.SlowPreset = preset.SlowPreset;
        _initializedPreset.BlackoutPreset = preset.BlackoutPreset;
        _initializedPreset.ShakeImmediately = preset.ShakeImmediately;
    }

    private void InitializeDropDown()
    {
        dropDownAnimationName.ClearOptions();

        var animationList = _toolPlayerObject.GetSkeletonData().Animations;

        int currentAnimationIndex = 0;

        for (int i = 0; i < animationList.Count; i++)
        {
            dropDownAnimationName.options.Add(new Dropdown.OptionData(animationList.Items[i].Name));

            if (animationList.Items[i].Name == _currentAttackPreset.AnimtaionName)
            {
                currentAnimationIndex = i;
            }
        }

        dropDownAnimationName.value = currentAnimationIndex;
        dropDownAnimationName.RefreshShownValue();
    }

    public void InitailizePanel()
    {
        txtPresetName.text = _currentAttackPreset.name;
        ipfAnimationName.text = _currentAttackPreset.AnimtaionName;

        sliderDamageDelay.value = _currentAttackPreset.DamageDelay;

        SetDamageDelayValue(_currentAttackPreset.DamageDelay);
    }

    public void SetDamageDelayValue(float value)
    {
        txtDamageDelay.text = value.ToString("F");
        _currentAttackPreset.DamageDelay = sliderDamageDelay.value;
    }

    public void SetAnimationName(int index)
    {
        _currentAttackPreset.AnimtaionName = dropDownAnimationName.options[index].text;
    }


    public void Save()
    {
        if (_currentAttackPreset == null)
        {
            return;
        }

        //_currentAttackPreset.AnimtaionName = ipfAnimationName.text;
        _currentAttackPreset.AnimtaionName = dropDownAnimationName.options[dropDownAnimationName.value].text;
        _currentAttackPreset.DamageDelay = sliderDamageDelay.value;
    }

    public void PlayAttackPreset()
    {
        _toolPlayerObject.SetAttackPreset(_currentAttackPreset);
        _toolPlayerObject.ChangeStateToAttack();
    }

    public void Focus()
    {
#if UNITY_EDITOR
        StringBuilder builder = new StringBuilder();

        builder.Append(Prefix_AttackPreset);
        builder.Append(_currentAttackPreset.name);
        builder.Append(Postfix_Preset);

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(builder.ToString(), typeof(UnityEngine.Object));

        Selection.activeObject = obj;

        EditorGUIUtility.PingObject(obj);
#endif
    }

    public void AcceptBlackoutPreset(BlackoutPreset blackoutPreset)
    {
        _currentAttackPreset.BlackoutPreset = blackoutPreset;
    }

    public void AcceptShakePreset(ShakePreset shakePreset)
    {
        _currentAttackPreset.ShakePreset = shakePreset;
    }

    public void RevertAttackPreset()
    {
        _currentAttackPreset.AnimtaionName = _initializedPreset.AnimtaionName;
        _currentAttackPreset.DamageDelay = _initializedPreset.DamageDelay;
        _currentAttackPreset.ShakePreset = _initializedPreset.ShakePreset;
        _currentAttackPreset.SlowPreset = _initializedPreset.SlowPreset;
        _currentAttackPreset.BlackoutPreset = _initializedPreset.BlackoutPreset;
        _currentAttackPreset.ShakeImmediately = _initializedPreset.ShakeImmediately;

        InitializeDropDown();
        InitailizePanel();
    }
}