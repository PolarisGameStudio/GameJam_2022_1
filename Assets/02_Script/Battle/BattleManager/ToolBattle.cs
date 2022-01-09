using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class ToolBattle : Battle
{
    public MonsterObject _toolMonsterObject;

    protected override void OnBattleInit()
    {
        _monsterObjects = new List<CharacterObject>(1);
        _monsterObjects.Add(_toolMonsterObject);
    }

    protected override void OnBattleStart()
    {
    }

    protected override void OnBattleClear()
    {
    }

    protected override void OnBattleOver()
    {
    }

    protected override void OnBattleEnd()
    {
    }
}