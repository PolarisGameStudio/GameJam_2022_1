using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GachaHandler
{
    public int GachaCount;
    
    // 가챠 시작부터 끝까지
    public abstract List<int> Gacha(int count, bool skipChest = false);
    
    // 가챠 확률처리
    public abstract int GetRandomIndex();

    // 가챠 진행, 리스트화
    public abstract List<int> GetGachaResultList(int gachaCount);

    public abstract void GachaResultAction(List<int> resultList);

    public virtual int GachaByGrade(Enum_ItemGrade grade)
    {
        return -1;
    }
}
