using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bottom : MonoBehaviour
{
    public List<Toggle> Toggles;

    [SerializeField] private UI_Player _player;
    [SerializeField] private UI_Follower _follower;
    [SerializeField] private UI_Equipment _equipment;
    [SerializeField] private UI_Dungeon _dungeon;
    [SerializeField] private UI_Shop _shop;


    public void OnClickButton(int index)
    {
        if (Toggles[index].isOn)
        {
            return;
        }
        Toggles[index].isOn = true;

        _player.Close();
        _follower.Close();
        _equipment.Close();
        _dungeon.Close();
        _shop.Close();

        switch (index)
        {
            case 0:
                _player.Open();
                break;
            case 1:
                _follower.Open();
                break;
            case 2:
                _equipment.Open();
                break;
            case 3:
                break;
            case 4:
                _dungeon.Open();
                break;
            case 5:
                _shop.Open();
                break;
        }
    }
}