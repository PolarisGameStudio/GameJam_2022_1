using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;

public class MonsterObjectFactory : ObjectMultiPool<MonsterObjectFactory, SpriteMonsterObject>
{
    public const int MAX_WAVE_COUNT = 3;
    
    public SpriteMonsterObject Make(Enum_CharacterType characterType, Vector3 position, int monsterIndex, Enum_BattleType battleType , bool IsSpecialType = false)
    {
      //  SpriteMonsterObject monsterObject = GetPooledObject( x=> x.Monster._data.Index == monsterIndex);
        SpriteMonsterObject monsterObject = GetPooledObject(monsterIndex);
        
        Monster monster = new Monster(TBL_MONSTER.GetEntity(monsterIndex));
        
        monsterObject.Init(characterType, position, monster);

        monsterObject.transform.localScale = IsSpecialType ? Vector3.one * 1.5f : Vector3.one;

        switch (characterType)
        {
            case Enum_CharacterType.StageNormalMonster:
            {
                HealthbarFactory.Instance.Show(monsterObject, monsterObject.GetAbility<SpriteAnimationAbility>().Height);
                break;
            }

            case Enum_CharacterType.StageBossMonster:
            case Enum_CharacterType.BossDungeonMonster:
            {
              // HealthbarFactory.Instance.Show(monsterObject, monsterObject.GetAbility<AnimationAbility>().Height);
               UI_BossHealthbar.Instance.Show(monsterObject);
                break;
            }
        }

        return monsterObject;
    }
}
