using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private float _sleepTimer;
    
    private void Update()
    {
        if (UI_Popup_Sleep.Instance.isActiveAndEnabled || !DataManager.OptionData.Sleep)
        {
            return;
        }
        
        if (Input.anyKey)
        {
            _sleepTimer = 0;
        }

        _sleepTimer += Time.deltaTime;

        if (_sleepTimer >= SystemValue.SLEEP_TIMER)
        {
            _sleepTimer = 0;
            UI_Popup_Sleep.Instance.Open();
        }
    }
}
