using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDungeonBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_DUNGEON_BOSS _bossDungeonData;
    public float RemainTime => SystemValue.BOSS_DUNGEON_LIMIT_TIME - _timer;
    private float _timer;

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    private void Update()
    {
        if (_bossDungeonData == null || BattleManager.Instance.CurrentBattle != this)
        {
            return;
        }
        
        _timer += Time.deltaTime;      

        if (RemainTime < 0)
        {
            BattleOver();
            _timer = 0;
        }
    }

    protected override void OnBattleInit()
    {
        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _timer = 0;
        
        _inited = true;
    }

    protected override void InitBattleData()
    {
        _bossDungeonData = TBL_DUNGEON_BOSS.GetEntity(_level);

        DamageFactor = _bossDungeonData.DamageFactor;
        HealthFactor = _bossDungeonData.HealthFactor;
    }

    private void SpawnBossMonster()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        int monsterIndex = 10;

        var spawnPosition = objPosition;
        MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.BossDungeonMonster, spawnPosition,
            monsterIndex, Enum_BattleType.BossDungeon);

        _monsterObjects.Add(obj);
    }

    protected override void OnBattleStart()
    {
        BattleCamera.Instance.SetPosition(_startCameraPosition);

        SpawnBossMonster();
    }

    protected override void OnBattleClear()
    {
    }

    protected override void OnBattleOver()
    {
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
        DataManager.DungeonData.OnDungeonBattleEnd(_battleType, _level);
    }

    public void OnGameEvent(MonsterEvent e)
    {
        if (BattleManager.Instance.CurrentBattleType != _battleType)
        {
            return;
        }

        if (e.Type == Enum_MonsterEventType.BossMonsterDeath)
        {
            OnWaveClear();
        }
    }

    private void OnWaveClear()
    {
        _level = Mathf.Min(_level + 1, TBL_DUNGEON_BOSS.CountEntities - 1);
        
        InitBattleData();
    }

    protected override void OnMonsterDeathReward()
    {
    }
}