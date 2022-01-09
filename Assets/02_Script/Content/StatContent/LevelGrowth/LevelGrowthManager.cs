using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
// [Serializable]
// public class LevelGrowthInfo
// {
//     public List<int> Levels;
// }
//
// public class LevelGrowthManager : StatManager<LevelGrowthManager>
// {
//     private LevelGrowthInfo _leveldGrowthInfo;
//
//     private List<LevelGrowth> _lists;
//
//     protected override void Awake()
//     {
//         base.Awake();
//         
//         Load();
//
//         Init();
//     }
//
//     private void Init()
//     {
//         // int count = _leveldGrowthInfo.Levels.Count;
//         //
//         // _lists = new List<LevelGrowth>(count);
//         //
//         // for (var i = 0; i < count; i++)
//         // {
//         //     //todo: 디비값 넣기
//         //     _lists.Add(new LevelGrowth(Enum_StatType.Damage, _leveldGrowthInfo.Levels[i], 1, 10));
//         // }
//
//         _lists = new List<LevelGrowth>(4);
//
//         _lists.Add(new LevelGrowth(Enum_StatType.Damage, _leveldGrowthInfo.Levels[0], 1, 10));
//         _lists.Add(new LevelGrowth(Enum_StatType.Health, _leveldGrowthInfo.Levels[1], 1, 10));
//         _lists.Add(new LevelGrowth(Enum_StatType.CriticalChance, _leveldGrowthInfo.Levels[2], 1, 10));
//         _lists.Add(new LevelGrowth(Enum_StatType.CriticalDamage, _leveldGrowthInfo.Levels[3], 1, 10));
//     }
//
//     public void TryLevelUp(int index)
//     {
//         if (_lists[index].TryLevelUp())
//         {
//             RefreshEvent.Trigger(Enum_RefreshEventType.Currency);
//
//             CalculateStat();
//         }
//     }
//
//     protected override void InitStat()
//     {
//         Stat.Reset();
//     }
//
//     protected override void CalculateStat()
//     {
//         InitStat();
//
//         foreach (var levelGrowth in _lists)
//         {
//             Stat[levelGrowth.StatType] += levelGrowth.Value;
//         }
//
//         RefreshEvent.Trigger(Enum_RefreshEventType.Stat);
//     }
//
//     public void ResetLevelPoint()
//     {
//         foreach (var levelGrowth in _lists)
//         {
//             levelGrowth.ResetLevel();
//         }
//
//         CalculateStat();
//
//         CurrencyManager.Instance.AddCurrency(Enum_CurrencyType.LevelPoint, UserManager.Instance.Level);
//     }
//
//
//     private const string _saveKey = "levelGrowthInfo";
//
//     public void Save()
//     {
//         ES3.Save(_saveKey, _leveldGrowthInfo);
//     }
//
//     private void Load()
//     {
//         _leveldGrowthInfo = ES3.Load<LevelGrowthInfo>(_saveKey, new LevelGrowthInfo());
//     }
// }