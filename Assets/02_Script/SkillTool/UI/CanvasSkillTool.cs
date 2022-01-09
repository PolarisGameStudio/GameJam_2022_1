using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkillTool : MonoBehaviour
{
    private AttackPreset _currentAttackPreset;
    private int _attackPresetIndex;
    private int _skillPresetIndex;
    private int _backgroundSetIndex;

    public Text txtAttackPresetName;
    public Text txtSkillPresetName;
    public Text txtCurrentWeaponCombination;
    public Text txtBackgroundSetName;
    public Text txtAttackSpeed;
    public Text txtMoveSpeed;

    public SkillTool_AttackPresetSetting AttackPresetSetting;

    [SerializeField] private Tool_PlayerObject _toolPlayerObject;
    [SerializeField] private Tool_MonsterObject _toolMonsterObject;

    private PlayerSkillManager _toolSkillManager;

    private BackgroundManager _backgroundManager;

    private void Start()
    {
        _attackPresetIndex = 0;
        _skillPresetIndex = 0;
        _backgroundSetIndex = 0;

        _toolSkillManager = PlayerSkillManager.Instance;
        _backgroundManager = BackgroundManager.Instance;

        SetCurrentAttackPreset();
        SetSkillPresetButton();
        SetBackground();

        _toolPlayerObject = FindObjectOfType<Tool_PlayerObject>();
        _toolMonsterObject = FindObjectOfType<Tool_MonsterObject>();
    }

    public void AddAttackPresetIndex(int number)
    {
        _attackPresetIndex += number;

        var attackPresetList = _toolPlayerObject.GetAbility<WeaponAbility>().GetAttackPresetList();
        _attackPresetIndex = Mathf.Clamp(_attackPresetIndex, 0, attackPresetList.Count - 1);

        SetCurrentAttackPreset();
    }

    private void ResetAttackPresetIndex()
    {
        _attackPresetIndex = 0;

        SetCurrentAttackPreset();
    }

    private string _currentAttackPresetName = string.Empty;

    private void SetCurrentAttackPreset()
    {
        var attackPresetList = _toolPlayerObject.GetAbility<WeaponAbility>().GetAttackPresetList();

        _currentAttackPreset = attackPresetList[_attackPresetIndex];

        SetAttackPresetUI();
    }

    private void SetAttackPresetUI()
    {
        _currentAttackPresetName = _currentAttackPreset.name;
        txtAttackPresetName.text = _currentAttackPresetName;

        AttackPresetSetting.Init(_currentAttackPreset, _toolPlayerObject);
    }


    public void AddSkillPresetIndex(int number)
    {
        _skillPresetIndex += number;

        _skillPresetIndex = Mathf.Clamp(_skillPresetIndex, 0, 1);

        SetSkillPresetButton();
    }

    private void ResetSkillPresetIndex()
    {
        _skillPresetIndex = 0;

        SetSkillPresetButton();
    }

    private void SetSkillPresetButton()
    {
        txtSkillPresetName.text = _toolSkillManager.GetActiveSkill(_skillPresetIndex).name;
    }

    public void PlayAttack()
    {
        _toolPlayerObject.SetAttackPreset(_currentAttackPreset);
        _toolPlayerObject.ChangeStateToAttack();
    }

    public void PlaySkill()
    {
        _toolPlayerObject.SetSkillPresetIndex(_skillPresetIndex);
        _toolPlayerObject.ChangeStateToSkill();
    }

    public void PlayIdle()
    {
        _toolPlayerObject.ChangeStateToIdle();
    }

    public void PlayRun()
    {
        _toolPlayerObject.ChangeStateToRun();
    }


    public void ChangeWeaponLeftHand(int weaponIndex)
    {
        Enum_WeaponType weaponType = (Enum_WeaponType) weaponIndex;

        Tool_WeaponManager.Instance.ChangeWeapon(true, weaponType);

        txtCurrentWeaponCombination.text = Tool_WeaponManager.Instance.GetCurrentCombination().ToString();

        ResetAttackPresetIndex();
    }

    public void ChangeWeaponRightHand(int weaponIndex)
    {
        Enum_WeaponType weaponType = (Enum_WeaponType) weaponIndex;

        Tool_WeaponManager.Instance.ChangeWeapon(false, weaponType);

        txtCurrentWeaponCombination.text = Tool_WeaponManager.Instance.GetCurrentCombination().ToString();

        ResetAttackPresetIndex();
    }

    public void ChangeBerserkMode(bool isOn)
    {
        _toolPlayerObject.ChangeBerserkMode(isOn);

        ResetAttackPresetIndex();
    }

    public void ChangeContinuousAttackMode(bool isOn)
    {
        _toolPlayerObject.ChangeContinuousAttackMode(isOn);
    }

    public void ChangeRandomAttackMode(bool isOn)
    {
        _toolPlayerObject.ChangeRandomAttackMode(isOn);
    }

    public void SetPlayerAttackSpeed(float speed)
    {
        txtAttackSpeed.text = $"공격 속도 : {speed:N2}";
        _toolPlayerObject.SetPlayerAttackSpeed(speed);
    }

    public void SetPlayerMoveSpeed(float speed)
    {
        txtMoveSpeed.text = $"이동 속도 : {speed:N2}";
        _toolPlayerObject.SetPlayerMoveSpeed(speed);
    }

    public void AddBackgroundSetIndex(int number)
    {
        _backgroundSetIndex += number;

        _backgroundSetIndex =
            Mathf.Clamp(_backgroundSetIndex, 0, Enum.GetValues(typeof(Enum_BackgroundType)).Length - 1);

        SetBackground();
    }

    private void SetBackground()
    {
        var backgroundType = (Enum_BackgroundType) _backgroundSetIndex;

        txtBackgroundSetName.text = backgroundType.ToString();
        //_backgroundManager.SetBackground(backgroundType);
    }

    public void ShowDamageText(bool isCritical)
    {
        _toolMonsterObject.TryTakeHit(1234567890, isCritical ? Enum_DamageType.Critical : Enum_DamageType.Normal);
    }
}