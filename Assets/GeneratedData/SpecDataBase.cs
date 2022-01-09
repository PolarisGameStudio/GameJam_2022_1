using UnityEngine;
/// <summary>
/// Generated SpecDataBase
/// </summary>
public class SpecDataBase : SingletonBehaviour<SpecDataBase>
{
	[SerializeField]
	protected DataSet_SpecStageWave _dataSetSpecStageWave;
	public static DataSet_SpecStageWave InstanceSpecStageWave => Instance._dataSetSpecStageWave;

	[SerializeField]
	protected DataSet_SpecLevelUp _dataSetSpecLevelUp;
	public static DataSet_SpecLevelUp InstanceSpecLevelUp => Instance._dataSetSpecLevelUp;

	[SerializeField]
	protected DataSet_SpecBasicWeapon _dataSetSpecBasicWeapon;
	public static DataSet_SpecBasicWeapon InstanceSpecBasicWeapon => Instance._dataSetSpecBasicWeapon;

	[SerializeField]
	protected DataSet_SpecAttackGrowth _dataSetSpecAttackGrowth;
	public static DataSet_SpecAttackGrowth InstanceSpecAttackGrowth => Instance._dataSetSpecAttackGrowth;

	[SerializeField]
	protected DataSet_SpecHealthGrowth _dataSetSpecHealthGrowth;
	public static DataSet_SpecHealthGrowth InstanceSpecHealthGrowth => Instance._dataSetSpecHealthGrowth;

	[SerializeField]
	protected DataSet_SpecCriticalProbGrowth _dataSetSpecCriticalProbGrowth;
	public static DataSet_SpecCriticalProbGrowth InstanceSpecCriticalProbGrowth => Instance._dataSetSpecCriticalProbGrowth;

	[SerializeField]
	protected DataSet_SpecCriticalDamGrowth _dataSetSpecCriticalDamGrowth;
	public static DataSet_SpecCriticalDamGrowth InstanceSpecCriticalDamGrowth => Instance._dataSetSpecCriticalDamGrowth;

	[SerializeField]
	protected DataSet_SpecEXP _dataSetSpecEXP;
	public static DataSet_SpecEXP InstanceSpecEXP => Instance._dataSetSpecEXP;

	[SerializeField]
	protected DataSet_SpecStageMonster _dataSetSpecStageMonster;
	public static DataSet_SpecStageMonster InstanceSpecStageMonster => Instance._dataSetSpecStageMonster;

	[SerializeField]
	protected DataSet_SpecWeaponUpgrade _dataSetSpecWeaponUpgrade;
	public static DataSet_SpecWeaponUpgrade InstanceSpecWeaponUpgrade => Instance._dataSetSpecWeaponUpgrade;

#if UNITY_EDITOR
	public void Reset()
	{
		_dataSetSpecStageWave = EditorUtil.EditorSetInspector<DataSet_SpecStageWave>();
		_dataSetSpecLevelUp = EditorUtil.EditorSetInspector<DataSet_SpecLevelUp>();
		_dataSetSpecBasicWeapon = EditorUtil.EditorSetInspector<DataSet_SpecBasicWeapon>();
		_dataSetSpecAttackGrowth = EditorUtil.EditorSetInspector<DataSet_SpecAttackGrowth>();
		_dataSetSpecHealthGrowth = EditorUtil.EditorSetInspector<DataSet_SpecHealthGrowth>();
		_dataSetSpecCriticalProbGrowth = EditorUtil.EditorSetInspector<DataSet_SpecCriticalProbGrowth>();
		_dataSetSpecCriticalDamGrowth = EditorUtil.EditorSetInspector<DataSet_SpecCriticalDamGrowth>();
		_dataSetSpecEXP = EditorUtil.EditorSetInspector<DataSet_SpecEXP>();
		_dataSetSpecStageMonster = EditorUtil.EditorSetInspector<DataSet_SpecStageMonster>();
		_dataSetSpecWeaponUpgrade = EditorUtil.EditorSetInspector<DataSet_SpecWeaponUpgrade>();
	}

#endif
}
