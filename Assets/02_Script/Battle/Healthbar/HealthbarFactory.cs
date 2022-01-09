using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarFactory : ObjectMultiPool<HealthbarFactory, HealthbarObject>
{
    public HealthbarObject Show(CharacterObject owner, float height)
    {
        var healthBar = GetPooledObject((bar) => bar.CharacterType == owner.CharacterType);
        healthBar.Show(owner, height);

        return healthBar;
    }
}
