using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestStat : SingletonBehaviour<UI_TestStat>, GameEventListener<RefreshEvent>
{
    public Text txtAttackSpeed;
    public Slider sliderAttackSpeed;
    
    public Text txtMoveSpeed;
    public Slider sliderMoveSpeed;
    
    public Text txtBerserkAttackSpeed;
    public Slider sliderBerserkAttackSpeed;
    
    public Text txtBerserkMoveSpeed;
    public Slider sliderBerserkMoveSpeed;
    
    public Text txtAttackMaxSpeed;
    public Slider sliderAttackMaxSpeed;
    
    private Stat _stat;
    private Stat _berserkStat;
    
    public Slider sliderCameraSize;
    public Text txtCameraSize;
    
    public void OnGameEvent(RefreshEvent e)
    {
        Refresh();

        // if (e.Type == Enum_RefreshEventType.Camera)
        // {
        //     InitCameraSlider();
        // }
    }


    private void Start()
    {
        this.AddGameEventListening<RefreshEvent>();
        
       // _stat = PlayerStatManager.Instance.Stat;
        // _berserkStat = BerserkManager.Instance.StatChange;
        
        Refresh();
        
        SafeSetActive(false);
    }
    
    private void Refresh()
    {
        SetPlayerAttackSpeed();
        SetPlayerMoveSpeed();
        SetAttackMaxSpeed();
        SetCameraSize();
    }
    
    public void SetPlayerAttackSpeed()
    {
        sliderAttackSpeed.value = (float)_stat[Enum_StatType.AttackSpeed];
        sliderBerserkAttackSpeed.value = (float)_berserkStat[Enum_StatType.AttackSpeed];
        
        txtAttackSpeed.text = $"공격 속도 : {_stat[Enum_StatType.AttackSpeed]:N2}";
        txtBerserkAttackSpeed.text = $"광폭화 공격 속도 : {_berserkStat[Enum_StatType.AttackSpeed]:N2}";
    }

    public void SetPlayerMoveSpeed()
    {
        sliderMoveSpeed.value = (float)_stat[Enum_StatType.MoveSpeed];
        sliderBerserkMoveSpeed.value = (float)_berserkStat[Enum_StatType.MoveSpeed];

        txtMoveSpeed.text = $"이동 속도 : {_stat[Enum_StatType.MoveSpeed]:N2}";
        txtBerserkMoveSpeed.text = $"광폭화 이동 속도 : {_berserkStat[Enum_StatType.MoveSpeed]:N2}";
    }

    public void SetAttackMaxSpeed()
    {
        float maxSpeed = BattleManager.Instance.PlayerObject.GetAbility<AnimationAbility>()
            .MaxAttackAnimationSpeedScale;

        sliderAttackMaxSpeed.value = maxSpeed;

        txtAttackMaxSpeed.text = $"애니메이션 최고 속도 : {maxSpeed}";
    }

    public void OnChangeAttackMaxSpeed(float value)
    {
        BattleManager.Instance.PlayerObject.GetAbility<AnimationAbility>()
            .MaxAttackAnimationSpeedScale = value;
        
        Refresh();
    }

    public void OnChangePlayerAttackSpeed(float value)
    {
        _stat[Enum_StatType.AttackSpeed] = value;
        
        Refresh();
    }
    
    
    public void OnChangePlayerMoveSpeed(float value)
    {
        _stat[Enum_StatType.MoveSpeed] = value;
        
        Refresh();
    }

    public void OnChangeBerserkAttackSpeed(float value)
    {
        _berserkStat[Enum_StatType.AttackSpeed] = value;
        
        Refresh();
    }
    
    public void OnChangeBerserkMoveSpeed(float value)
    {
        _berserkStat[Enum_StatType.MoveSpeed] = value;
        
        Refresh();
    }

    public void OnClickButton()
    {
        if (isActiveAndEnabled)
        {
            SafeSetActive(false);
        }
        else
        {
            SafeSetActive(true);
        }
    }

    private void InitCameraSlider()
    {
        float size = BattleCamera.Instance.OriginOrthographicSize;
        sliderCameraSize.value = size;
        sliderCameraSize.minValue = size - size / 3f;
        sliderCameraSize.maxValue = size + size / 3f;
    }
    
    private void SetCameraSize()
    {
        float size = BattleCamera.Instance.OrthographicSize;
        sliderCameraSize.value = size;
        txtCameraSize.text = $"카메라 크기: {size}";
    }

    public void OnChangeCameraSize(float value)
    {
        BattleCamera.Instance.SetOrthographicSize(value);
    }

    public void OnChangeCameraZoomType(int value)
    {
       // Debug.LogError(value);

        BattleCamera.Instance.SetZoomType((Enum_CameraZoomType) value);
    }
}
