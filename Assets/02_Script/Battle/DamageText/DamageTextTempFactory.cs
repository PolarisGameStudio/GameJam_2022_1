using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextTempFactory : ObjectMultiPool<DamageTextTempFactory, DamageTextTemp>
{
    public void Show(Vector3 startPosition, float height, double damage, Enum_DamageType damageType)
    {
        var pooledDamageText = GetPooledObject((damageText) => damageText.DamageType == damageType);

        if (pooledDamageText == null)
        {
            return;
        }
        
        pooledDamageText.Show(startPosition, height, damage);
    }
}

//public class DamageText : ObjectPool<D>