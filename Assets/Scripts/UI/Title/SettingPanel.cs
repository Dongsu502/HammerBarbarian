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
        // 옵션매니저의 현재 값으로 슬라이더 초기화
        if (OptionManager.instance != null)
        {
            sliderMaster.value = OptionManager.instance.MasterVolume;
            sliderBGM.value = OptionManager.instance.BGMVolume;
            sliderSFX.value = OptionManager.instance.SFXVolume;
            sliderUI.value = OptionManager.instance.UIVolume;
        }

        // 이벤트 리스너 등록 (슬라이더 값이 바뀔 때 옵션매니저에 반영)
        sliderMaster.onValueChanged.AddListener(OnMasterChanged);
        sliderBGM.onValueChanged.AddListener(OnBGMChanged);
        sliderSFX.onValueChanged.AddListener(OnSFXChanged);
        sliderUI.onValueChanged.AddListener(OnUIChanged);
    }

    private void OnDisable()
    {
        // 리스너 해제 (메모리 누수 방지)
        sliderMaster.onValueChanged.RemoveListener(OnMasterChanged);
        sliderBGM.onValueChanged.RemoveListener(OnBGMChanged);
        sliderSFX.onValueChanged.RemoveListener(OnSFXChanged);
        sliderUI.onValueChanged.RemoveListener(OnUIChanged);
    }

    private void OnMasterChanged(float value)
    {
        if (OptionManager.instance != null)
            OptionManager.instance.MasterVolume = value;
    }
    private void OnBGMChanged(float value)
    {
        if (OptionManager.instance != null)
            OptionManager.instance.BGMVolume = value;
    }
    private void OnSFXChanged(float value)
    {
        if (OptionManager.instance != null)
            OptionManager.instance.SFXVolume = value;
    }
    private void OnUIChanged(float value)
    {
        if (OptionManager.instance != null)
            OptionManager.instance.UIVolume = value;
    }

    #region EventKey

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
