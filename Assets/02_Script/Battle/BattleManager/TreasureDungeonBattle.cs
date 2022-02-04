using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureDungeonBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_DUNGEON_TREASURE _treasureDungeonData;

    public float RemainTime => _treasureDungeonData.TimeLimit - _timer;
    private float _timer;

    private int _monsterKillCount = 0;

    public override string GetBattleTitle()
    {
        return "보물고";
    }

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    private void Update()
    {
        if (_treasureDungeonData == null || BattleManager.Instance.CurrentBattle != this)
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
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _timer = 0;
        _inited = true;
    }

    protected override void InitBattleData()
    {
        _treasureDungeonData = TBL_DUNGEON_TREASURE.GetEntity(_level);

        DamageFactor = _treasureDungeonData.DamageFactor;
        HealthFactor = _treasureDungeonData.HealthFactor;
    }

    private void SpawnWaveMonsters()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        var spawnCount = _treasureDungeonData.WaveMonsterCount;

        for (int i = 0; i < spawnCount; i++)
        {
            int monsterIndex =
                _treasureDungeonData.SpawnMonsterList[(Random.Range(0, _treasureDungeonData.SpawnMonsterList.Count))];

            var spawnPosition = objPosition + (i * _monsterOffestX) * Vector3.right;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, spawnPosition,
                monsterIndex, _battleType);

            _monsterObjects.Add(obj);
        }
    }

    protected override void OnBattleStart()
    {
        BattleCamera.Instance.SetPosition(_startCameraPosition);

        SpawnWaveMonsters();
    }

    protected override void OnBattleClear()
    {
        DataManager.DungeonData.RecordDungeonScore(_battleType, _monsterKillCount);
        DataManager.DungeonData.OnDungeonBattleEnd(_battleType, _level);
    }

    protected override void OnBattleOver()
    {
        DataManager.DungeonData.RecordDungeonScore(_battleType, _monsterKillCount);
        DataManager.DungeonData.OnDungeonBattleEnd(_battleType, _level);
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
    }

    protected override void OnMonsterDeathReward()
    {
        throw new System.NotImplementedException();
    }

    public void OnGameEvent(MonsterEvent e)
    {
        if (BattleManager.Instance.CurrentBattleType != _battleType)
        {
            return;
        }

        switch (e.Type)
        {
            case Enum_MonsterEventType.NormalMonsterDeath:
            {
                _monsterKillCount++;
                CheckWaveClear();
                break;
            }
        }
    }

    private void OnWaveClear()
    {
        _level = Mathf.Min(_level + 1, TBL_DUNGEON_TREASURE.CountEntities - 1);
        _timer = 0;
        InitBattleData();
        _monsterObjects.Clear();

        SpawnWaveMonsters();
    }


    private void CheckWaveClear()
    {
        if (_monsterObjects.Find(monster => monster.IsAlive) == null)
        {
            OnWaveClear();
        }
    }

    public override float GetProgress()
    {
        return RemainTime / _treasureDungeonData.TimeLimit;
    }
}