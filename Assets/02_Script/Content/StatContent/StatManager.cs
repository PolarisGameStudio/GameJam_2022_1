using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatManager<T> : SingletonBehaviour<T> where T : MonoBehaviour
{
    public Stat Stat = new Stat();

    protected virtual void InitStat()
    {
    }

    protected abstract void CalculateStat();
}
