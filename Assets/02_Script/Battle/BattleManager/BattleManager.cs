
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BattleManager : SingletonBehaviour<BattleManager>
{
    private Enum_BattleType _currentBattleType = Enum_BattleType.None;
    public Enum_BattleType CurrentBattleType => _currentBattleType;

    private Battle[] _battles;
    private bool _battleCached = false;

    [SerializeField][Header("플레이어 오브젝트")] private PlayerObject _playerObject;

    public PlayerObject PlayerObject => _playerObject;

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

        _playerObject = FindObjectOfType<PlayerObject>();

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

        
        BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);
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
        
        StopAllCoroutines();

        CurrentBattleEnd();

        _currentBattleType = battleType;

        switch (battleType)
        {
            case Enum_BattleType.Stage:
                SoundManager.Instance.PlayBackground("BGM_Stage");
                break;
            case Enum_BattleType.StageBoss:
            case Enum_BattleType.PromotionBattle:
            case Enum_BattleType.TreasureDungeon:
            case Enum_BattleType.SmithDungeon:
            case Enum_BattleType.BossDungeon:
                SoundManager.Instance.PlayBackground("BGM_Boss");
                break;
        }

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
        
        DataManager.Instance.Save();
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
    public async void BattleClear(Enum_BattleType battleType, int battleLevel)
    {
        BattleEvent.Trigger(Enum_BattleEventType.BattleClear, Enum_BattleType.Stage);

        StartCoroutine(StageChangeCoroutine(battleType, battleLevel));
    }

    IEnumerator StageChangeCoroutine(Enum_BattleType battleType, int battleLevel)
    {
        yield return new WaitForSecondsRealtime(1);

        switch (battleType)
        {
            case Enum_BattleType.None:
                Debug.LogError("None 타입 배틀 호출");
                BattleStart(battleType, DataManager.StageData.StageLevel);
                break;
            case Enum_BattleType.Stage:
                BattleStart(battleType, DataManager.StageData.StageLevel);
                break;       
            case Enum_BattleType.StageBoss:
                DataManager.StageData.BossStageClear();
                BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);
                break;
            case Enum_BattleType.PromotionBattle:
                DataManager.PromotionData.OnClearPromotionBattle(battleLevel);
                UI_Popup_OK.Instance.Open("승급 성공!", $"{TBL_PROMOTION.GetEntity(battleLevel).name} 승급에 성공했습니다.");
                BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);
                break;
            
            case Enum_BattleType.SmithDungeon:
                DataManager.DungeonData.OnDungeonBattleEnd(battleType, battleLevel);
                BattleStart(Enum_BattleType.Stage, DataManager.StageData.StageLevel);
                break;

            case Enum_BattleType.TreasureDungeon:
            case Enum_BattleType.BossDungeon:
                Debug.LogError($"{battleType}얘네는 Clear 호출하면 안됨");
                break;
        }
    }
}