
using System;
using System.Text;
using UnityEngine;

/// 캐릭터 성장 스텟
/// 모든 버프가 적용된 최종 값을 제공한다.
/// 편의상 public 이지만 readonly 로 사용한다..
/// </summary>
[Serializable]
public class PlayerGrowStat
{
	[SerializeField] public int level;
	[SerializeField] public double exp;					
	[SerializeField] public double totalExp;          // 총 획득 경험치
	
	[SerializeField] public double attackPower;		// 전투력
	[SerializeField] public double health;			// 
	[SerializeField] public float moveSpeed;			// %

	// [SerializeField] public float criticalChance;		// %
	// [SerializeField] public float criticalDamage;		// %
	// [SerializeField] public float attackSpeed;		// %

	// [SerializeField] private BerserkerStat _berserkerStat;
	// [SerializeField] private List<EquipItemStatBase> _myEquipItems;

	public double nextLevelExp => DataSpecContainer.GetLevelUpEXP(level + 1);
	
	public PlayerGrowStat()
	{
		level = 1;
		health = 100;
		attackPower = 10;
		moveSpeed = 7;
	}
	

#if UNITY_EDITOR
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"level : {level}");
		stringBuilder.AppendLine($"exp : {exp}");
		stringBuilder.AppendLine($"totalExp : {totalExp}");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine($"attack power : {attackPower}");
		stringBuilder.AppendLine($"health : {health}");
		stringBuilder.AppendLine($"moveSpeed : {moveSpeed}");
		// stringBuilder.AppendLine($"criticalChance % : {criticalChance}");
		// stringBuilder.AppendLine($"criticalDamage % : {criticalDamage}");
		// stringBuilder.AppendLine($"attackSpeed % : {attackSpeed}");

		// stringBuilder.AppendLine($"damage : {GetDamage()}");
		// stringBuilder.AppendLine($"health : {GetHealth()}");
		// stringBuilder.AppendLine($"attackSpeed : {GetAttackSpeed()}");
		// stringBuilder.AppendLine($"criticalChance : {GetCriticalChance()}");
		// stringBuilder.AppendLine($"criticalDamage : {GetCriticalDamage()}");
		// stringBuilder.AppendLine($"moveSpeed : {GetMoveSpeed()}");

		// stringBuilder.AppendLine();
		// stringBuilder.AppendLine($"berserker");
		// stringBuilder.AppendLine($"{_berserkerStat.ToString()}");

		return stringBuilder.ToString();
	}
	#endif
	
	
	// public StatFuncContainer<StatDelegate> CalculatedStats;		// TODO: 이게 얼마나 편한가?
	//
	// private double GetHealth()
	// {
	// 	return _health;
	// }
	//
	// private double GetCriticalChance()
	// {
	// 	return _criticalChance;
	// }
	//
	// private double GetCriticalDamage()
	// {
	// 	return _criticalChance;
	// }
	// public void Init()
	// {
	// 	CalculatedStats = new StatFuncContainer<StatDelegate>();
	// 	CalculatedStats[Enum_CalculatedStat.Power] = GetCharacterPower;
	// 	CalculatedStats[Enum_CalculatedStat.Attack] = GetDamage;
	// 	CalculatedStats[Enum_CalculatedStat.AttackSpeed] = GetAttackSpeed;
	// 	CalculatedStats[Enum_CalculatedStat.Health] = GetHealth;
	// 	CalculatedStats[Enum_CalculatedStat.CriticalChance] = GetCriticalChance;
	// 	CalculatedStats[Enum_CalculatedStat.CriticalDamage] = GetCriticalDamage;
	// 	CalculatedStats[Enum_CalculatedStat.MoveSpeed] = GetMoveSpeed;
	// }

	public void AddExp(double exp)
	{
		this.exp += exp;
		totalExp += exp;

		CheckLevelUp();
	}

	public void CheckLevelUp()
	{
		// if (DataSpecContainer.InstanceSpecLevelUp.ContainsKey(level + 1) == false)
		// {
		// 	Debug.LogError($"spec container doesn't have {level + 1} data. only {DataSpecContainer.InstanceSpecLevelUp.Count} levels exist");
		// 	return;
		// }
		//
		// double levelUpExp = DataSpecContainer.GetLevelUpEXP(level);
		// double remainExp = exp - levelUpExp;
		// if (remainExp >= 0)
		// {
		// 	exp = remainExp;
		// 	level++;
		// 	
		// 	AddLevelUpStats(level);
		// 	
		// 	RefreshEvent.Trigger(Enum_RefreshEventType.StatCalculate);
		// 	RefreshEvent.Trigger(Enum_RefreshEventType.LevelUp);
		// 	
		// 	CheckLevelUp();	// 재귀로 다시 검사. : TODO : 레벨업 여러단계 한 번에 되게 하려면 수정 필요.
		// }
	}

	// level up 시 적용될 값 반영
	private bool AddLevelUpStats(int targetLevel)
	{
		SpecLevelUp levelUpData = DataSpecContainer.InstanceSpecLevelUp[targetLevel];
		attackPower = levelUpData.attack;
		health = levelUpData.health;	 
		// attackSpeed += levelUpData.attackSpeed;
		// criticalChance += levelUpData.criticalChance;
		// criticalDamage += levelUpData.criticalDamage;
		// moveSpeed = levelUpData.moveSpeed;

		return true;
	}
	
}
