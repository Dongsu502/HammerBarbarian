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
        SoundManager.instance.PlayUI("UI_OptionDrag_Open");
    }

    #region EventKey

    public void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    public void MasterVolumeEvent()
    {
        sliderBGM.value = sliderMaster.value;
        sliderSFX.value = sliderMaster.value;
        sliderUI.value = sliderMaster.value;
    }

    #endregion
}
