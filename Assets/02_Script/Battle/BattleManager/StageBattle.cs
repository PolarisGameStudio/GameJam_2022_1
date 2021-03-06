using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_STAGE _stageData;
    
    
    public override string GetBattleTitle()
    {
        return $"{_stageData.World.name} {_stageData.Index % 20 + 1}";
    }

    public override float GetProgress()
    {
        return waveLevel / (float) _stageData.WaveCount;
    }


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
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();
        
        BackgroundManager.Instance.SetBackground(_stageData.World.BackgroundType);

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
        double goldMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Gold) ? 2 : 1;
        double expMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Exp) ? 2 : 1;

        goldMultiply *= (100+ DataManager.Container.Stat[Enum_StatType.MoreGold]) / 100f;
        expMultiply *= (100+ DataManager.Container.Stat[Enum_StatType.MoreExp]) / 100f;

        DataManager.CurrencyData.Add(Enum_CurrencyType.Gold, _stageData.Gold * goldMultiply);
        DataManager.PlayerData.AddExp(_stageData.Exp * expMultiply);
        
        if (UtilCode.GetChance(_stageData.UpgradeStonePercent))
        {
            DataManager.CurrencyData.Add(Enum_CurrencyType.EquipmentStone, _stageData.UpgradeStone);
        }
        
        if (UtilCode.GetChance(_stageData.EquipmentPercent))
        {
            int random = Random.Range(0, (int) Enum_EquipmentType.Count);
            Enum_EquipmentType equipmentType = (Enum_EquipmentType) random;

            GachaType gachaType = equipmentType == Enum_EquipmentType.Ring ? GachaType.Ring : GachaType.Weapon;

            var equipmentIndex = GachaManager.Instance.GachaByGrade(gachaType, _stageData.EquipmentGrade);

            DataManager.EquipmentData.AddEquipment(equipmentIndex, 1);
        }
        
        //????????? ?????? ????????? ??????
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
