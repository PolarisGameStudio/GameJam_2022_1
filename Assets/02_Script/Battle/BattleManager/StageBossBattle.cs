using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageBossBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")] private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] private Vector3 _startCameraPosition;

    [SerializeField] [Header("웨이브 오프셋(x)")]  private float _waveOffsetX;
    [SerializeField] [Header("몬스터 오프셋(x)")]  private float _monsterOffestX;

    private TBL_STAGE _stageData;


    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    protected override void InitBattleData()
    {
        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _stageData = TBL_STAGE.GetEntity(_level);

        DamageFactor = _stageData.DamageFactor * 10;
        HealthFactor = _stageData.HealthFactor * 10;
        GoldAmount = _stageData.Gold * 10;
        ExpAmount = _stageData.Exp * 10;
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();


        _inited = true;
    }

    private void SpawnBossMonster()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        int monsterIndex = _stageData.BossMonsterIndex;

        var spawnPosition = objPosition;
        MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageBossMonster, spawnPosition,
            monsterIndex, Enum_BattleType.StageBoss);

        _monsterObjects.Add(obj);
    }

    protected override void OnBattleStart()
    {
        BattleCamera.Instance.SetPosition(_startCameraPosition);

        SpawnBossMonster();
    }

    protected override void OnBattleClear()
    {
        BattleManager.Instance.BattleClear(_battleType, _level);
    }

    protected override void OnBattleOver()
    {
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
    }

    protected override void OnMonsterDeathReward()
    {
        var goldMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Gold) ? 2 : 1;
        var expMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Exp) ? 2 : 1;

        DataManager.CurrencyData.Add(Enum_CurrencyType.Gold,
            BattleManager.Instance.CurrentBattle.GoldAmount * goldMultiply);
        DataManager.PlayerData.AddExp(BattleManager.Instance.CurrentBattle.ExpAmount * expMultiply);
        

        if (UtilCode.GetChance(10))
        {
            DataManager.CurrencyData.Add(Enum_CurrencyType.EquipmentStone,
                BattleManager.Instance.CurrentBattle.StoneAmount);
        }
        //아이템 로그 여기서 찍기
    }

    public void OnGameEvent(MonsterEvent e)
    {
        if (BattleManager.Instance.CurrentBattleType != _battleType)
        {
            return;
        }

        if (e.Type == Enum_MonsterEventType.BossMonsterDeath)
        {
            CheckWaveClear();
        }
    }

    private void OnWaveClear()
    {
        _monsterObjects.Clear();
        
        BattleClear();
    }
    

    private void CheckWaveClear()
    {
        if (_monsterObjects.Find(monster => monster.IsAlive) == null)
        {
            OnWaveClear();
        }
    }

}
