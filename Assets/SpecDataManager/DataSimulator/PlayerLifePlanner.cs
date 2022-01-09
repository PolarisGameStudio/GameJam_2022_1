using System;

/// <summary>
/// gacha 에만 테스트로 적용 해보자.
/// 캐릭터 공격은 빈도가 너무 잦음.
/// </summary>
public static class PlayerLifePlanner
{
	private static int _seedGacha = 0;
	private static Random _randomGacha= null;
	private static uint _gachaCounter;

		
	public static void GenerateSeed(int seed1, uint gachaCounter)
	{
		_seedGacha = 5678;		// Test val
		_gachaCounter = gachaCounter;
		
		_randomGacha = new Random(_seedGacha);
		for (ulong i = 0; i < _gachaCounter; i++)
			_randomGacha.Next();
	}
		
	public static Random GachaRandom => _randomGacha;


	#region public Functions

	public static int Random(int min, int max)
	{
		_gachaCounter++;
		return _randomGacha.Next(min, max);
	}
	#endregion

}