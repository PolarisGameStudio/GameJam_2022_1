
using System;
using System.Text;

public enum Enum_UpgradeStat
{
    attackLevel,
    healthLevel,
    criticalChanceLevel,
    criticalDamageLevel,
    
    Count
}

/// <summary>
/// player 가 직접 강화시키는 수치들
/// </summary>
[Serializable]
public class PlayerUpgradeStat
{
    public int attackLevel;
    public int healthLevel;
    public int criticalChanceLevel;
    public int criticalDamageLevel;

    public double attack;
    public double health;
    public float criticalChancePercentage;
    public float criticalDamagePercentage;


    public PlayerUpgradeStat()
    {
        attackLevel = 1;
        healthLevel = 1;
        criticalChanceLevel = 1;
        criticalDamageLevel = 1;
        attack = 10;
        health = 100;
        criticalChancePercentage = 0;
        criticalDamagePercentage = 0;

    }

    /// <summary>
    /// 업그레이드 레벨 기준으로 데이터 다시 계산.
    /// </summary>
    // public void RecalculateStatValue()
    // {
    //     attack = 0;
    //     health = 0;
    //     criticalChancePercentage = 0;
    //     criticalDamagePercentage = 0;
    //     
    //     for (var i = 1; i < attackLevel; i++)
    //     {
    //         attack += DataSpecContainer.InstanceSpecAttackGrowth[i].attack;
    //     }
    //     for (var i = 1; i < healthLevel; i++)
    //     {
    //         health += DataSpecContainer.InstanceSpecHealthGrowth[i].health;
    //     }
    //     for (var i = 1; i < criticalChanceLevel; i++)
    //     {
    //         criticalChancePercentage += DataSpecContainer.InstanceSpecCriticalProbGrowth[i].health;
    //     }
    //     for (var i = 1; i < criticalDamageLevel; i++)
    //     {
    //         criticalDamagePercentage += DataSpecContainer.InstanceSpecCriticalDamGrowth[i].health;
    //     }
    // }

    public int LevelUp(Enum_UpgradeStat statType)
    {
        int ret;
        switch (statType)
        {
            case Enum_UpgradeStat.attackLevel:
                ret = ++attackLevel;
                attack = DataSpecContainer.InstanceSpecAttackGrowth[attackLevel].attack;
                break;
            case Enum_UpgradeStat.healthLevel:
                ret = ++healthLevel;
                health = DataSpecContainer.InstanceSpecHealthGrowth[healthLevel].health;
                break;
            case Enum_UpgradeStat.criticalChanceLevel:
                ret = ++criticalChanceLevel;
                criticalChancePercentage = DataSpecContainer.InstanceSpecCriticalProbGrowth[criticalChanceLevel].criticalChancePercentage;
                break;
            case Enum_UpgradeStat.criticalDamageLevel:
                ret = ++criticalDamageLevel;
                criticalDamagePercentage = DataSpecContainer.InstanceSpecCriticalDamGrowth[criticalDamageLevel].criticalDamagePercentage;
                break;
            default:
            {
                throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        return ret;
    }
    
    

    #if UNITY_EDITOR
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"attackLevel         : {attackLevel}");
        stringBuilder.AppendLine($"healthLevel         : {healthLevel}");
        stringBuilder.AppendLine($"criticalChanceLevel : {criticalChanceLevel}");
        stringBuilder.AppendLine($"criticalDamageLevel : {criticalDamageLevel}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"attack value         : {attack}");
        stringBuilder.AppendLine($"health value         : {health}");
        stringBuilder.AppendLine($"criticalChance percentage : {criticalChancePercentage}%");
        stringBuilder.AppendLine($"criticalDamage percentage : {criticalDamagePercentage}%");
        return stringBuilder.ToString();
    }
    #endif
}
