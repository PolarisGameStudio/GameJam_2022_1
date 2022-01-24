using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageBossBattle : Battle, GameEventListener<MonsterEvent>
{
    private TBL_STAGE _stageData;
    
    
    public override string GetBattleTitle()
    {
        return $"{_stageData.World.name} {_stageData.Index % 20 + 1} 보스도전";
    }
    
    private void Awake()
    {
        this.AddGameEventListening<MonsterEvent>();
    }

    protected override void InitBattleData()
    {
        BattleManager.Instance.PlayerObject.BattleStart(_startTransform.position);

        _stageData = TBL_STAGE.GetEntity(_level);

        DamageFactor = _stageData.DamageFactor * 10;
        HealthFactor = _stageData.HealthFactor * 10;
    }

    protected override void OnBattleInit()
    {
        _inited = false;

        MonsterObjectFactory.Instance.HideAll();
        HealthbarFactory.Instance.HideAll();


        _inited = true;
    }

    private void SpawnBossMonster()
    {
        Vector3 objPosition = _player.Position + Vector3.right * _waveOffsetX;

        int monsterIndex = _stageData.BossMonsterIndex;

        var spawnPosition = objPosition;
        MonsterObject obj = MonsterObjectFactory.Instance.Make(Enum_CharacterType.StageBossMonster, spawnPosition,
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
    }

    protected override void OnBattleEnd()
    {
        UI_BossHealthbar.Instance.Hide();
    }

    protected override void OnMonsterDeathReward()
    {
        var goldMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Gold) ? 2 : 1;
        var expMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Exp) ? 2 : 1;
        var stoneMultiply = DataManager.RuneData.IsRuneActivate(Enum_RuneBuffType.Stone) ? 2 : 1;

        DataManager.CurrencyData.Add(Enum_CurrencyType.Gold, _stageData.Gold * goldMultiply * 10);
        DataManager.PlayerData.AddExp(_stageData.Exp * expMultiply * 10);
        
        if (UtilCode.GetChance(_stageData.UpgradeStonePercent))
        {
            DataManager.CurrencyData.Add(Enum_CurrencyType.EquipmentStone, _stageData.UpgradeStone * stoneMultiply);
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
