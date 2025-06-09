using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    #region EventKey

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    public void SetMatserVolume(float sliderValue)
    {
        Debug.Log($"MasterVolume: {sliderValue}");
        SetBackgroundVolume(sliderValue);
        SetSFXVolume(sliderValue);
        SetUISFXVolume(sliderValue);
    }

    public void SetBackgroundVolume(float sliderValue)
    {
        Debug.Log($"BackgroundVolume: {sliderValue}");
        SoundManager.instance.SetBGMVolume(sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        Debug.Log($"SFXVolume: {sliderValue}");
        SoundManager.instance.SetPlayerSFXVolume(sliderValue);
        SoundManager.instance.SetMonsterSFXVolume(sliderValue);
    }

    public void SetUISFXVolume(float sliderValue)
    {
        Debug.Log($"UIVolume: {sliderValue}");
        SoundManager.instance.SetUIVolume(sliderValue);
    }

    #endregion
}
