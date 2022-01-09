using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SlashVFXContainer : MonoBehaviour
{
    [FormerlySerializedAs("Combination")] public Enum_WeaponCombinationType combinationType;

    [SerializeField] private List<ParticleSystem> _slashVFXList;

    private ParticleSystem _lastSlashVFX = null;

    private float _lastAnimationSpeed;

    private void Start()
    {
        if (_slashVFXList.Count == 0)
        {
            _slashVFXList = new List<ParticleSystem>();
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var slashVFX = transform.GetChild(i).GetComponent<ParticleSystem>();
                if (slashVFX == null)
                {
                    continue;
                }
                
                _slashVFXList.Add(slashVFX);
            }
        }

        for (var i = 0; i < _slashVFXList.Count; i++)
        {
            _slashVFXList[i].gameObject.SetActive(false);
        }
    }

    public void PlaySlashVFX(int order, float speed = 1f)
    {
        if (order >= _slashVFXList.Count || order < 0)
        {
            Debug.LogError($"검기 이펙트 : Order에 문제가 있습니다.  Order:{order}, ListCount:{(_slashVFXList.Count)}");
            return;
        }

        StopCurrentSlashVFX();

        var matchedSlashVFX = _slashVFXList[order];

        if (!speed.Equals(_lastAnimationSpeed))
        {
            foreach (var particle in matchedSlashVFX.GetComponentsInChildren<ParticleSystem>())
            {
                var mainModule = particle.main;
                mainModule.simulationSpeed = speed;
            }

            _lastAnimationSpeed = speed;
        }

        if (matchedSlashVFX == null)
        {
            Debug.LogError($"{order}와 일치하는 SlashVFX가 없습니다.");
            return;
        }

        matchedSlashVFX.gameObject.SetActive(true);
        matchedSlashVFX.Stop();
        matchedSlashVFX.Play();

        _lastSlashVFX = matchedSlashVFX;
    }

    public void StopCurrentSlashVFX()
    {
        if (_lastSlashVFX != null)
        {
            _lastSlashVFX.Stop();
            _lastSlashVFX.gameObject.SetActive(false);
        }

        _lastSlashVFX = null;
    }
}