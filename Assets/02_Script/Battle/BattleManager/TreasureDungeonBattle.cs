using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureDungeonBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")] private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] private Vector3 _startCameraPosition;

    [SerializeField] [Header("웨이브 오프셋(x)")]
    private float _waveOffsetX;

    [SerializeField] [Header("몬스터 오프셋(x)")]
    private float _monsterOffestX;

    private TBL_DUNGEON_TREASURE _treasureDungeonData;

    private bool _inited = false;
    public bool IsInited => _inited;

    public float RemainTime => _treasureDungeonData.TimeLimit - _timer;
    private float _timer;

    private int _monsterKillCount = 0;

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (RemainTime < 0)
        {
            BattleOver();
        }
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _inited = true;
    }

    protected override void InitBattleData()
    {
        _treasureDungeonData = TBL_DUNGEON_TREASURE.GetEntity(_level);

        DamageFactor = _treasureDungeonData.DamageFactor;
        HealthFactor = _treasureDungeonData.HealthFactor;

        GoldAmount = _treasureDungeonData.GoldAmount;
        ExpAmount = _treasureDungeonData.ExpAmount;
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
    }

    protected override void OnBattleOver()
    {
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
        DataManager.DungeonData.RecordDungeonScore(_battleType, _monsterKillCount);
        DataManager.DungeonData.OnDungeonBattleEnd(_battleType, _level);
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
        _level++;
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
}