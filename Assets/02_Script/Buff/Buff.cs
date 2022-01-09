using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구조체 참조 안되면  
[Serializable]
public class Buff
{
    // 버프 시간,
    private float _remainTime;
    public float RemainTime => _remainTime;
    // 버프 타입

    private Stat _stat;
    public Stat Stat => _stat;
    
    // 내부

    public Buff(Stat stat, float remainTime)
    {
        _stat = stat;
        _remainTime = remainTime;
    }

    public bool CheckBuffEnd(float dt)
    {
        _remainTime -= dt;
    
        if (_remainTime <= 0)
        {
            BuffEnd();
            return true;
        }

        return false;
     }

    public void BuffEnd()
    {
    }
}
