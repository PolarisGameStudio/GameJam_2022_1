using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class UI_DropItemInfo : MonoBehaviour
{
    [SerializeField] private Image _dropItemImage;
    [SerializeField] private Text _dropItemNameText;
    [SerializeField] private Text _dropItemCountText;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    
    // todo: 아이템 정보 받아서 정보갱신  
    public void Init(int count)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        _dropItemCountText.text = $"x {count}";
        
        StartCoroutine(CloseDelay());
    }

    private IEnumerator CloseDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        _animator.SetTrigger("Close");
        
        yield return new WaitForSecondsRealtime(0.2f);
        
        OnClose();
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
    }
}
