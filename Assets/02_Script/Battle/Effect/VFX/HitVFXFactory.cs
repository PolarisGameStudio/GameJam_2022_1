using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitVFXFactory : ObjectMultiPool<HitVFXFactory, HitVFX>
{
    public void Show(Vector3 startPosition, float height, float width, Enum_DamageType damageType)
    {
        var pooledVFX = GetPooledObject((vfx) => vfx.DamageType == damageType);

        if (pooledVFX == null)
        {
            return;
        }

        pooledVFX.Show(startPosition, height, width);
    }
}