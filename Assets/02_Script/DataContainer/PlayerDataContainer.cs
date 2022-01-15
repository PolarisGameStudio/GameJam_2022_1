
using System;
using System.Text;
using UnityEngine;

public class PlayerDataContainer : DataContainerBase, GameEventListener<RefreshEvent>
{
	[NonSerialized]
	public const string DefFileName = "PlayerDataContainer.dat";
	
	[SerializeField]
	private PlayerUpgradeStat _playerUpgradeStatData;
	
	[SerializeField]
	private PlayerGrowStat _playerGrowStatData;

	[SerializeField]


	
	public PlayerDataContainer() : base(false, DefFileName)
	{
		_playerUpgradeStatData = new PlayerUpgradeStat();
		_playerGrowStatData = new PlayerGrowStat();
		
	}


	public void Init()
	{
		CalculateStatValues();	
		this.AddGameEventListening<RefreshEvent>();
	}
	
	public void OnGameEvent(RefreshEvent gameEventType)
	{
		if (gameEventType.Type == Enum_RefreshEventType.PlayerStat || gameEventType.Type == Enum_RefreshEventType.Stat)
		{
			CalculateStatValues();
		}
	}


	#region Reference Stat Data
	public bool GetUpgradeStat(ref PlayerUpgradeStat playerUpgradeStatData)
	{
		if (_playerUpgradeStatData == null)
		{
			Debug.LogError("_playerUpgradeStatData is null but try to get Data");
			return false;
		}

		playerUpgradeStatData = _playerUpgradeStatData;
		// _playerUpgradeStatData.RecalculateStatValue();
		return true;
	}


	public bool GetGrowStat(ref PlayerGrowStat playerGrowStatData)
	{
		if (_playerGrowStatData == null)
		{
			Debug.LogError("_playerGrowStatData is null but try to get Data");
			return false;
		}
		
		playerGrowStatData = _playerGrowStatData;
		return true;
	}

	// public bool GetBerserkerStat(ref BerserkerStat berserkerStatData)
	// {
	// 	if (_berserkerStatData == null)
	// 	{
	// 		Debug.LogError("_berserkerStatData is null but try to get Data");
	// 		return false;
	// 	}
	// 	
	// 	berserkerStatData = _berserkerStatData;
	// 	return true;
	// }
	#endregion



	public Stat Stat = new Stat();



	#region Get Player Stats
	//TODO : stat 계산을 실시간으로 할게 아니라 상태에 따라 set dirty = true 인 경우만 다시 계산해서 캐싱 하도록 처리 필요 
	
	// 전투력 = 공격력 * 공격속도 * (1+치명타 확률) * 치명타 피해량 * 10
	// + 공격력 * (1+광폭화 시 추가 피해량) * (1+광폭화 시 공격속도) * 2
	// + 생명력
	public double GetCharacterPower() => _playerGrowStatData.attackPower * (1 + _playerUpgradeStatData.criticalChancePercentage) * _playerUpgradeStatData.criticalDamagePercentage * 10 +
	                                      _playerGrowStatData.attackPower * 2  + _playerGrowStatData.health;

	private double GetDamage()
	{
		/* 각 요소(성장/장비 등)에서 증가량과 증가율이 함께 사용될 경우 증가량을 우선 합산한 후 증가율 곱연산
		ex) 스탯요소1 공격력 +100 스탯요소2 공격력 +200
		
		무기 공격력 +100% 방어구 공격력 +50%
		-> (100+200)*(1+1)*(1+0.5) = 900			*/
		
		return (_playerUpgradeStatData.attack + _playerGrowStatData.attackPower);
	}
	
	// private double GetAttackSpeedPercentage()
	// {
	// 	return _playerGrowStatData.attackSpeed + _berserkerStatData.GetStatValue(Enum_StatType.AttackSpeed);
	// }
	
	private double GetAttackSpeed()
	{
		return 2  ;		// TODO 기본값과 연산해야 함. 기본값은 어디서? 
	}

	private double GetMoveSpeed()
	{
		return _playerGrowStatData.moveSpeed;
	}
	private double GetHealth()
	{
		return (_playerUpgradeStatData.health + _playerGrowStatData.health + _playerUpgradeStatData.health);
	}
	private float GetCriticalDamagePercentage()
	{
		return 1 + _playerUpgradeStatData.criticalDamagePercentage;
	}
	private double GetCriticalDamage()
	{
		return GetDamage() * GetCriticalDamagePercentage();
	}
	private float GetCriticalChancePercentage()
	{
		return _playerUpgradeStatData.criticalChancePercentage;
	}
	
	
	private void CalculateStatValues()
	{
		Stat[Enum_StatType.Damage] = GetDamage();
		Stat[Enum_StatType.AttackSpeed] = GetAttackSpeed();
		Stat[Enum_StatType.MaxHealth] = GetHealth();
		Stat[Enum_StatType.CriticalDamage] = GetCriticalDamage();
		Stat[Enum_StatType.CriticalChance] = GetCriticalChancePercentage();
		Stat[Enum_StatType.MoveSpeed] = GetMoveSpeed();
	}
	#endregion



	public void AddExp(double exp)
	{
		this._playerGrowStatData.AddExp(exp);
	}
	

#if UNITY_EDITOR
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		
		stringBuilder.AppendLine($"Character Power : {GetCharacterPower()}");
		stringBuilder.AppendLine();
		
		stringBuilder.AppendLine($"Damage : {GetDamage()}");
		stringBuilder.AppendLine($"AttackSpeed : {GetAttackSpeed()}");
		stringBuilder.AppendLine($"MaxHealth : {GetHealth()}");
		stringBuilder.AppendLine($"CriticalDamage : {GetCriticalDamagePercentage()}");
		stringBuilder.AppendLine($"CriticalChance : {GetCriticalChancePercentage()}");
		stringBuilder.AppendLine($"MoveSpeed : {GetMoveSpeed()}");

		return stringBuilder.ToString();
	}
#endif
	
	// TODO: simulator 용  notification 을 여기에 observer 등록하게 할까 공용으로 사용하는 observer 를 쓸까 고민
	public int StatLevelUp(Enum_UpgradeStat statType)
	{
		return _playerUpgradeStatData.LevelUp(statType);
	}
	
	// TODO: simulator 용  notification 을 여기에 observer 등록하게 할까 공용으로 사용하는 observer 를 쓸까 고민
	public int GetStatValue(Enum_UpgradeStat statType)
	{
		switch (statType)
		{
			case Enum_UpgradeStat.attackLevel:
				return _playerUpgradeStatData.attackLevel; 
			case Enum_UpgradeStat.healthLevel:
				return _playerUpgradeStatData.healthLevel;
			case Enum_UpgradeStat.criticalChanceLevel:
				return _playerUpgradeStatData.criticalChanceLevel;
			case Enum_UpgradeStat.criticalDamageLevel:
				return _playerUpgradeStatData.criticalDamageLevel;
			default:
				throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
		}
	}

}