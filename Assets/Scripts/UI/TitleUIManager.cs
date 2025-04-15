using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("PlayButton")]
    [Tooltip("메뉴 버튼")]
    public GameObject PlayButton;

    [Space(2)]
    [Header("Panel")]
    [Tooltip("메뉴 패널")]
    public GameObject MenuPanel;
    [Tooltip("저장데이터 패널")]
    public GameObject StorageDataPanel;

    #region UnityCall_Func

    private void Awake()
    {
        TitleUI_Initialize();
    }

    #endregion

    private void TitleUI_Initialize()
    {
        PlayButton.SetActive(false);

        MenuPanel.SetActive(false);
        StorageDataPanel.SetActive(false);
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
        Debug.Log("새로시작");
    }

    public void Click_StorageDataButton()
    {
        MenuPanel.SetActive(false);

        StorageDataPanel.SetActive(true);
    }

    public void Click_SettingButton()
    {
        Debug.Log("설정");
    }

    public void Click_QuitButton()
    {
        Debug.Log("게임 종료");
    }
}
