using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle : MonoBehaviour
{
    [SerializeField] [Header("전투 타입")] protected Enum_BattleType _battleType;
    public Enum_BattleType BattleType => _battleType;

    protected int _level;
    public int Level => _level;

    protected List<CharacterObject> _monsterObjects = new List<CharacterObject>(3);
    public List<CharacterObject> MonsterObjects => _monsterObjects;

    public double DamageFactor;
    public double HealthFactor;
    public double GoldAmount;
    public double ExpAmount;

    protected PlayerObject _player;


    protected bool _inited = false;
    public bool IsInited => _inited;

    public virtual void BattleInit(int level)
    {
        _level = level;

        _monsterObjects.Clear();

        _inited = false;

        InitBattleData();
        OnBattleInit();

        _player = BattleManager.Instance.PlayerObject;

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    protected abstract void InitBattleData();
    protected abstract void OnBattleInit();


    public virtual void BattleStart()
    {
        OnBattleStart();

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    protected abstract void OnBattleStart();


    public virtual void BattleClear()
    {
        OnBattleClear();

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    protected abstract void OnBattleClear();


    public virtual void BattleOver()
    {
        OnBattleOver();
        BattleManager.Instance.BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    protected abstract void OnBattleOver();


    public virtual void BattleEnd()
    {
        MonsterObjectFactory.Instance.HideAll();
        MonsterObjectFactory.Instance.ClearPool();

        _monsterObjects.Clear();

        OnBattleEnd();

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    protected abstract void OnBattleEnd();
}