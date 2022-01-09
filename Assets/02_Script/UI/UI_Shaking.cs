using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UI_Shaking : MonoBehaviour
{
    private Vector3 _originPosition; // 원래 포지션으로 돌아가기 위한 포지션값,

    private Quaternion _originRotation; // 원래 각도로 돌아가기 위한 각도값

    public float ShakeTime = 0.2f; // 흔들림 시간
    private float _shakeTimer;

    public float ShakeIntensity = 0.5f; // 흔들림 강도
    private float _shakeIntensity;

    private void Start()
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
    }

    private void OnDisable()
    {
        transform.position = _originPosition;
        transform.rotation = _originRotation;

        _shakeIntensity = 0;
    }

    private void Update()
    {
        if (_shakeIntensity > 0)
        {
            _shakeTimer += Time.deltaTime;

            transform.position = _originPosition + Random.insideUnitSphere * _shakeIntensity;

            transform.rotation = new Quaternion(
                _originRotation.x + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                _originRotation.y + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                _originRotation.z + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f,
                _originRotation.w + Random.Range(-_shakeIntensity, _shakeIntensity) * .2f);

            _shakeIntensity = Mathf.Lerp(ShakeIntensity, 0, _shakeTimer / ShakeTime);
        }
    }

    public void Shake()
    {
        //흔들림 강도, 간격을 초기화함. 

        _shakeIntensity = ShakeIntensity; // 흔들림 강도

        _shakeTimer = 0;

        //_shakeTime = ShakeTime;
    }
}