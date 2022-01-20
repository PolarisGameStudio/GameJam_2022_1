using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageBattle : Battle, GameEventListener<MonsterEvent>
{
    [SerializeField] [Header("캐릭터 시작 위치")] private Transform _startTransform;

    [SerializeField] [Header("카메라 시작 위치")] private Vector3 _startCameraPosition;

    [SerializeField] [Header("웨이브 오프셋(x)")]  private float _waveOffsetX;
    [SerializeField] [Header("몬스터 오프셋(x)")]  private float _monsterOffestX;

    private TBL_STAGE _stageData;


    private int waveLevel = 0;

    public float StageProcess => waveLevel / (float) _stageData.WaveCount;
    public string StageTitle => $"{_stageData.World.name} {_stageData.Index % 20 + 1}";

    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    protected override void InitBattleData()
    {
        _level = Mathf.Min(TBL_STAGE.CountEntities - 1, _level);
        
        _stageData = TBL_STAGE.GetEntity(_level);

        DamageFactor = _stageData.DamageFactor;
        HealthFactor = _stageData.HealthFactor;
        GoldAmount = _stageData.Gold;
        ExpAmount = _stageData.Exp;
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();

        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _inited = true;
    }

    private void SpawnWaveMonsters()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        var spawnCount = _stageData.WaveMonsterCount;

        for (int i = 0; i < spawnCount; i++)
        {
            int monsterIndex = _stageData.SpawnMonsterIndex[(Random.Range(0, _stageData.SpawnMonsterIndex.Count))];

            var spawnPosition = objPosition + (i * _monsterOffestX) * Vector3.right;
            MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageNormalMonster, spawnPosition,
                monsterIndex, _battleType, (i == spawnCount - 1) && waveLevel == _stageData.WaveCount - 1);

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
        BattleManager.Instance.BattleClear(Enum_BattleType.Stage, _level);
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

        DataManager.CurrencyData.Add(Enum_CurrencyType.Gold, _stageData.Gold * goldMultiply);
        DataManager.PlayerData.AddExp(_stageData.Exp * expMultiply);
        
        if (UtilCode.GetChance(_stageData.UpgradeStonePercent))
        {
            DataManager.CurrencyData.Add(Enum_CurrencyType.EquipmentStone,
                BattleManager.Instance.CurrentBattle.StoneAmount);
        }
        
        if (UtilCode.GetChance(_stageData.EquipmentPercent))
        {
            int random = Random.Range(0, (int) Enum_EquipmentType.Count);
            Enum_EquipmentType equipmentType = (Enum_EquipmentType) random;

            GachaType gachaType = equipmentType == Enum_EquipmentType.Ring ? GachaType.Ring : GachaType.Weapon;

            var equipmentIndex = GachaManager.Instance.GachaByGrade(gachaType, _stageData.EquipmentGrade);

            DataManager.EquipmentData.AddEquipment(equipmentIndex, 1);
        }
        
        //아이템 로그 여기서 찍기
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
                OnMonsterDeathReward();
                CheckWaveClear();
                break;
            }
        }
    }

    private void OnWaveClear()
    {
        waveLevel++;
        _monsterObjects.Clear();

        if (waveLevel >= _stageData.WaveCount)
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
