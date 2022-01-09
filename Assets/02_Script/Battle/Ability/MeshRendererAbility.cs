using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererAbility : CharacterAbility
{
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    private Coroutine _flashCoroutine;
    [Header("점멸 컬러")] [SerializeField]
    private Color _flashColor = Color.white;
    [Header("점멸 값")] [SerializeField]
    private float _flashValue = 1f;
    [Header("점멸 시간")] [SerializeField]
    private float _flashTime = 0.2f;

    private Coroutine _fillBlackCoroutine;
    private Color _fillBlackColor = Color.black;

    private static readonly int FillColor = Shader.PropertyToID("_FillColor");
    private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
    

    public override void Init()
    {
        base.Init();
        
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        
        StopFlash();
        StopFillBlack();
    }

    
    // todo: Disable 사용 시 null상태인 컴포넌트(_meshRenderer) 호출함. 
    // private void OnDisable()
    // {
    //     StopFlash();
    //     StopFillBlack();
    // }

    public override void LateProcessAbility()
    {
        _meshRenderer.sortingOrder = (int)(transform.position.y * -100f);
    }

    private void Reset()
    {
        _materialPropertyBlock.SetFloat(FillPhase, 0f);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    
    public void Flash()
    {
        StopFlash();
        
        _flashCoroutine = StartCoroutine(Flash_Coroutine());
    }

    private IEnumerator Flash_Coroutine()
    {
        _materialPropertyBlock.SetColor(FillColor, _flashColor);
        _materialPropertyBlock.SetFloat(FillPhase, _flashValue);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        
        yield return new WaitForSeconds(_flashTime);
        
        _materialPropertyBlock.SetFloat(FillPhase, 0f);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void StopFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }
        
        Reset();
    }


    public void FillBlack(float duration)
    {
        StopFillBlack();

        _fillBlackCoroutine = StartCoroutine(FillBlack_Coroutine(duration));
    }
    
    private IEnumerator FillBlack_Coroutine(float duration, int count = 10)
    {
        _materialPropertyBlock.SetColor(FillColor, _fillBlackColor);

        _materialPropertyBlock.SetFloat(FillPhase, 0f);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);

        var fillValue = 0f;
        var interval = 1f / count;
        var wait = new WaitForSeconds(interval);
        var startTime = Time.time;

        while (true)
        {
            fillValue += interval;   
            
            _materialPropertyBlock.SetFloat(FillPhase, fillValue);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
            
            yield return wait;

            if ((Time.time - startTime) >= duration)
            {
                yield break;
            }
        }
        
        _materialPropertyBlock.SetFloat(FillPhase, 0f);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void StopFillBlack()
    {
        if (_fillBlackCoroutine != null)
        {
            StopCoroutine(_fillBlackCoroutine);
            _fillBlackCoroutine = null;
        }

        Reset();
    }
}
