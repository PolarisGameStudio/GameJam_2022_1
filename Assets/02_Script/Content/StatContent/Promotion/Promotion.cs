using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: 디비 추가
public class Promotion
{
    public Stat Stat = new Stat();

    public int MaxLevel { get; }

    public Promotion(Stat stat , int maxLevel)
    {
        Stat = stat;
        MaxLevel = maxLevel;
    }
}
