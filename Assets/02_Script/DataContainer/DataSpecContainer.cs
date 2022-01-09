using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Todo :
/// 현재 데이터 스펙이 확정된 상태가 아니므로 테이블 이름이 바뀔 가능성이 크다.
/// 되도록 이 스크립트 안에서 계산 하고, 이후 테이블 이름이나 갯수가 확정되면 코드 분리하도록 해야 할 듯.
/// </summary>
public class DataSpecContainer : SpecDataBase
{
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this.gameObject);
	}

	public static double GetLevelUpEXP(int currentLevel)
	{
		if (InstanceSpecEXP.Keys.Last() < currentLevel)
		{
			Debug.LogError($"최대 레벨 도달 : {currentLevel} 마지막 값을 반환합니다.");
			return InstanceSpecEXP.Values.Last().exp;
		}
		return InstanceSpecEXP[currentLevel].exp;
	}
	
	
	// #region Init inspector data

	
	// public SpecWave 
	
	

// #if UNITY_EDITOR
// 	public void Reset()
// 	{
// 		Debug.Log("Set all DataSet");

// 		
// 		_dataSetSpecWave = EditorUtil.EditorSetInspector<DataSet_SpecWave>();
// 		_dataSetSpecMonsterType = EditorUtil.EditorSetInspector<DataSet_SpecMonsterType>();
// 		_dataSetSpecMonster = EditorUtil.EditorSetInspector<DataSet_SpecMonster>();
// 		_dataSetSpecLevelUp = EditorUtil.EditorSetInspector<DataSet_SpecLevelUp>();
// 		_dataSetSpecBasicWeapon = EditorUtil.EditorSetInspector<DataSet_SpecBasicWeapon>();
// 	}
//
// #endif
// 	#endregion
// 	
// 	public SpecMonster GetMonsterData(int level)
// 	{
// 		SpecMonster monster = null;
// 		if (_dataSetSpecMonster.TryGetValue(level, out monster) == false)
// 		{
// 			// Debug.LogError($"failed to get Monster data {level}");
// 			throw new Exception($"failed to get MonsterHP data {level}");
// 		}
//
// 		return monster;
// 	}
// 	
	// 일반 몬스터 HP 공식
	// 	- 초기 HP 100 (기본 공격력으로 10회 타격)
	// 	- 이전 레벨 HP * 1.2
	// 	- 10레벨마다 이전 레벨 대비 일정값 곱적용
	//
	// 보스 몬스터 HP 공식
	// 	- 해당 스테이지 일반 몬스터 HP * 100
	
	
	// 전투력 산정 공식(프로토타입용)
	// 캐릭터 상세정보창에 표기되는 수치를 기준으로 연산
	// 전투력 = 공격력 * 공격속도 * (1+치명타 확률) * 치명타 피해량 * 10
	// + (1+광폭화 시 추가 피해량) * (1+광폭화 시 공격속도) * 2
	// + 생명력
	
	
	
	// 광폭화 발동 시 7초간 아래 효과 발동
	// 공격력 +100%
	// 공격속도 +100%
	// 이동속도 +100%

	// 장비
	// 강화 레벨
	// 무기의 강화 단계
	// 공격력
	// 공격속도

}
