using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleUIManager : MonoBehaviour
{
    [Header("PlayButton")]
    [Tooltip("메뉴 버튼")]
    [SerializeField] private GameObject PlayButton;

    [Space(2)]
    [Header("Panel")]
    [Tooltip("메뉴 패널")]
    [SerializeField] private GameObject MenuPanel;
    [Tooltip("저장데이터 패널")]
    [SerializeField] private GameObject StorageDataPanel;
    [Tooltip("설정 패널")]
    [SerializeField] private GameObject SettingPanel;

    #region UnityCall_Func

    private void Awake()
    {
        TitleUI_Initialize();
    }

    private void OnEnable()
    {
        StartCoroutine(StartSequence());
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

    public void GobackMenu()
    {
        MenuPanel?.SetActive(true);

        PlayButton?.SetActive(false);
        StorageDataPanel?.SetActive(false);
        SettingPanel?.SetActive(false);
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(3f);
        //플레이 버튼 활성화
        OnPlayButton();
    }

    #region ButtonEvent

    public void Click_PlayButton()
    {
        PlayButton.SetActive(false);

        MenuPanel.SetActive(true);
    }

    public void Click_NewGameStartButton()
    {
        if(!DataManager.Instance.NeedToCreateNewDataFile())
        {
            Debug.LogWarning("모든 저장 슬롯이 사용 중입니다. 새 게임을 시작할 수 없습니다.");
            return;
        }

        int emptySlotIndex = DataManager.Instance.GetFirstEmptyDataSlotIndex();

        if(emptySlotIndex != -1)
        {
            // 슬롯 설정
            DataManager.Instance.SetCurrentData(emptySlotIndex);

            // 슬롯에 맞는 데이터 인스턴스 생성 및 초기화
            switch (emptySlotIndex)
            {
                case 0:
                    DataManager.Instance.data1 = new Data1();
                    DataManager.Instance.DeleteDataFile(0);
                    DataManager.Instance.SaveGameData(DataManager.Instance.data1, DataManager.Instance.GameDataFileName1);
                    break;

                case 1:
                    DataManager.Instance.data2 = new Data2();
                    DataManager.Instance.DeleteDataFile(1);
                    DataManager.Instance.SaveGameData(DataManager.Instance.data2, DataManager.Instance.GameDataFileName2);
                    break;

                case 2:
                    DataManager.Instance.data3 = new Data3();
                    DataManager.Instance.DeleteDataFile(2);
                    DataManager.Instance.SaveGameData(DataManager.Instance.data3, DataManager.Instance.GameDataFileName3);
                    break;
            }

            Debug.Log($"새로운 게임이 슬롯 {emptySlotIndex + 1}번에 생성되었습니다.");

            SceneManager.LoadScene("ChoiceDungeon");
        }
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
        Application.Quit();
    }

    #endregion
}
