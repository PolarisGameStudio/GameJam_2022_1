using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PromotionBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")] private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] private Vector3 _startCameraPosition;

    [SerializeField] [Header("웨이브 오프셋(x)")]  private float _waveOffsetX;
    [SerializeField] [Header("몬스터 오프셋(x)")]  private float _monsterOffestX;

    private TBL_PROMOTION _promotionStageData;

    private bool _inited = false;
    public bool IsInited => _inited;

    private int waveLevel = 0;

    public float StageProcess => waveLevel / (float) _promotionStageData.WaveCount;
    public string StageTitle => $"승급 {_promotionStageData.name}";

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _level = Mathf.Min(TBL_STAGE.CountEntities - 1, _level);
        
        _promotionStageData = TBL_PROMOTION.GetEntity(_level);

        DamageFactor = _promotionStageData.DamageFactor;
        HealthFactor = _promotionStageData.HealthFactor;

        _inited = true;
    }

    private void SpawnWaveMonsters()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        if (waveLevel == _promotionStageData.WaveCount - 1)
        {
            // 보스 소환

            int monsterIndex = _promotionStageData.BossMonsterIndex;

            var spawnPosition = objPosition;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageBossMonster, spawnPosition,
                monsterIndex, _battleType);

            _monsterObjects.Add(obj);
        }
        else
        {
            var spawnCount = _promotionStageData.WaveMonsterCount;

            for (int i = 0; i < spawnCount; i++)
            {
                int monsterIndex = _promotionStageData.SpawnMonsterList[(Random.Range(0, _promotionStageData.SpawnMonsterList.Count))];

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
       // PlayerStatManager.Instance.InitHealth();

        BattleManager.Instance.BattleStart(Enum_BattleType.PromotionBattle, _level);
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
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

        if (waveLevel > _promotionStageData.WaveCount)
        {
            BattleManager.Instance.BattleClear(Enum_BattleType.PromotionBattle, _level);
        }
        else
        {
            SpawnWaveMonsters();
        }
        
        StageWaveEvent.Trigger(Enum_StageWaveEventType.Exit, waveLevel);
    }
    

    private void CheckWaveClear()
    {
        if (_monsterObjects.Find(monster => monster.IsAlive) == null)
        {
            OnWaveClear();
        }
    }

}
