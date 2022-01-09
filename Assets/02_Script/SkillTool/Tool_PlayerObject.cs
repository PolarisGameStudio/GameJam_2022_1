using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

public class Tool_PlayerObject : PlayerObject , GameEventListener<RefreshEvent>
{
    private FSMAbility _fsmAbility;
    private WeaponAbility _toolWeaponAbility;
    private ToolAbility _toolAbility;

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Weapon)
        {
            RefreshWeapon();
        }
    }

    private void Awake()
    {
        GameEventManager.AddListener<RefreshEvent>(this);
    }

    private void Start()
    {
        Init(Enum_CharacterType.Player, PlayerStatManager.Instance.Stat);

        _fsmAbility = GetAbility<FSMAbility>();
        _toolWeaponAbility = GetAbility<WeaponAbility>();
        _toolAbility = GetAbility<ToolAbility>();
        
        _fsmAbility.Register(Enum_PlayerStateType.Attack, new Tool_PlayerAttackState(this,_fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Run, new Tool_PlayerRunState(this,_fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Idle, new Tool_PlayerIdleState(this,_fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Skill, new Tool_PlayerSkillState(this,_fsmAbility.StateMachine));
        //_fsmAbility.Register(Enum_PlayerStateType.Tool_BerserkEnter, new Tool_PlayerBerserkEnterState(this,_fsmAbility.StateMachine));
        
        Stat[Enum_StatType.Damage] = -1;
        Stat[Enum_StatType.DetectRange] = 9999999;
        Stat[Enum_StatType.AttackSpeed] = 1;
        Stat[Enum_StatType.MoveSpeed] = 2;

        RefreshWeapon();
        
        _fsmAbility.Initialize(Enum_PlayerStateType.Run);
    }

    public void RefreshWeapon()
    {
        GetAbility<WeaponAbility>().RefreshWeapon();
    }
    
    
    ///--------------------------------- 시뮬레이션 UI와 통신하는 부분 //  ----------------------------
    /// ----------------------------------------------------------------------------------------
    

    public void ChangeStateToAttack()
    {
        _fsmAbility.ChangeState(Enum_PlayerStateType.Attack);
    }

    public void ChangeStateToSkill()
    {
        _fsmAbility.ChangeState(Enum_PlayerStateType.Skill);
    }
    public void ChangeStateToRun()
    {
        _fsmAbility.ChangeState(Enum_PlayerStateType.Run);
    }
    public void ChangeStateToIdle()
    {
        _fsmAbility.ChangeState(Enum_PlayerStateType.Idle);
    }

    public void SetAttackPreset(AttackPreset selectedAttackPreset)
    {
        _toolAbility.SetAttackPreset(selectedAttackPreset);
    }

    public void SetSkillPresetIndex(int index)
    {
        _toolAbility.SetSkillPresetIndex(index);
    }

    public void ChangeBerserkMode(bool isOn)
    {
        // if (isOn)
        // {
        //     _fsmAbility.ChangeState(Enum_PlayerStateType.Tool_BerserkEnter);
        // }

        if (isOn)
        {
            GetAbility<BerserkAbility>().On();
        }
        else
        {
            GetAbility<BerserkAbility>().Off();
        }

        RefreshWeapon();
        
        ChangeStateToRun();
    }
    
    public void ChangeContinuousAttackMode(bool isOn)
    {
        _toolAbility.ChangeContinuousAttackMode(isOn);

        // if (isOn)
        // {
        //     ChangeStateToAttack();
        // }
    }

    public void ChangeRandomAttackMode(bool isOn)
    {
        _toolAbility.ChangeRandomAttackMode(isOn);

        // if (isOn)
        // {
        //     ChangeStateToAttack();
        // }
    }

    public SkeletonData GetSkeletonData()
    {
        return GetComponentInChildren<SkeletonAnimation>().SkeletonDataAsset.GetSkeletonData(true);
    }

    public void SetPlayerAttackSpeed(float attackSpeed)
    {
        Stat[Enum_StatType.AttackSpeed] = attackSpeed;
    }
    
    public void SetPlayerMoveSpeed(float moveSpeed)
    {
        Stat[Enum_StatType.MoveSpeed] = moveSpeed;
    }
}
