using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : CharacterObject, GameEventListener<StatEvent>, GameEventListener<FollowerEvent>,GameEventListener<WeaponEvent>
{
    private FSMAbility _fsmAbility;

    private float _defaultScaleSize = 0.8f;
    private float _berserkScaleSize = 1.0f;

    private HealthbarObject _healthbarObject;

    public Transform SkillEffectPos;

    [SerializeField] private List<FollowerObject> _followers;

    private void Awake()
    {
        Init(Enum_CharacterType.Player, new Stat());

        _fsmAbility = GetAbility<FSMAbility>();

        _fsmAbility.Register(Enum_PlayerStateType.Init, new PlayerInitState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Idle, new PlayerIdleState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Run, new PlayerRunState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Attack, new PlayerAttackState(this, _fsmAbility.StateMachine));
        _fsmAbility.Register(Enum_PlayerStateType.Death, new PlayerDeathState(this, _fsmAbility.StateMachine));
        //_fsmAbility.Register(Enum_PlayerStateType.Skill, new PlayerSkillState(this, _fsmAbility.StateMachine));

        _fsmAbility.Initialize(Enum_PlayerStateType.Init);

        GetAbility<ShadowAbility>().SetSize(GetAbility<AnimationAbility>().Width);

        this.AddGameEventListening<StatEvent>();
        this.AddGameEventListening<FollowerEvent>();
        this.AddGameEventListening<WeaponEvent>();
        // this.AddGameEventListening<RefreshEvent>();

        RefreshFollowers();
        RefreshWeaponSkin();

        CalculateStat();
    }
    
    public override void CalculateStat()
    {
        GetAbility<PlayerStatAbility>().Calculate();
    }

    public void BattleStart(Vector3 newPosition)
    {
        _currentHealth = Stat[Enum_StatType.MaxHealth];

        Model.localPosition = Vector3.zero;

        transform.position = newPosition;

        _alive = true;

        foreach (var ability in _abilities)
        {
            ability.Init();
        }

        SafeSetActive(true);

        RefreshHealthbar();
        RefreshSkillAbility();

        GetAbility<PlayerSkillAbility>().HideSkills();

        _fsmAbility.ChangeState(Enum_PlayerStateType.Run);
    }

    public void RefreshHealthbar()
    {
        if (!_healthbarObject || !_healthbarObject.isActiveAndEnabled)
        {
            // Todo: 플레이어 캐릭터 적절한 위치
            _healthbarObject = HealthbarFactory.Instance.Show(this, GetAbility<AnimationAbility>().Height);
        }
    }


    protected override void OnTakeHit(double damage, Enum_DamageType damageType)
    {
        GetAbility<PlayerSkillAbility>().OnHit();
        
        GetAbility<MeshRendererAbility>().Flash();

        var animationAbility = GetAbility<AnimationAbility>();
        var height = animationAbility.Height;
        var width = animationAbility.Width;

        // TODO: 1. Damage Text
        DamageTextFactory.Instance.Show(transform.position, height, width, damage, damageType);

        // TODO: 2. Hit VFX
        HitVFXFactory.Instance.Show(transform.position, height, width, damageType);
    }

    public override void OnAttack(CharacterObject target, double damage, Enum_DamageType damageType)
    {
        GetAbility<PlayerSkillAbility>().OnAttack(target);

        if (target.IsDeath)
        {
            GetAbility<PlayerSkillAbility>().OnEnemyKill();
        }
    }

    protected override void OnDeath()
    {
        _fsmAbility.ChangeState(Enum_PlayerStateType.Death);
    }


    private void RefreshSkillAbility()
    {
        GetAbility<PlayerSkillAbility>().RefreshSkill();
    }

    public void OnGameEvent(StatEvent e)
    {
        if (e.Type == Enum_StatEventType.StatCalculate)
        {
            CalculateStat();
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    // 동료

    public void OnGameEvent(FollowerEvent e)
    {
        RefreshFollowers();
    }

    private void RefreshFollowers()
    {
        for (var i = 0; i < _followers.Count; i++)
        {
            //_followers[i].ChangeFollowerModel(DataManager.FollowerData.EquippedIndex[i]);
        }
    }

    public void SetFollowersMove(float moveScale)
    {
        for (var i = 0; i < _followers.Count; i++)
        {
            _followers[i].PlayMoveAnimation(moveScale);
        }
    }
        
    public void SetFollowersIdle()
    {
        for (var i = 0; i < _followers.Count; i++)
        {
            _followers[i].PlayIdleAnimation();
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    //  무기 스킨

    public void OnGameEvent(WeaponEvent e)
    {
        RefreshWeaponSkin();
    } 
    
    private void RefreshWeaponSkin()
    {
        GetAbility<MultiSkinAnimationAbility>().RefreshSkins();
    }
}