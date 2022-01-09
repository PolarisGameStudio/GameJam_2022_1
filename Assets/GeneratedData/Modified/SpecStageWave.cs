using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generated Class From Table: SpecStageWave
/// </summary>
public class SpecStageWave : DataFieldBase<int>
{
	public int stageIndex;	 // stage index
	public string stageName;	 // 
	public int [] Wave1MonsterGroupIndex;	 // monster level index list
	public int [] Wave2MonsterGroupIndex;	 // 
	public int [] Wave3MonsterGroupIndex;	 // 데이터 테이블에 3개 미만으로 설정 되었다면 마지막 인덱스로 3개 소환, 3개 이상 설정해도 이후 무시됨(하지만 데이터는 있음)
	public int [] Wave4MonsterGroupIndex;	 // 
	public int [] Wave5MonsterGroupIndex;	 // 
	public int [] Wave6MonsterGroupIndex;	 // 
	public int [] Wave7MonsterGroupIndex;	 // 
	public int [] Wave8MonsterGroupIndex;	 // 
	public int [] Wave9MonsterGroupIndex;	 // 
	public int [] Wave10MonsterGroupIndex;	 // 
	public int BossMonsterIndex;	 // 해당 레벨의 일반 몬스터 * 10 으로 처리중

	private List<int []> _waveMonsterGroupDataList;
	public List<int []> WaveMonsterGroupDataList
	{
		get 
		{
			if (_waveMonsterGroupDataList == null)
			{
				_waveMonsterGroupDataList = new List<int []>()
				{
					Wave1MonsterGroupIndex,
					Wave2MonsterGroupIndex,
					Wave3MonsterGroupIndex,
					Wave4MonsterGroupIndex,
					Wave5MonsterGroupIndex,
					Wave6MonsterGroupIndex,
					Wave7MonsterGroupIndex,
					Wave8MonsterGroupIndex,
					Wave9MonsterGroupIndex,
					Wave10MonsterGroupIndex
				};
			}

			return _waveMonsterGroupDataList;
		}
	}}
