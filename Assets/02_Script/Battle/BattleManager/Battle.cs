using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battle : MonoBehaviour
{
    [SerializeField] [Header("전투 타입")] protected Enum_BattleType _battleType;
    public Enum_BattleType BattleType => _battleType;
    
    [SerializeField] [Header("캐릭터 시작 위치")] protected Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] protected Vector3 _startCameraPosition;

    protected int _level;
    public int Level => _level;

    protected List<CharacterObject> _monsterObjects = new List<CharacterObject>(3);
    public List<CharacterObject> MonsterObjects => _monsterObjects;

    [SerializeField] [Header("웨이브 오프셋(x)")]  protected float _waveOffsetX = 10;
    [SerializeField] [Header("몬스터 오프셋(x)")]  protected float _monsterOffestX = 0.3f;
    
    protected int waveLevel = 0;


    [NonSerialized] public double DamageFactor;
    [NonSerialized] public double HealthFactor;

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
    
    protected abstract void OnMonsterDeathReward();

    public abstract string GetBattleTitle();

    public virtual float GetProgress()
    {
        return 1;
    }
}