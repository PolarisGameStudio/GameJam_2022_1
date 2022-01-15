using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class NoWaveBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")]
    private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] 
    private Vector3 _startCameraPosition;
    
    [SerializeField] [Header("몬스터 생성 거리")]
    private float _monsterSpawnOffsetX;    
    
    [SerializeField] [Header("보스 몬스터 생성 거리")]
    private float _bossMonsterDistanceX;
    
    private SpecStageWave _stageSpecData;
    public string StageName => _stageSpecData.stageName;
    
    // 보스 남은 시간
    public float MaxBossRemainTime => 30; // TODO : 여기 더미
    private float _currentBossRemainTime;
    public float CurrentBossRemainTime => _currentBossRemainTime;
    
    // 보스 게이지
    public int RequireBossGauge => 0; // TODO : 여기 더미 //_stageData.RequireBossGauge;
    private int _currentBossGauge = 0;
    public int CurrentBossGauge => _currentBossGauge;
    public bool IsEnableBossChallenge => _currentBossGauge >= RequireBossGauge;
    
    // 보스 도전중
    private bool _bossTime = false;
    public bool IsBossTime => _bossTime;
    
    public const int MAX_WAVE_COUNT = 3;
    public const int MAX_WAVE_INDEX_COUNT = 10;

    public int MinSpawnCount = 1;
    public int MaxSpawnCount = 6;

    public int MaxActiveMonsterCount = 50;
    
    // todo: 외부로 빼기
    public float SpawnPeriod = 0.5f;
    private float _spawnTimer = 0;
    public int SpecialMonserSpawnRequireCount = 100;
    private int _monsterSpawnCount = 0;

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

        _currentBossGauge = 0;

        _bossTime = false;

        //todo: 웨이브 없이 스테이지 정보만 가져와야함
        _stageSpecData = DataSpecContainer.InstanceSpecStageWave[0];
        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _player = BattleManager.Instance.PlayerObject;
        _inited = true;
    }
    
    protected override void OnBattleStart()
    {
        _monsterSpawnCount = 0;
        _spawnTimer = 0;
        
        BattleCamera.Instance.SetPosition(_startCameraPosition);
        
        // if (OptionManager.Instance.IsAutoBossChallenge)
        // {
        //     TryBossChallenge();
        // }
    }
    
    public void OnGameEvent(MonsterEvent e)
    {
        if (BattleManager.Instance.CurrentBattleType != Enum_BattleType.Stage)
        {
            return;
        }

        switch (e.Type)
        {
            case Enum_MonsterEventType.NormalMonsterDeath:
            {
                OnNormalMonsterDeath();
                break;
            }
            
            case Enum_MonsterEventType.BossMonsterDeath:
            {
                OnBossMonsterDeath();
                break;
            }
        }
    }

    private void OnNormalMonsterDeath()
    {
        _currentBossGauge += 1;
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
        
        // Todo: 공식 적용
        // CurrencyManager.Instance.AddGold(10000);
        // UserManager.Instance.AddExp(10);

    //     if (OptionManager.Instance.IsAutoBossChallenge)
    //     {
    //         TryBossChallenge();
    //     }
     }

    private void OnBossMonsterDeath()
    {
        // Todo: 암막처리하고 뭐하고 뭐하고..
        
        // Todo: 공식 적용
        // CurrencyManager.Instance.AddGold(50);
        // UserManager.Instance.AddExp(100);

        BattleClear();
    }

    private void SpawnMonster()
    {
        _spawnTimer = 0;
        
        if (_bossTime)
        {
            return;
        }
        
        Vector3 objPosition = _player.Position + Vector3.right * _monsterSpawnOffsetX;

        var spawnCount = UnityEngine.Random.Range(MinSpawnCount, MaxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++) 
        {
            var activeMonsterCount = _monsterObjects.FindAll(monster => monster.IsAlive).Count;

            if (activeMonsterCount >= MaxActiveMonsterCount)
            {
                break;
            }

            //todo: 몬스터 인덱스 
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, objPosition, 0,BattleType, _monsterSpawnCount % SpecialMonserSpawnRequireCount == 0);
            
            _monsterObjects.Add(obj);
            _monsterSpawnCount++;
        }
    }

    protected override void OnBattleClear()
    {
        BattleManager.Instance.BattleClear(Enum_BattleType.Stage, _level);
        BattleCamera.Instance.SetOrthographicSizeSmooth(BattleCamera.Instance.OriginOrthographicSize,0.03f);
    }

    protected override void OnBattleOver()
    {
        PlayerStatManager.Instance.InitHealth();
        
        BattleManager.Instance.BattleStart(Enum_BattleType.Stage, _level);
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
        //throw new System.NotImplementedException();
        
        // Todo: 테스트용
        BattleCamera.Instance.ResetOrthographicSizeSmooth(1f);
    }


    public bool TryBossChallenge()
    {
        if (_bossTime)
            return false;
        
        if (!IsEnableBossChallenge)
        {
            return false;
        }

        // Todo: 테스트용
        BattleCamera.Instance.SetOrthographicSizeSmooth(BattleCamera.Instance.OriginOrthographicSize * 1.25f, 1f);
        
        _currentBossRemainTime = MaxBossRemainTime;
        
        _bossTime = true;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();
        BattleManager.Instance.PlayerObject.RefreshHealthbar();
        
        _monsterObjects.Clear();

        var bossSpawnPosition = BattleManager.Instance.PlayerObject.Position;
        bossSpawnPosition.x += _bossMonsterDistanceX;

        var bossMonster = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageBossMonster, bossSpawnPosition, _stageSpecData.BossMonsterIndex, BattleType);
        
        _monsterObjects.Add(bossMonster);
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
        
        return true;
    }

    private void Update()
    {
        var dt = Time.deltaTime;

        if (_bossTime)
        {
            _currentBossRemainTime -= dt;

            if (_currentBossRemainTime <= 0)
            {
                BattleManager.Instance.PlayerObject.Death();

                _bossTime = false;
            }
        }
        else
        {
            _spawnTimer += dt;

            if (_spawnTimer >= SpawnPeriod)
            {
                SpawnMonster();
            }
        }
    }
}
