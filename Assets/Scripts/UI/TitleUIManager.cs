using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("PlayButton")]
    [Tooltip("메뉴 버튼")]
    [SerializeField]
    private GameObject PlayButton;

    [Space(2)]
    [Header("Panel")]
    [Tooltip("메뉴 패널")]
    [SerializeField]
    private GameObject MenuPanel;
    [Tooltip("저장데이터 패널")]
    [SerializeField]
    private GameObject StorageDataPanel;
    [Tooltip("설정 패널")]
    [SerializeField]
    private GameObject SettingPanel;

    #region UnityCall_Func

    private void Awake()
    {
        TitleUI_Initialize();
    }

    #endregion

    private void TitleUI_Initialize()
    {
        UIWhiteBox.SetTitleUIWB(this);

        PlayButton.SetActive(false);

        MenuPanel.SetActive(false);
        StorageDataPanel.SetActive(false);

        SettingPanel.SetActive(false);
    }

    [ContextMenu("플레이버튼 등장!")]
    public void OnPlayButton()
    {
        PlayButton.SetActive(true);
    }

    public void Click_PlayButton()
    {
        PlayButton.SetActive(false);

        MenuPanel.SetActive(true);
    }

    public void Click_NewGameStartButton()
    {
        SceneManager.LoadScene("Map");
    }

    public void Click_StorageDataButton()
    {
        MenuPanel.SetActive(false);

        StorageDataPanel.SetActive(true);
    }

    public void Click_SettingButton()
    {
        MenuPanel.SetActive(false);

        SettingPanel.SetActive(true);
    }
    public void Click_Setting_GobackButton()
    {
        SettingPanel.SetActive(false);

        MenuPanel.SetActive(true);
    }

    public void Click_QuitButton()
    {
        Debug.Log("게임 종료");
    }

    
}
