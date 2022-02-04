using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    public GameObject _popupDice;
    private void OnDisable()
    {
        _popupDice.gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        
    }
}
