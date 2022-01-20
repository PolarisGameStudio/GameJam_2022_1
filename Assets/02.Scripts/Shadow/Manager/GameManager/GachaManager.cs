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
        // _artifactGacha = new Dictionary<GachaType, GachaHandler<int>>();
        // _artifactGacha.Add(GachaType.ArtifactSmall, new ArtifactSmallGachaHandler());
        // _artifactGacha.Add(GachaType.ArtifactBig, new ArtifactBigGachaHandler());
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
