using System.Collections.Generic;

public static class WeaponConfig
{
    public static readonly Dictionary<Enum_WeaponType, Dictionary<Enum_WeaponType, Enum_WeaponCombinationType>> Combinations
        = new Dictionary<Enum_WeaponType, Dictionary<Enum_WeaponType, Enum_WeaponCombinationType>>(3)
        {
            {Enum_WeaponType.Sword, new Dictionary<Enum_WeaponType, Enum_WeaponCombinationType>(3)
            {
                {Enum_WeaponType.Sword, Enum_WeaponCombinationType.Sword_Sword},
                {Enum_WeaponType.Axe,   Enum_WeaponCombinationType.Sword_Axe},
                {Enum_WeaponType.Blunt, Enum_WeaponCombinationType.Sword_Blunt},
            }},
                
            {Enum_WeaponType.Axe, new Dictionary<Enum_WeaponType, Enum_WeaponCombinationType>(3)
            {
                {Enum_WeaponType.Sword, Enum_WeaponCombinationType.Sword_Axe},
                {Enum_WeaponType.Axe,   Enum_WeaponCombinationType.Axe_Axe},
                {Enum_WeaponType.Blunt, Enum_WeaponCombinationType.Axe_Blunt},
            }},
                
            {Enum_WeaponType.Blunt, new Dictionary<Enum_WeaponType, Enum_WeaponCombinationType>(3)
            {
                {Enum_WeaponType.Sword, Enum_WeaponCombinationType.Sword_Blunt},
                {Enum_WeaponType.Axe,   Enum_WeaponCombinationType.Axe_Blunt},
                {Enum_WeaponType.Blunt, Enum_WeaponCombinationType.Blunt_Blunt},
            }},
        };


    public const int DefaultLeftHandWeaponIndex  = 0;
    public const int DefaultRightHandWeaponIndex = 1;
}
