using System;

public class GoldGrowthManager : StatManager<GoldGrowthManager>
{
    private PlayerUpgradeStat _playerUpgradeStat;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        PlayerDataManager.PlayerDataContainer.GetUpgradeStat(ref _playerUpgradeStat);
    }

    public void TryLevelUp(Enum_UpgradeStat type)
    {
        PlayerDataManager.PlayerDataContainer.StatLevelUp(type);

        CalculateStat();
    }


    protected override void InitStat()
    {
    }

    protected override void CalculateStat()
    {
        // todo: db스탯 + 유저 스탯레벨로 현재 스탯 계산
        // Stat[Enum_StatType.Damage] = _playerUpgradeStat.le
        // Stat[Enum_StatType.MaxHealth] = _goldGrowthInfo.HealthLevel * 10;

        RefreshEvent.Trigger(Enum_RefreshEventType.Stat);
    }

    public double GetPrice(Enum_UpgradeStat statType)
    {
        switch (statType)
        {
            case Enum_UpgradeStat.attackLevel:

                if (DataSpecContainer.InstanceSpecAttackGrowth.Count <= _playerUpgradeStat.attackLevel)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecAttackGrowth[_playerUpgradeStat.attackLevel].cost;

            case Enum_UpgradeStat.healthLevel:
                
                if (DataSpecContainer.InstanceSpecHealthGrowth.Count <= _playerUpgradeStat.healthLevel)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecHealthGrowth[_playerUpgradeStat.healthLevel].cost;

            case Enum_UpgradeStat.criticalChanceLevel:

                if (DataSpecContainer.InstanceSpecCriticalProbGrowth.Count <= _playerUpgradeStat.criticalChanceLevel)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecCriticalProbGrowth[_playerUpgradeStat.criticalChanceLevel]
                    .cost;

            case Enum_UpgradeStat.criticalDamageLevel:

                if (DataSpecContainer.InstanceSpecCriticalDamGrowth.Count <= _playerUpgradeStat.criticalDamageLevel)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecCriticalDamGrowth[_playerUpgradeStat.criticalDamageLevel]
                    .cost;
        }

        return 0;
    }


    public double GetStatValue(Enum_UpgradeStat statType)
    {
        switch (statType)
        {
            case Enum_UpgradeStat.attackLevel:
                return _playerUpgradeStat.attack;

            case Enum_UpgradeStat.healthLevel:
                return _playerUpgradeStat.health;

            case Enum_UpgradeStat.criticalChanceLevel:
                return _playerUpgradeStat.criticalChancePercentage;

            case Enum_UpgradeStat.criticalDamageLevel:
                return _playerUpgradeStat.criticalDamagePercentage;
        }

        return 0;
    }

    public double GetNextStatValue(Enum_UpgradeStat type)
    {
        switch (type)
        {
            case Enum_UpgradeStat.attackLevel:

                if (DataSpecContainer.InstanceSpecAttackGrowth.Count <= _playerUpgradeStat.attackLevel + 1)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecAttackGrowth[_playerUpgradeStat.attackLevel + 1].attack;

            case Enum_UpgradeStat.healthLevel:

                if (DataSpecContainer.InstanceSpecHealthGrowth.Count <= _playerUpgradeStat.healthLevel + 1)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecHealthGrowth[_playerUpgradeStat.healthLevel + 1].health;

            case Enum_UpgradeStat.criticalChanceLevel:

                if (DataSpecContainer.InstanceSpecCriticalProbGrowth.Count <= _playerUpgradeStat.criticalChanceLevel + 1)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecCriticalProbGrowth[_playerUpgradeStat.criticalChanceLevel + 1]
                    .criticalChancePercentage;

            case Enum_UpgradeStat.criticalDamageLevel:

                if (DataSpecContainer.InstanceSpecCriticalDamGrowth.Count <= _playerUpgradeStat.criticalDamageLevel + 1)
                {
                    return 0;
                }

                return DataSpecContainer.InstanceSpecCriticalDamGrowth[_playerUpgradeStat.criticalDamageLevel+ 1]
                    .criticalDamagePercentage;
        }

        return 0;
    }
}