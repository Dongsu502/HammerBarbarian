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
    }

    /// <summary>
    /// 설정창 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_SettingPanel(bool isActive)
    {
        SettingPanel.SetActive(isActive);
        SettingBackgroundImage.gameObject.SetActive(!isActive);
    }

    #region ButtonEvent

    public void Click_GobackButton()
    {
        mainUIManager.PausePanel_SetActive(false);
        UIWhiteBox.MainUICurrentState = MainUIState.NONE;
    }

    public void Click_Setting()
    {
        SetActive_SettingPanel(true);
        UIWhiteBox.MainUICurrentState = MainUIState.PAUSE_SETTING;
    }

    public void Click_GoTitleButton()
    {
        UIWhiteBox.SceneName = "Title";
        SceneManager.LoadScene("Loading");
    }

    public void Click_QuitButton()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    #endregion
}
