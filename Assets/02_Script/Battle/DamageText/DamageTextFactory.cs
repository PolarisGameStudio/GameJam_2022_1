using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextFactory : ObjectMultiPool<DamageTextFactory, DamageText>
{
    private bool _useRandomPosition = true;
    public void ToggleRandomPosition(bool isOn)
    {
        _useRandomPosition = isOn;
    }
    
    
    public void Show(Vector3 startPosition, float height, float width ,double damage, Enum_DamageType damageType)
    {
        var pooledDamageText = GetPooledObject( x=>x.Type == damageType);

        if (pooledDamageText == null)
        {
            return;
        }
        
        pooledDamageText.Show(startPosition, height, width, damage, damageType, true);
    }
}
