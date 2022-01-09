
using System;
using System.Collections;
//using Sirenix.Utilities;
using UnityEngine;

namespace DataSimulator
{
    using System.Collections.Generic;

    public enum Enum_CalculatedStat : int
    {
        Power,          // 전투력
        Attack,         // 공격력
        AttackSpeed,
        Health,         // 체력

        CriticalChance, 
        CriticalDamage,

        MoveSpeed,

        Count
    }
    
    // public class StatContainer
    // {
    //     private Hashtable _statValues = new Hashtable();
    //     
    //     public object this[Enum_Stat statType]
    //     {
    //         get => _statValues[statType];
    //         set => _statValues.Add(statType, value);
    //     }
    // }
    
    public delegate double StatDelegate();

    public class StatFuncContainer<T>
    {
        private readonly List<T> _values = new List<T>(new T[(int) Enum_CalculatedStat.Count]);
    
        public T this[Enum_CalculatedStat calculatedStatType]
        {
            get
            {
                return _values[(int)calculatedStatType];

            }
            set => _values[(int)calculatedStatType] = value;
        }
    }

    public class StatBuilder
    {

        public virtual StatBuilder ActionCode()
        {
            return this;
        }
    }

}