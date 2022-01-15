using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public static class DataTableExtension
{
    
}

// public partial class TBL_TEXT : BGEntity
// {
//     public string Text
//     {
//         get
//         {
//             if (string.IsNullOrEmpty(Korean))
//             {
//                 return string.Empty;
//             }
            
//             switch (Application.systemLanguage)
//             {
//                 case SystemLanguage.Korean: return Korean.Replace("\\n", "\n");

//                 case SystemLanguage.Chinese:
//                 case SystemLanguage.ChineseTraditional:
//                 case SystemLanguage.ChineseSimplified: return Taiwanese.Replace("\\n", "\n");
//             }
            
//             return Korean.Replace("\\n", "\n");
//         }
//     }
// }

public partial class TBL_STAGE : BGEntity
{
    public List<float> ArtifactDropChanceList
    {
        get
        {
            var list = new List<float>(new float[20]);
            list[0] = ArtifactChance1;
            list[1] = ArtifactChance2;
            list[2] = ArtifactChance3;
            list[3] = ArtifactChance4;
            list[4] = ArtifactChance5;
            list[5] = ArtifactChance6;
            list[6] = ArtifactChance7;
            list[7] = ArtifactChance8;
            list[8] = ArtifactChance9;
            list[9] = ArtifactChance10;
            list[10] = ArtifactChance11;
            list[11] = ArtifactChance12;
            list[12] = ArtifactChance13;
            list[13] = ArtifactChance14;
            list[14] = ArtifactChance15;
            list[15] = ArtifactChance16;
            list[16] = ArtifactChance17;
            list[17] = ArtifactChance18;
            list[18] = ArtifactChance19;
            list[19] = ArtifactChance20;
            
            return list;
        }
    }

    public List<ArtifactGrade> ArtifactDropGradeList
    {
        get
        {
            var list = new List<ArtifactGrade>(new ArtifactGrade[20]);
            list[0] = ArtifactGrade1;
            list[1] = ArtifactGrade2;
            list[2] = ArtifactGrade3;
            list[3] = ArtifactGrade4;
            list[4] = ArtifactGrade5;
            list[5] = ArtifactGrade6;
            list[6] = ArtifactGrade7;
            list[7] = ArtifactGrade8;
            list[8] = ArtifactGrade9;
            list[9] = ArtifactGrade10;
            list[10] = ArtifactGrade11;
            list[11] = ArtifactGrade12;
            list[12] = ArtifactGrade13;
            list[13] = ArtifactGrade14;
            list[14] = ArtifactGrade15;
            list[15] = ArtifactGrade16;
            list[16] = ArtifactGrade17;
            list[17] = ArtifactGrade18;
            list[18] = ArtifactGrade19;
            list[19] = ArtifactGrade20;

            return list;
        }
    }
}

public partial class TBL_DUNGEON1 : BGEntity
{
    public List<RewardType> RewardTypes
    {
        get
        {
            var list = new List<RewardType>(3);
            list.Add(RewardType1);
            list.Add(RewardType2);
            list.Add(RewardType3);

            return list;
        }
    }
    
    public List<int> RewardCounts
    {
        get
        {
            var list = new List<int>(3);
            list.Add(RewardCount1);
            list.Add(RewardCount2);
            list.Add(RewardCount3);

            return list;
        }
    }
}

public partial class TBL_STAGE : BGEntity
{
    public int WaveCount
    {
        get
        {
            int count = 1;
            
            if (Wave2 != null) count += 1;
            else return count;
            
            if (Wave3 != null) count += 1;
            else return count;
            
            if (Wave4 != null) count += 1;
            else return count;
            
            if (Wave5 != null) count += 1;

            return count;
        }
    }
}

public partial class TBL_SHADOW_PROMOTION : BGEntity
{
    public List<int> Damages      => new List<int>(5){Damage1, Damage2, Damage3, Damage4, Damage5};
    public List<int> Healths      => new List<int>(5){Health1, Health2, Health3, Health4, Health5};
    public List<int> AttackSpeeds => new List<int>(5){AttackSpeed1, AttackSpeed2, AttackSpeed3, AttackSpeed4, AttackSpeed5};
    public List<int> TobeolPowers => new List<int>(5){ TobeolPower1, TobeolPower2, TobeolPower3, TobeolPower4, TobeolPower5 };
}