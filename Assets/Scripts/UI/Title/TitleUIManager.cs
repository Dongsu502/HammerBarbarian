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

    [Space(2)]
    [Header("Animator")]
    [Tooltip("타이틀캔버스 애니메이터")]
    [SerializeField] private Animator Anim_Title;
    [Tooltip("페이드인아웃 이미지")]
    [SerializeField] private Image Image_fadeInOut;
    [Tooltip("설정창 Off 애니메이션")]
    [SerializeField] private Animator Anim_SettingOff;

    public bool isNewGame { get; private set; }

    #region UnityCall_Func

    private void Awake()
    {
        TitleUI_Initialize();
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM("Title01", 0);
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

        //페이드인아웃 이미지
        Image_fadeInOut.gameObject.SetActive(false);
    }

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
        yield return new WaitForSeconds(2f);
        //플레이 버튼 활성화
        OnPlayButton();
    }

    #region ButtonEvent

    public void Click_PlayButton()
    {
        PlayButton.SetActive(false);

        Anim_Title.SetTrigger("Start");
        MenuPanel.SetActive(true);
    }

    public void Click_NewGameStartButton()
    {
        isNewGame = true;

        SettingPanel.SetActive(false);
        //페이드인아웃 애니메이션
        Image_fadeInOut.gameObject.SetActive(true);
    }

    public void Click_StorageDataButton()
    {
        isNewGame = false;

        SettingPanel.SetActive(false);
        //페이드인아웃 애니메이션
        Image_fadeInOut.gameObject.SetActive(true);
    }

    public void Click_SettingButton()
    {
        SettingPanel.SetActive(true);
    }
    public void Click_Setting_GobackButton()
    {
        Anim_SettingOff.SetTrigger("Off");
    }

    public void Click_QuitButton()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    #endregion
}
