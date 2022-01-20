using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class GachaManager : SingletonBehaviour<GachaManager>
{
    private Dictionary<GachaType, GachaHandler> _gachaHandlers;

    public GachaType LastGachaType { get; set; }

    private void Awake()
    {
        _gachaHandlers = new Dictionary<GachaType, GachaHandler>();
        _gachaHandlers.Add(GachaType.Weapon, new WeaponGachaHandler());
        _gachaHandlers.Add(GachaType.Ring, new RingGachaHandler());
        //_gachaHandlers.Add(GachaType.Skill, new SkillGachaHandler());
        
        _gachaHandlers.Add(GachaType.Dice, new DiceGachaHandler());
        
        // _artifactGacha = new Dictionary<GachaType, GachaHandler<int>>();
        //
        // _skillGacha = new Dictionary<GachaType, GachaHandler<int>>();
        // _skillGacha.Add(GachaType.SkillSmall, new SkillSmallGachaHandler());
        // _skillGacha.Add(GachaType.SkillBig, new SkillBigGachaHandler());
        //
        //
    }

    public List<int> Gacha(GachaType type, int count = 1)
    {
        if (!_gachaHandlers.ContainsKey(type))
        {
            Debug.LogError($"{type} 가챠가 없습니다.");
            return null;
        }

        DataManager.GachaData.AddGachaCount(type, count);

        return _gachaHandlers[type].Gacha(count);
    }

    public int GachaByGrade(GachaType type, Enum_ItemGrade grade)
    {
        if (_gachaHandlers.ContainsKey(type))
        {

            return _gachaHandlers[type].GachaByGrade(grade);
        }

        return -1;
    }
    
    
    public int GetRandomStarCount()
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random < 40)
        {
            return 0;
        }
        else if (random < 70)
        {
            return 1;   
        }        
        else if (random < 90)
        {
            return 2;   
        }
        else if(random < 95)
        {
            return 3;
        }    
        else
        {
            return 4;
        }
    }
    
    
    
    

    public int GetRandomIndex(GachaType type)
    {
        // if (_artifactGacha.ContainsKey(type))
        // {
        //     return _artifactGacha[type].GetRandomIndex();
        // }
        // else if (_skillGacha.ContainsKey(type))
        // {
        //     return _skillGacha[type].GetRandomIndex();
        // }

        return 0;
    }

}
