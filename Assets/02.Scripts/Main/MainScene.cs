using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleManager.Instance.BattleStart(Enum_BattleType.StageBoss, DataManager.BattleData.StageLevel);   
        }
    }
}
