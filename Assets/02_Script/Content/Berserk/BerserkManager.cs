using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkManager : SingletonBehaviour<BerserkManager>
{
    private BerserkerStat _berserkerStat;
    

    // 광폭화 게이지
    private float _gauge;
    public float CurrentGauge => _gauge;
    public float MaxGauge => _berserkerStat.chargeCount; 
    
    
    // 광폭화 남은 시간
    private float _remainTime = 0;
    public float CurrentRemainTime => _remainTime;
    public float MaxRemainTime => _berserkerStat.duration; 

    public bool IsBerserkMode => _remainTime > 0;

    
    // 광폭화 스탯
  //  public readonly Stat Stat = new Stat();     // 여기 접근하는 애들 수정하자.

    
    protected override void Awake()
    {
        base.Awake();

        PlayerDataManager.PlayerDataContainer.GetBerserkerStat(ref _berserkerStat);
        
        Calculate();
    }
    
    // todo: Stat 은 임시로 사용하다가 버서커 관련 성장요소 추가 되면 유저 광폭화레벨 추가 후, PlayerManager랑 마찬가지로 Calculate에서 계산
    private void Calculate()
    {
        // todo: 기획서 대로 임시
        // Stat[Enum_StatType.Damage] = _berserkerStat.GetStatValue(Enum_StatType.Damage, true); //100;
        // Stat[Enum_StatType.AttackSpeed] = _berserkerStat.GetStatValue(Enum_StatType.AttackSpeed, true);//80;
        // Stat[Enum_StatType.MoveSpeed] = _berserkerStat.GetStatValue(Enum_StatType.MoveSpeed, true);//50;
    }

    public void OnPlayerAttack()
    {
        if (IsBerserkMode)
        {
            return;
        }

        _gauge = Math.Min(_gauge + 1, MaxGauge);
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Berserk);

        if (OptionManager.Instance.IsAutoBerserkMode)
        {
            TryBerserkMode();
        }
    }

    public bool IsEnableBerserkMode()
    {
        return !IsBerserkMode && (_gauge >= MaxGauge);
    }

    public bool TryBerserkMode()
    {
        if (!IsEnableBerserkMode())
        {
            return false;
        }
        _remainTime = MaxRemainTime;

        _gauge = 0;
        
        _berserkerStat.SetBerserkerState(true);
        RefreshEvent.Trigger(Enum_RefreshEventType.Berserk);
        RefreshEvent.Trigger(Enum_RefreshEventType.Stat);
        BerserkEvent.Trigger(Enum_BerserkEventType.Start);
        
        return true;
    }

    private void Update()
    {
        if (!IsBerserkMode)
        {
            return;
        }

        var dt = Time.deltaTime;

        _remainTime -= dt;
        
        if (_remainTime <= 0)
        {
            _remainTime = 0;
            
            _berserkerStat.SetBerserkerState(false);
            RefreshEvent.Trigger(Enum_RefreshEventType.Berserk);
            RefreshEvent.Trigger(Enum_RefreshEventType.Stat);
            BerserkEvent.Trigger(Enum_BerserkEventType.End);
        }
    }
}
