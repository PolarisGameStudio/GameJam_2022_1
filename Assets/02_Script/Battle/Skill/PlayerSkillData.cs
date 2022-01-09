using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// todo: 스킬 스펙 데이터 생기면 임시변수 제거 후 수정
public class PlayerSkillData
{
    // 임시 변수
    public int Index;
    
    public float CoolTime = 10;
    public float Damage = 200;

    public float Value1;
    public float Value2;
    public float Value3;
    public float Value4;

    public PlayerSkillData(int index)
    {
        Index = index;
    }
}
