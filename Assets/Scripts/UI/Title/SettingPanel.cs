using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderUI;

    private void OnEnable()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayUI("UI_OptionDrag_Open");

            // 슬라이더 값을 SoundManager의 저장된 값으로 초기화
            sliderMaster.value = SoundManager.instance.MasterVolume; // SoundManager에서 MasterVolume 가져오기
            sliderBGM.value = SoundManager.instance.BGMVolume; // SoundManager에서 BGMVolume 가져오기
            sliderSFX.value = SoundManager.instance.SFXVolume; // SoundManager에서 SFXVolume 가져오기
            sliderUI.value = SoundManager.instance.UIVolume; // SoundManager에서 UIVolume 가져오기
        }
    }

    private void Start()
    {
        // 슬라이더 값 변경 시 SoundManager에 반영
        sliderMaster.onValueChanged.AddListener((value) => SoundManager.instance.MasterVolume = value);
        sliderBGM.onValueChanged.AddListener((value) => SoundManager.instance.BGMVolume = value);
        sliderSFX.onValueChanged.AddListener((value) => SoundManager.instance.SFXVolume = value);
        sliderUI.onValueChanged.AddListener((value) => SoundManager.instance.UIVolume = value);
    }

    #region EventKey

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
