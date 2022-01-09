using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkVFXContainer : MonoBehaviour
{
    [Header("광전사 이펙트")] [SerializeField] private ParticleSystem _berserkIdleEffect;
    [Header("광전사 진입 이펙트")] [SerializeField] private ParticleSystem _berserkEnterEffect;    
    
    [Header("광전사 눈 트레일")] [SerializeField] private Transform _berserkEyeEffect;
    
    public void ChangeBerserkEffectActivation(bool isOn)
    {
        _berserkEnterEffect.Stop();
        _berserkEnterEffect.Play();
        
        _berserkEyeEffect.gameObject.SetActive(isOn);
    }

    public Transform GetBerserkEyeTrailEffect()
    {
        return _berserkEyeEffect;
    }
}
