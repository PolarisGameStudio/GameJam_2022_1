using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BottomMenu : MonoBehaviour
{
    public void OnClickCharacterButton()
    {
        UI_Player.Instance.Open();
    }
}
