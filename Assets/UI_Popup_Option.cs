using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Option : UI_BasePopup<UI_Popup_Option>
{
    protected override void Refresh()
    {
        SetBGMToggle();
        SetSFXToggle();
        SetSleepModeToggle();
        SetEffectToggle();
    }

    public void OnClickQuit()
    {
        DataManager.Instance.Save();

        UI_Popup_OK.Instance.Open("게임 종료", "게임을 종료하시겠습니까?", () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }, () => { });
    }

    public void ToggleBGM()
    {
        DataManager.OptionData.ToggleMusic();
        SoundManager.Instance.ToggleMusic();
        Refresh();
    }

    public void ToggleSFX()
    {
        DataManager.OptionData.ToggleSfx();
        Refresh();
    }

    public void ToggleSleep()
    {
        DataManager.OptionData.ToggleSleep();
        Refresh();
    }

    public void ToggleVFX()
    {
        DataManager.OptionData.ToggleVfx();
        Refresh();
    }

    public Toggle _bgmOn;
    public Toggle _bgmOff;

    private void SetBGMToggle()
    {
        bool isMusicOn = DataManager.OptionData.BGM;
        _bgmOn.isOn = isMusicOn;
        _bgmOff.isOn = !isMusicOn;
    }

    public Toggle _sfxOn;
    public Toggle _sfxOff;

    private void SetSFXToggle()
    {
        bool isSFXOn = DataManager.OptionData.SFX;
        _sfxOn.isOn = isSFXOn;
        _sfxOff.isOn = !isSFXOn;
    }

    public Toggle _sleepOn;
    public Toggle _sleepOff;

    private void SetSleepModeToggle()
    {
        bool isSleepOn = DataManager.OptionData.Sleep;
        _sleepOn.isOn = isSleepOn;
        _sleepOff.isOn = !isSleepOn;
    }

    public Toggle _vfxOn;
    public Toggle _vfxOff;

    private void SetEffectToggle()
    {
        bool IsEffectOn = DataManager.OptionData.VFX;
        _vfxOn.isOn = IsEffectOn;
        _vfxOff.isOn = !IsEffectOn;
    }
}