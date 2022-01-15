using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine.Unity;
using UnityEngine;

public class MonsterObjectFactory : ObjectPool<MonsterObjectFactory, MonsterObject>
{
    public List<SkeletonDataAsset> SkeletonDataAssets;
    public List<AttackPreset> MonsterAttackPreset;

    public const int MAX_WAVE_COUNT = 3;
    
    public MonsterObject Make(Enum_CharacterType characterType, Vector3 position, int monsterIndex, Enum_BattleType battleType , bool IsSpecialType = false)
    {
        MonsterObject monsterObject = GetPooledObject();
        
        Monster monster = new Monster(TBL_MONSTER.GetEntity(monsterIndex));
        
        monsterObject.Init(characterType, position, monster, SkeletonDataAssets[monsterIndex], null);

        monsterObject.transform.localScale = IsSpecialType ? Vector3.one * 1.5f : Vector3.one;

        switch (characterType)
        {
            case Enum_CharacterType.StageNormalMonster:
            {
                HealthbarFactory.Instance.Show(monsterObject, monsterObject.GetAbility<AnimationAbility>().Height);
                break;
            }

            case Enum_CharacterType.StageBossMonster:
            {
                UI_BossHealthbar.Instance.Show(monsterObject);
                break;
            }
        }

        return monsterObject;
    }
}
