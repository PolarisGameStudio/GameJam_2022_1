using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;

[Serializable]
public class MonsterSpriteSet
{
    public List<Sprite> Set;
}

public class MonsterObjectFactory : ObjectPool<MonsterObjectFactory, SpriteMonsterObject>
{
    public List<MonsterSpriteSet> MonsterSpriteSet;

    public const int MAX_WAVE_COUNT = 3;
    
    public SpriteMonsterObject Make(Enum_CharacterType characterType, Vector3 position, int monsterIndex, Enum_BattleType battleType , bool IsSpecialType = false)
    {
        SpriteMonsterObject monsterObject = GetPooledObject();
        
        Monster monster = new Monster(TBL_MONSTER.GetEntity(monsterIndex));
        
        monsterObject.Init(characterType, position, monster, MonsterSpriteSet[monsterIndex].Set);

        monsterObject.transform.localScale = IsSpecialType ? Vector3.one * 1.5f : Vector3.one;

        switch (characterType)
        {
            case Enum_CharacterType.StageNormalMonster:
            {
                HealthbarFactory.Instance.Show(monsterObject, monsterObject.GetAbility<SpriteAnimationAbility>().Height);
                break;
            }

            case Enum_CharacterType.StageBossMonster:
            {
               // HealthbarFactory.Instance.Show(monsterObject, monsterObject.GetAbility<AnimationAbility>().Height);
               // UI_BossHealthbar.Instance.Show(monsterObject);
                break;
            }
        }

        return monsterObject;
    }
}
