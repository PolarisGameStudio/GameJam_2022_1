using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PromotionBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_PROMOTION _promotionBattleData;

    
    public float RemainTime => SystemValue.PROMOTION_BATTLE_LIMIT_TIME - _timer;
    private float _timer;
    
    public float StageProcess => waveLevel / (float) _promotionBattleData.WaveCount;
    
    
    public override string GetBattleTitle()
    {
        return $"{_promotionBattleData.name} 승급전";
    }

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }
    private void Update()
    {
        if (BattleManager.Instance.CurrentBattle != this)
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
        _level = Mathf.Min(TBL_PROMOTION.CountEntities - 1, _level);
        
        _promotionBattleData = TBL_PROMOTION.GetEntity(_level);

        DamageFactor = _promotionBattleData.DamageFactor;
        HealthFactor = _promotionBattleData.HealthFactor;
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

    private void SpawnWaveMonsters()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        if (waveLevel == _promotionBattleData.WaveCount - 1)
        {
            // 보스 소환

            int monsterIndex = _promotionBattleData.BossMonsterIndex;

            var spawnPosition = objPosition;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageBossMonster, spawnPosition,
                monsterIndex, _battleType);

            _monsterObjects.Add(obj);
        }
        else
        {
            var spawnCount = _promotionBattleData.WaveMonsterCount;

            for (int i = 0; i < spawnCount; i++)
            {
                int monsterIndex = _promotionBattleData.SpawnMonsterList[(Random.Range(0, _promotionBattleData.SpawnMonsterList.Count))];

                var spawnPosition = objPosition + (i * _monsterOffestX) * Vector3.right;
                MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, spawnPosition,
                    monsterIndex, _battleType);

                _monsterObjects.Add(obj);
            }
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
        BattleManager.Instance.BattleClear(Enum_BattleType.PromotionBattle, _level);
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
        throw new NotImplementedException();
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
            case Enum_MonsterEventType.BossMonsterDeath:
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

        if (waveLevel >= _promotionBattleData.WaveCount)
        {
            BattleClear();
        }
        else
        {
            SpawnWaveMonsters();
        }
    }
    

    private void CheckWaveClear()
    {
        if (_monsterObjects.Find(monster => monster.IsAlive) == null)
        {
            OnWaveClear();
        }
    }

}
