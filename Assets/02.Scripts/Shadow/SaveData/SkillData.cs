
using System.Collections.Generic;
public class SkillData : SaveDataBase
{
    public List<int> EquippedIndex = new List<int>();

    public override void ValidCheck()
    {
        base.ValidCheck();
        
        int slotCount = SystemValue.SKILL_MAX_SLOT_COUNT;
        int saveCount = EquippedIndex.Count;

        if (slotCount > saveCount)
        {
            for (int i = saveCount; i < slotCount; i++)
            {
                EquippedIndex.Add(0);
            }
        }

    }

    public void AddSkillList(List<TBL_SKILL> skillList)
    {
    }
}
