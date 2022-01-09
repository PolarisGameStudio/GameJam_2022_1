using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextTestCanvas : MonoBehaviour
{
    public MonsterObject MonsterObject;

    public double Damage = 312738;

    public float HeightOffset;
    public float WidthOffest;

    private void Start()
    {
    }

    public void OnClickNormalDamageText()
    {
        DamageTextTempFactory.Instance.Show(new Vector3(0, 0), 0f, Damage, Enum_DamageType.Normal);
    }

    public void OnClickCriticalDamageText()
    {
        DamageTextTempFactory.Instance.Show(new Vector3(0, 0), 0f, Damage, Enum_DamageType.Critical);
    }

    public void OnClickTestButton()
    {
        DamageTextFactory.Instance.Show(MonsterObject.Position, HeightOffset, WidthOffest, Damage,
            Enum_DamageType.Normal);
    }
}