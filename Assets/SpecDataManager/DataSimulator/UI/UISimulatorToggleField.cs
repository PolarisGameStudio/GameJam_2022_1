using System;
using System.Collections;
using System.Collections.Generic;
using PCG.String;
using UnityEngine;
using UnityEngine.UI;

public class UISimulatorToggleField : MonoBehaviour
{
    [SerializeField] private Text txtFiledName;
    [SerializeField] private Text txtState;
    [SerializeField] private Button btnToggle;

    private bool _toggleState = false;
    private Action<bool> _onToggle;
    
    public void SetFieldData(string fieldName, Action<bool> onToggle, bool defaultState = false)
    {
        _onToggle = onToggle;
        if (_onToggle != null)
        {
            this.btnToggle.onClick.AddListener(OnClickToggle);
        }
        else
        {
            Debug.LogError($"{fieldName} callback is null");
        }
        txtFiledName.text = fieldName;
        _toggleState = defaultState;
        txtState.text = _toggleState.ToString();
    }
    
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            OnClickToggle();
        }
    }


    void OnClickToggle()
    {
        _toggleState = !_toggleState;
        this.txtState.text = _toggleState.ToString();
        
        _onToggle?.Invoke(_toggleState);
    }
    
}
