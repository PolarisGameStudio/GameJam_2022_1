using System;
using System.Text;
using UnityEngine;


[Serializable]
public class BerserkerStat
{
	public readonly int chargeCount;		// 적 1회 타격 시 광폭화 게이지 1스택 누적 (타격 마리수와 관계 없이 1스택)
											// 10스택 누적 시 광폭화 발동 가능
	public readonly float duration;			// 광폭화 발동 시 7초간 아래 효과 발동

	[SerializeField] private float damage;		
	[SerializeField] private float attackSpeed;
	[SerializeField] private float moveSpeed;

	private bool isOn;
	
	// 광폭화 스탯
	protected readonly Stat Stat = new Stat();
	
	public BerserkerStat()
	{
		chargeCount = 10;
		duration = 7;
		isOn = false;
		
		Stat[Enum_StatType.Damage] = damage = 1f;		// 공격력 +100%
		Stat[Enum_StatType.AttackSpeed] = attackSpeed = 1f;	// 공격속도 +100%
		Stat[Enum_StatType.MoveSpeed] = moveSpeed = 1f;		// 이동속도 +100%
	}


	#region public funcs
	
	public double GetStatValue(Enum_StatType type, bool ignoreCondition = false)
	{
		if (isOn || ignoreCondition)
			return Stat[type];

		return 0;
	}

	public void SetBerserkerState(bool bSet)
	{
		isOn = bSet;
	}
	#endregion

#if UNITY_EDITOR
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"Damage : + {Stat[Enum_StatType.Damage] * 100}%");
		stringBuilder.AppendLine($"AttackSpeed : + {Stat[Enum_StatType.AttackSpeed] * 100}%");
		stringBuilder.AppendLine($"MoveSpeed : + {Stat[Enum_StatType.MoveSpeed] * 100}%");

		return stringBuilder.ToString();
	}
#endif
}

	// public abstract class UpgradableStatField<T> where T : struct
	// {
	// 	protected T _value;
	// 	public abstract bool SpecLevelUp(T setVal = default);
	// }
	//
	// public class DoubleFiled : UpgradableStatField<double>
	// {
	// 	public override bool SpecLevelUp(double setVal = default)
	// 	{
	// 		if (setVal == default)
	// 		{
	// 			_value++;
	// 		}
	// 		else
	// 		{
	// 			_value = setVal;
	// 		}
	// 		return _value;
	// 	}
	// }

	
