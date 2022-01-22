using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MonsterObject : CharacterObject
{
    private Monster _monster;
    public Monster Monster => _monster;

    protected FSMAbility _fsmAbility;
    protected bool _fsmInited = false;
    
    protected double _damageHit; // 보스 던전용 변수

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

    public void Init(Enum_CharacterType characterType, Vector3 initPosition, Monster monster,
        SkeletonDataAsset skeletonDataAsset, AttackPreset attackPreset)
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

        _fsmAbility.ChangeState(Enum_MonsterStateType.Idle);

        _damageHit = 0;

        SafeSetActive(true);
    }

    private void InitScale()
    {
        switch (CharacterType)
        {
            case Enum_CharacterType.StageNormalMonster:
                Model.localScale = new Vector3(1f, 1f, 1f);
                break;

            case Enum_CharacterType.StageBossMonster:
                Model.localScale = new Vector3(2f, 2f, 1);
                break;
        }
    }

    protected override void OnTakeHit(double damage, Enum_DamageType damageType)
    {
        GetAbility<MeshRendererAbility>().Flash();

        var animationAbility = GetAbility<AnimationAbility>();
        var height = animationAbility.Height;
        var width = animationAbility.Width;

        _damageHit += damage;

        // TODO: 1. Damage Text
        DamageTextFactory.Instance.Show(transform.position, height, width, damage, damageType);

        // TODO: 2. Hit VFX
        HitVFXFactory.Instance.Show(transform.position, height, width, damageType);

        // 넉백
        GetAbility<KnockbackAbility>().Knockback();
    }

    protected override void OnDeath()
    {
        // 보스던전 몬스터는 안주거요
        if (_characterType == Enum_CharacterType.BossDungeonMonster)
        {
            _alive = true;
        }
        else
        {
            _fsmAbility.ChangeState(Enum_MonsterStateType.Death);
        }

        //
        // DataManager.CurrencyData.Add(Enum_CurrencyType.Gold, BattleManager.Instance.CurrentBattle.GoldAmount);
        // DataManager.PlayerData.AddExp(BattleManager.Instance.CurrentBattle.ExpAmount);

        switch (_characterType)
        {
            case Enum_CharacterType.StageNormalMonster:
                MonsterEvent.Trigger(Enum_MonsterEventType.NormalMonsterDeath);
                break;

            case Enum_CharacterType.StageBossMonster:
                MonsterEvent.Trigger(Enum_MonsterEventType.BossMonsterDeath);
                break;
            
            case Enum_CharacterType.BossDungeonMonster:
                MonsterEvent.Trigger(Enum_MonsterEventType.BossMonsterDeath);
                DataManager.DungeonData.RecordDungeonScore(Enum_BattleType.BossDungeon, _damageHit);
                break;

            default:
                MonsterEvent.Trigger(Enum_MonsterEventType.NormalMonsterDeath);
                break;
        }
    }
}