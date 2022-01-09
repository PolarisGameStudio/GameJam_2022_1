using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 프로토타입  배경음, 효과음 , 자동전투(광폭화) , 보스 자동 ,
public class OptionManager : SingletonBehaviour<OptionManager>
{
    private bool _isAutoBossChallenge;
    public bool IsAutoBossChallenge => _isAutoBossChallenge;

    private bool _isAutoBerserkMode;
    public bool IsAutoBerserkMode => _isAutoBerserkMode;
    
    private bool _isAutoSkill;
    public bool IsAutoSkill => _isAutoSkill;


    public void ToggleAutoBerserkMode(bool isOn)
    {
        _isAutoBerserkMode = isOn;
        
        // todo: 로컬저장
    }

    public void ToggleAutoBossChallenge(bool isOn)
    {
        _isAutoBossChallenge = isOn;
        
        // todo: 로컬저장
    }
    
    public void ToggleAutoSkill(bool isOn)
    {
        _isAutoSkill = isOn;
        
        // todo: 로컬저장
    }
    
}
