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
        OptionManager.OnInitialized += InitSlidersSafely;
        TryInitImmediately();
    }

    private void OnDisable()
    {
        OptionManager.OnInitialized -= InitSlidersSafely;
        RemoveListeners();
    }

    private void TryInitImmediately()
    {
        // 씬의 상황에 따라 OptionTest.instance가 이미 생성되어 있을 수도 있으니,
        // 바로 초기화 시도
        if (OptionManager.instance != null)
            InitSlidersSafely();
    }

    private void InitSlidersSafely()
    {
        // 이 함수에서 슬라이더를 OptionTest 값으로 세팅, 리스너 등록
        sliderMaster.value = OptionManager.instance.MasterVolume;
        sliderBGM.value = OptionManager.instance.BGMVolume;
        sliderSFX.value = OptionManager.instance.SFXVolume;
        sliderUI.value = OptionManager.instance.UIVolume;

        // 기존 리스너 해제(중복 방지)
        RemoveListeners();

        // 새로 리스너 등록
        sliderMaster.onValueChanged.AddListener(OnMasterChanged);
        sliderBGM.onValueChanged.AddListener(OnBGMChanged);
        sliderSFX.onValueChanged.AddListener(OnSFXChanged);
        sliderUI.onValueChanged.AddListener(OnUIChanged);
    }

    private void RemoveListeners()
    {
        sliderMaster.onValueChanged.RemoveListener(OnMasterChanged);
        sliderBGM.onValueChanged.RemoveListener(OnBGMChanged);
        sliderSFX.onValueChanged.RemoveListener(OnSFXChanged);
        sliderUI.onValueChanged.RemoveListener(OnUIChanged);
    }

    private void OnMasterChanged(float value) => OptionManager.instance.MasterVolume = value;
    private void OnBGMChanged(float value) => OptionManager.instance.BGMVolume = value;
    private void OnSFXChanged(float value) => OptionManager.instance.SFXVolume = value;
    private void OnUIChanged(float value) => OptionManager.instance.UIVolume = value;

    //private void OnMasterChanged(float value)
    //{
    //    if (OptionManager.instance != null)
    //        OptionManager.instance.MasterVolume = value;
    //}
    //private void OnBGMChanged(float value)
    //{
    //    if (OptionManager.instance != null)
    //        OptionManager.instance.BGMVolume = value;
    //}
    //private void OnSFXChanged(float value)
    //{
    //    if (OptionManager.instance != null)
    //        OptionManager.instance.SFXVolume = value;
    //}
    //private void OnUIChanged(float value)
    //{
    //    if (OptionManager.instance != null)
    //        OptionManager.instance.UIVolume = value;
    //}

    #region EventKey

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
