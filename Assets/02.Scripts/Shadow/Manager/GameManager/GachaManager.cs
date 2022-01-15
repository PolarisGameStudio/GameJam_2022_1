using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public enum GachaType
{
    ArtifactSmall,
    ArtifactBig,
    SkillSmall,
    SkillBig,
    //ShadowSmall,
    ShadowBig,
    ShadowBeast,
    ShadowUndead,
    ShadowEvil,
    
    None,
}

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
        
        switch (type)
        {
            case GachaType.ArtifactSmall:
            case GachaType.ArtifactBig:
                _artifactGacha[type].Gacha(skipChest);
                break;
            case GachaType.SkillSmall:
            case GachaType.SkillBig:
                _skillGacha[type].Gacha(skipChest);
                break;
            case GachaType.ShadowBig:
            case GachaType.ShadowBeast:
            case GachaType.ShadowUndead:
            case GachaType.ShadowEvil:
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
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

    public int GetArtifactGachaLevel()
    {
        List<int> levelConditions = new List<int>();

        TBL_GACHA_ARTIFACT.ForEachEntity(data => levelConditions.Add(data.RequireCount));
        
        var currenctGachaCount = GetArtifactGachaPoint();
        
        int level = 0;
        
        for (level = 0; level < levelConditions.Count; level++)
        {
            if (currenctGachaCount < levelConditions[level])
            {
                break;
            }
        }
        
        level = Mathf.Min(level, levelConditions.Count - 1);

        return level;
    }

    //todo: 배율은 조절
    public int GetArtifactGachaPoint()
    {
        int gachaPoint = 0;

        return gachaPoint;
    }

    public ArtifactGrade GetArtifactHighestGrade()
    {
        var level = GetArtifactGachaLevel();

        var percents = TBL_GACHA_ARTIFACT.GetEntity(level).Percents;

        int i = percents.Count - 1;
        for (; i >= 0 ; i--)
        {
            if (percents[i] != 0)
            {
                break;
            }
        }

        var highest = (ArtifactGrade) i;

        return highest;
    }
}
