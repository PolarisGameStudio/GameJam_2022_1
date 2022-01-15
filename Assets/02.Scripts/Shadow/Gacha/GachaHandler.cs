using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GachaHandler<T>
{
    
    // 가챠 시작부터 끝까지
    public abstract void Gacha(bool skipChest = false);
    
    // 가챠 확률처리
    public abstract int GetRandomIndex();

    // 가챠 진행, 리스트화
    public abstract List<T> GetGachaResultList(int gachaCount);

    public abstract void GachaResultAction(List<T> resultList);
}
