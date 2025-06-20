using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUIManager : MonoBehaviour
{
    private MainUIManager mainUIManager;

    [Header("Panel")]
    [Tooltip("옵션 패널")]
    public GameObject SettingPanel;

    [Space(2)]
    [Tooltip("옵션창 배경이미지")]
    [SerializeField] private Image SettingBackgroundImage;

    [Space]
    [SerializeField] private GameObject keyDescriptionObj;

    private void Awake()
    {
        UIWhiteBox.SetPauseUIWB(this);
    }

    private void Start()
    {
        mainUIManager = GetComponentInParent<MainUIManager>();
    }

    private void OnEnable()
    {
        SetActive_SettingPanel(false);
        SetActive_KeyDescription(false);

        WorldWhiteBox.WhiteBox.PauseGame();
    }

    private void OnDisable()
    {
        WorldWhiteBox.WhiteBox.ResumeGame();
    }


    /// <summary>
    /// 설정창 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_SettingPanel(bool isActive)
    {
        SettingPanel.SetActive(isActive);
        //SettingBackgroundImage.gameObject.SetActive(!isActive);
    }

    public void SetActive_KeyDescription(bool isActive)
    {
        keyDescriptionObj.SetActive(isActive);
    }

    #region ButtonEvent

    public void Click_GobackButton()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        mainUIManager.PausePanel_SetActive(false);
        UIWhiteBox.MainUICurrentState = MainUIState.NONE;
    }

    public void Click_Setting()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        SetActive_SettingPanel(true);
        UIWhiteBox.MainUICurrentState = MainUIState.PAUSE_SETTING;
    }

    public void Click_KeyDescription()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        SetActive_KeyDescription(true);
        UIWhiteBox.MainUICurrentState = MainUIState.PAUSE_KEYDESCRIPTION;
    }

    public void Click_GoTitleButton()
    {
        SoundManager.instance.PlayUI("UI_Botton05");
        SoundManager.instance.StopBGM();

        var currentData = DataManager.Instance.GetCurrentData();
        string currentDataName = DataManager.Instance.currentDataFileName;
        DataManager.Instance.SaveGameData(currentData, currentDataName);

        UIWhiteBox.SceneName = "Title";
        SceneManager.LoadScene("Loading");
    }

    public void Click_QuitButton()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        var currentData = DataManager.Instance.GetCurrentData();
        string currentDataName = DataManager.Instance.currentDataFileName;
        DataManager.Instance.SaveGameData(currentData, currentDataName);

        Debug.Log("게임 종료");
        Application.Quit();
    }

    #endregion
}
