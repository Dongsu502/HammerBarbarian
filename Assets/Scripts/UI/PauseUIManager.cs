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

    private void Start()
    {
        mainUIManager = GetComponentInParent<MainUIManager>();
    }

    private void OnEnable()
    {
        SettingPanel.SetActive(false);
    }

    public void Click_GobackButton()
    {
        mainUIManager.pausePanel_SetActive(false);
    }

    public void Click_Save()
    {
        Debug.Log("저장");
    }

    public void Click_Setting()
    {
        SettingPanel.SetActive(true);
    }

    public void Click_KeyboardSettingButton()
    {
        Debug.Log("키보드세팅");
    }

    public void Click_GoTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void Click_QuitButton()
    {
        Debug.Log("게임 종료");
    }
}
