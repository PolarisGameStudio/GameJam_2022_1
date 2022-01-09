
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BattleManager : SingletonBehaviour<BattleManager>
{
    private Enum_BattleType _currentBattleType = Enum_BattleType.None;
    public Enum_BattleType CurrentBattleType => _currentBattleType;

    private Battle[] _battles;
    private bool _battleCached = false;

    [SerializeField] [Header("플레이어 오브젝트")] private PlayerObject _playerObject;

    public PlayerObject PlayerObject => _playerObject;

    private int _battleID = 0;
    public int BattleID => _battleID;

    public Battle CurrentBattle
    {
        get
        {
            foreach (Battle battle in _battles)
            {
                if (battle.BattleType == _currentBattleType)
                {
                    return battle;
                }
            }

            return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        CacheBattles();
    }

    private void CacheBattles()
    {
        _battles = FindObjectsOfType<Battle>();

        _battleCached = true;
    }

    public T GetBattle<T>() where T : Battle
    {
        if (!_battleCached)
        {
            CacheBattles();
        }

        foreach (Battle battle in _battles)
        {
            if (battle is T findBattle)
            {
                return findBattle;
            }
        }

        return null;
    }

    private void Start()
    {
        // TODO: 씬 스크립트로 옮겨야 함
        Application.targetFrameRate = 60;


        // TODO: 시작 씬에 따라 스테이지 or 툴
        // 프로토타입이라서 일단 여기서 제어

        if (SceneManager.GetActiveScene().name.Contains("Tool"))
        {
            BattleStart(Enum_BattleType.Tool, 0);
        }
        else
        {
            BattleStart(Enum_BattleType.Stage, 0);
        }
    }

    public List<CharacterObject> GetMonsters()
    {
        if (_currentBattleType == Enum_BattleType.None)
        {
            return null;
        }

        return CurrentBattle.MonsterObjects;
    }

    public void BattleStart(Enum_BattleType battleType, int level)
    {
        //SaveManager.Save();

        _battleID += 1;

        CurrentBattleEnd();

        _currentBattleType = battleType;

        CurrentBattleInitAndStart(level);
    }

    public void BattleOver()
    {
        CurrentBattleOver();
    }

    private void CurrentBattleEnd()
    {
        if (_currentBattleType == Enum_BattleType.None)
        {
            return;
        }

        CurrentBattle.BattleEnd();

        BattleEvent.Trigger(Enum_BattleEventType.BattleEnd, _currentBattleType);

        _currentBattleType = Enum_BattleType.None;

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    private void CurrentBattleInitAndStart(int level)
    {
        if (_currentBattleType == Enum_BattleType.None)
        {
            return;
        }

        CurrentBattle.BattleInit(level);

        // BattleEvent.Trigger(Enum_BattleEventType.BattleInit, _currentBattleType);

        CurrentBattle.BattleStart();

        BattleEvent.Trigger(Enum_BattleEventType.BattleStart, _currentBattleType);

        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    private void CurrentBattleOver()
    {
        if (_currentBattleType == Enum_BattleType.None)
        {
            return;
        }

        CurrentBattle.BattleOver();

        BattleEvent.Trigger(Enum_BattleEventType.BattleOver, _currentBattleType);
    }
    
    //todo: 반복이 아닌 던전 or 기타컨텐츠에서는 Stage Start 호출
    // 던전 별 클리어 연출이 다르다면, 각 Battle 스크립트에서 처리 후
    // 해당 메소드 수정
    public async void BattleClear(Enum_BattleType battleType, int level)
    {
        BattleEvent.Trigger(Enum_BattleEventType.BattleClear, Enum_BattleType.Stage);

        await Task.Delay(TimeSpan.FromSeconds(3));
        
        switch (battleType)
        {
            case Enum_BattleType.None:
                break;
            case Enum_BattleType.Stage:
                BattleStart(battleType, level + 1);
                break;
            case Enum_BattleType.Dungeon:
                break;
            case Enum_BattleType.Tool:
                break;
            case Enum_BattleType.Count:
                break;
            default:
                break;
        }
        
    }
}