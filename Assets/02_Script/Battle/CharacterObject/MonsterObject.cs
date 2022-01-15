using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MonsterObject : CharacterObject
{
    private Monster _monster;
    public Monster Monster => _monster;
    
    private FSMAbility _fsmAbility;
    private bool _fsmInited = false;

    private const int BossAbilityMultiple = 10;
  
    private void InitFSM()
    {
        if (_fsmInited)
        {
            return;
        }

        _fsmAbility = GetAbility<FSMAbility>();

        _fsmAbility.Register(Enum_MonsterStateType.Init, new MonsterInitState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_MonsterStateType.Idle, new MonsterIdleState(this, _fsmAbility.StateMachine));
        // _fsmAbility.Register(Enum_MonsterStateType.Run,  new MonsterRunState(this, _fsmAbility.StateMachine));
        //_fsmAbility.Register(Enum_MonsterStateType.Attack, new MonsterAttackState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_MonsterStateType.Death, new MonsterDeathState(this, _fsmAbility.StateMachine));

        _fsmAbility.Initialize(Enum_MonsterStateType.Init);
        
        _fsmInited = true;
    }

    private void InitStat()
    {
        Stat = _monster.Stat;
        Stat[Enum_StatType.Damage] *= BattleManager.Instance.CurrentBattle.DamageFactor;
        Stat[Enum_StatType.MaxHealth] *= BattleManager.Instance.CurrentBattle.HealthFactor;
        
        _currentHealth = Stat[Enum_StatType.MaxHealth];

        // todo: 매직넘버 + 거리 느낌을 주기 의해 들어간건가? 이런 애들때문에 예측 계산이 맞지 않음.
        Stat[Enum_StatType.DetectRange] += Random.Range(0, 2f);
    }
    
    public void Init(Enum_CharacterType characterType, Vector3 initPosition, Monster monster, SkeletonDataAsset skeletonDataAsset, AttackPreset attackPreset)
    {
        // _monster = MonsterManager.Instance.GetMonster(monsterIndex);
        _monster = monster;
        
        _characterType = characterType;
        
        InitFSM();
        InitStat();
        
        base.Init(characterType);

        InitScale();

        transform.position = initPosition;
        
        InitAbilities();

        GetAbility<AnimationAbility>().SetSkeletonDataAsset(skeletonDataAsset);
       // GetAbility<MonsterAttackAbility>().SetAttackPreset(attackPreset);
        GetAbility<ShadowAbility>().SetSize(GetAbility<AnimationAbility>().Width);
        
        //_fsmAbility.ChangeState(Enum_MonsterStateType.Run);
        
        SafeSetActive(true);
    }

    private void InitScale()
    {
        switch (CharacterType)
        {
            case Enum_CharacterType.StageNormalMonster:
                Model.localScale = new Vector3(0.8f, 0.8f, 1f);
                break;
            
            case Enum_CharacterType.StageBossMonster:
                Model.localScale = new Vector3(1.6f, 1.6f, 1);
                break;
        }
    }

    protected override void OnTakeHit(double damage, Enum_DamageType damageType)
    {
        GetAbility<MeshRendererAbility>().Flash();

        var animationAbility = GetAbility<AnimationAbility>();
        var height = animationAbility.Height;
        var width = animationAbility.Width;
            
        // TODO: 1. Damage Text
        DamageTextFactory.Instance.Show(transform.position, height, width, damage, damageType);
        
        // TODO: 2. Hit VFX
        HitVFXFactory.Instance.Show(transform.position, height, width, damageType);
        
        // 넉백
        GetAbility<KnockbackAbility>().Knockback();
    }

    protected override void OnDeath()
    {
        _fsmAbility.ChangeState(Enum_MonsterStateType.Death);
        
        CurrencyManager.Instance.AddGold(BattleManager.Instance.CurrentBattle.GoldAmount);
        PlayerDataManager.PlayerDataContainer.AddExp(BattleManager.Instance.CurrentBattle.ExpAmount);
                
        MonsterEvent.Trigger(Enum_MonsterEventType.NormalMonsterDeath);

        switch (CharacterType)
        {
            case Enum_CharacterType.StageNormalMonster:
                MonsterEvent.Trigger(Enum_MonsterEventType.NormalMonsterDeath);
                break;
            
            case Enum_CharacterType.StageBossMonster:
                MonsterEvent.Trigger(Enum_MonsterEventType.BossMonsterDeath);
                break;
        }
    }
}
