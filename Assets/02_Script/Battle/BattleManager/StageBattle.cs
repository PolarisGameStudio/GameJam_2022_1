using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class StageBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")]
    private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] 
    private Vector3 _startCameraPosition;
    
    [SerializeField] [Header("스테이지 웨이브들")]
    private List<StageWave> _stageWaves;

    [SerializeField] [Header("웨이브 시작 위치")] 
    private Vector2 _waveStartPosition;
    
    [SerializeField] [Header("웨이브 오프셋(x)")] 
    private float _waveOffsetX;

    [SerializeField] [Header("보스 몬스터 생성 거리")]
    private float _bossMonsterDistanceX;
    
    private int _waveLevel = 0;
    public int WaveLevel => _waveLevel;

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

    public int SpawnCount = 6;

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
        
        _waveLevel = 0;

        _currentBossGauge = 0;

        _bossTime = false;

        _stageSpecData = DataSpecContainer.InstanceSpecStageWave[Math.Min(_level, DataSpecContainer.InstanceSpecStageWave.Count - 1)];
        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        var wavePosition = _waveStartPosition;
        for (int i = 0; i < _stageWaves.Count; ++i)
        {
            _stageWaves[i].SafeSetActive(false);
            _stageWaves[i].SetLevel(i);
            _stageWaves[i].transform.position = wavePosition;
            _stageWaves[i].SafeSetActive(true);

            wavePosition.x += _waveOffsetX;
        }
        
        _inited = true;
    }
    
    protected override void OnBattleStart()
    {
        BattleCamera.Instance.SetPosition(_startCameraPosition);
        
        // if (OptionManager.Instance.IsAutoBossChallenge)
        // {
        //     TryBossChallenge();
        // }
    }

    public void OnWaveEnter(int waveLevel)
    {
        if (_bossTime || !_inited)
        {
            return;
        }
        
        _waveLevel = waveLevel;
        // Debug.Log($"스테이지{_stageSpecData.stageName} 웨이브({waveLevel}) 입장");

        _monsterObjects.Clear();

        // TODO : 웨이브 갯수 넘어가면 마지막 웨이브 반복? 임의로 처리 
        int waveIndex = _waveLevel < _stageSpecData.WaveMonsterGroupDataList.Count - 1 ? _waveLevel : _stageSpecData.WaveMonsterGroupDataList.Count - 1; 
        int[] waveMonsterIndexes = _stageSpecData.WaveMonsterGroupDataList[waveIndex];
        for (int i = 0; i < SpawnCount; i++) // TODO : 스폰 포인트 3개만 있음. 3개 이상 처리하려면 추가 작업 필요
        {
            Vector3 objPosition = _stageWaves[_waveLevel % MAX_WAVE_COUNT].GetSpawnPosition(i & MAX_WAVE_COUNT);

            // TODO : 임의로 데이터 테이블에 3개 미만으로 설정 되었다면 마지막 인덱스로 3개 채움
            int tempIdx = i;
            if (waveMonsterIndexes.Length - 1 < tempIdx)
                tempIdx = waveMonsterIndexes.Length - 1;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, objPosition,
                waveMonsterIndexes[tempIdx], BattleType);
            _monsterObjects.Add(obj);
        }
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
    }

    public void OnWaveExit(int waveLevel)
    {
        if (_bossTime || !_inited)
        {
            return;
        }
        
        //Debug.Log($"스테이지 웨이브({waveLevel}) 퇴장");

        var exitedWave = _stageWaves[waveLevel % MAX_WAVE_COUNT];
        exitedWave.SetLevel(waveLevel + MAX_WAVE_COUNT);
        var newPosition = _waveStartPosition;
        newPosition.x += exitedWave.WaveLevel * _waveOffsetX;
        exitedWave.transform.position = newPosition;
                
        RefreshEvent.Trigger(Enum_RefreshEventType.Battle);
        
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

        // if (OptionManager.Instance.IsAutoBossChallenge)
        // {
        //     TryBossChallenge();
        // }
    }

    private void OnBossMonsterDeath()
    {
        // Todo: 암막처리하고 뭐하고 뭐하고..
        
        // Todo: 공식 적용
        // CurrencyManager.Instance.AddGold(50);
        // UserManager.Instance.AddExp(100);

        BattleClear();
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

    // 웨이브 레벨에 따라 몇 번째 몬스터 그룹 인덱스를 내보낼건지
    private int GetWaveGroupIndex()
    {
        // TODO: 협의가 안되었으므로 일단 순환 반복
        return _waveLevel % MAX_WAVE_INDEX_COUNT;
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
    }
}
