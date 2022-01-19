using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDungeonBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")] private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] private Vector3 _startCameraPosition;

    [SerializeField] [Header("웨이브 오프셋(x)")]
    private float _waveOffsetX;

    [SerializeField] [Header("몬스터 오프셋(x)")]
    private float _monsterOffestX;

    private TBL_DUNGEON_BOSS _bossDungeonData;

    private bool _inited = false;
    public bool IsInited => _inited;

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

        _bossDungeonData = TBL_DUNGEON_BOSS.GetEntity(0);

        DamageFactor = _bossDungeonData.DamageFactor * 10;
        HealthFactor = _bossDungeonData.HealthFactor * 10;
        
        _inited = true;
    }

    private void SpawnBossMonster()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        Debug.LogError("보스던전 몬스터 정해줘야함!!!!!!!!!");
        // int monsterIndex = SystemValue.BossDungeonMonsterIndex;
        int monsterIndex = 1;

        var spawnPosition = objPosition;
        MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.BossDungeonMonster, spawnPosition,
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
        //PlayerStatManager.Instance.InitHealth();

        BattleManager.Instance.BattleStart(Enum_BattleType.Stage, _level);
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