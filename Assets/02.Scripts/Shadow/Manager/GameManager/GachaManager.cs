using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



public class GachaManager : SingletonBehaviour<GachaManager>
{
    private Dictionary<GachaType, GachaHandler<int>> _artifactGacha;
    private Dictionary<GachaType, GachaHandler<int>> _skillGacha;

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

    public void Gacha(GachaType type)
    {
        bool skipChest = LastGachaType == type;
        
        LastGachaType = type;
    }

    public int GetRandomIndex(GachaType type)
    {
        if (_artifactGacha.ContainsKey(type))
        {
            return _artifactGacha[type].GetRandomIndex();
        }
        else if (_skillGacha.ContainsKey(type))
        {
            return _skillGacha[type].GetRandomIndex();
        }

        return 0;
    }

}
