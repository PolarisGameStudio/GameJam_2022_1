using System;
using UnityEngine;



// all stats for simulator
	
	public enum Enum_EquipItemType
	{
		Weapon,
		Ring,
		Armor
	}


	[Serializable]
	public class EquipItemStatBase
	{
		[SerializeField] private int _level;
		[SerializeField] private Enum_EquipItemType _itemType;

		protected EquipItemStatBase(int level, Enum_EquipItemType type)
		{
			_level = level;
			_itemType = type;
		}

		protected virtual void SetItemStats()
		{
			Debug.LogError("Not implemented!");
		}
		
	}
	

	public class MyWeaponStat : EquipItemStatBase
	{
		public float _damage;
		public int _attackSpeed;

		public MyWeaponStat(int level, Enum_EquipItemType type) : base(level, type)
		{
			
		}
	}