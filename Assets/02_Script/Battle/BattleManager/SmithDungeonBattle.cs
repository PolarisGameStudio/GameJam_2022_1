using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithDungeonBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_DUNGEON_SMITH _smithDungeonData;
    

    public float RemainTime => SystemValue.SMITH_DUNGEON_LIMIT_TIME - _timer;
    private float _timer;
    
    
    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }
    
    private void Update()
    {
        if (_smithDungeonData == null || BattleManager.Instance.CurrentBattle != this)
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


    protected override void InitBattleData()
    {
        _level = Mathf.Min(TBL_DUNGEON_SMITH.CountEntities - 1, _level);

        _smithDungeonData = TBL_DUNGEON_SMITH.GetEntity(_level);

        DamageFactor = _smithDungeonData.DamageFactor;
        HealthFactor = _smithDungeonData.HealthFactor; 
    }

    protected override void OnBattleInit()
    {
        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _timer = 0;

        _inited = true;
    }

    private void SpawnWaveMonsters()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        var spawnCount = _smithDungeonData.WaveMonsterCount;

        for (int i = 0; i < spawnCount; i++)
        {
            int monsterIndex = _smithDungeonData.SpawnMonsterList[(Random.Range(0, _smithDungeonData.SpawnMonsterList.Count))];

            var spawnPosition = objPosition + (i * _monsterOffestX) * Vector3.right;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, spawnPosition,
                monsterIndex, _battleType);

            _monsterObjects.Add(obj);
        }
    }

    protected override void OnBattleStart()
    {
        waveLevel = 0;

        BattleCamera.Instance.SetPosition(_startCameraPosition);

        SpawnWaveMonsters();
    }

    protected override void OnBattleClear()
    {
        BattleManager.Instance.BattleClear(Enum_BattleType.SmithDungeon, _level);
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
        throw new System.NotImplementedException();
    }

    public override string GetBattleTitle()
    {
        return $"좀비 대장간 {_level+1}층";
    }

    public override float GetProgress()
    {
        return waveLevel / (float) _smithDungeonData.WaveCount;
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
                CheckWaveClear();
                break;
            }
        }
    }

    private void OnWaveClear()
    {
        waveLevel++;
        _monsterObjects.Clear();

        if (waveLevel >= _smithDungeonData.WaveCount)
        {
            BattleClear();
        }
        else
        {
            SpawnWaveMonsters();
        }
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }


    private void CheckWaveClear()
    {
        if (_monsterObjects.Find(monster => monster.IsAlive) == null)
        {
            OnWaveClear();
        }
    }
}