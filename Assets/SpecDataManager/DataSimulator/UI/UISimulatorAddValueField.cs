using System;
using System.Collections;
using System.Collections.Generic;
using PCG.String;
using UnityEngine;
using UnityEngine.UI;

public class UISimulatorAddValueField : MonoBehaviour
{
    [SerializeField] private Text txtFiledName;
    [SerializeField] private InputField inputValue;
    [SerializeField] private Button btnApply;

    private Action<double> _onApply;
    
    public void SetFieldData(string fieldName, Action<double> onApply)
    {
        _onApply = onApply;
        if (_onApply != null)
        {
            this.btnApply.onClick.AddListener(OnClickApply);
        }
        else
        {
            Debug.LogError($"{fieldName} callback is null");
        }
        txtFiledName.text = fieldName;
        inputValue.text = "";
    }
    
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            OnClickApply();
        }
    }


    void OnClickApply()
    {
        if (this.inputValue.text.IsNullOrEmpty())
            return;
        
        double val = double.Parse(this.inputValue.text);
        if (val <= 0)
            return;
        
        // this.inputValue.text = "";
        
        _onApply?.Invoke(val);
    }
    
}
