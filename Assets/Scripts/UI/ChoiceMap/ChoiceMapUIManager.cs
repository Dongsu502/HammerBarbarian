using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoiceMapUIManager : MonoBehaviour
{
    [Header("던전 맵")]
    public GameObject DungeonPanel;
    [SerializeField] private Button[] DungeonButtons;

    [Header("월드 맵")]
    public GameObject WorldPanel;
    [SerializeField] private Button[] WorldButtons;

    [Header("세팅패널")]
    public GameObject SettingPanel;

    private UIInputAction uiInput;

    private void Awake()
    {
        uiInput = new UIInputAction();
    }

    private void Start()
    {
        SoundManager.instance.StopBGM();
    }

    private void OnEnable()
    {
        uiInput.ChoiceMapUI.Enable();

        uiInput.ChoiceMapUI.OpenWorldMap.started += R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started += RightMouseDown;
        uiInput.ChoiceMapUI.Setting.started += EscapeEvent;

        InitializeChoiceUI();
    }
    private void OnDisable()
    {
        uiInput.ChoiceMapUI.Disable();

        uiInput.ChoiceMapUI.OpenWorldMap.started -= R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started -= RightMouseDown;
        uiInput.ChoiceMapUI.Setting.started -= EscapeEvent;
    }

    private void InitializeChoiceUI()
    {
        DungeonPanel.SetActive(true);
        WorldPanel.SetActive(false);
        SettingPanel.SetActive(false);

        UIWhiteBox.ChoiceMapUICurrentState = ChoiceMapUIState.DUNGEON;
    }

    private void R_KeyDown(InputAction.CallbackContext context)
    {
        switch(UIWhiteBox.ChoiceMapUICurrentState)
        {
            case ChoiceMapUIState.WORLD:
                return;
            case ChoiceMapUIState.DUNGEON:
                SetActiveWorldPanel(true);
                SetActiveDungeonPanel(false);
                UIWhiteBox.ChoiceMapUICurrentState = ChoiceMapUIState.WORLD;
                break;
        }
    }

    private void RightMouseDown(InputAction.CallbackContext context)
    {
        UIWhiteBox.SceneName = "Title";
        SceneManager.LoadScene("Loading");
    }

    private void EscapeEvent(InputAction.CallbackContext context)
    {
        ChoiceMapUIState currentUIState = ChoiceMapUIState.DUNGEON;
        if(UIWhiteBox.ChoiceMapUICurrentState != ChoiceMapUIState.SETTING)
        {
            if(UIWhiteBox.ChoiceMapUICurrentState == ChoiceMapUIState.DUNGEON)
            {
                currentUIState = ChoiceMapUIState.DUNGEON;
            }
            else if(UIWhiteBox.ChoiceMapUICurrentState == ChoiceMapUIState.WORLD)
            {
                currentUIState = ChoiceMapUIState.WORLD;
            }
            SettingPanel.SetActive(true);

            UIWhiteBox.ChoiceMapUICurrentState = ChoiceMapUIState.SETTING;
        }
        else
        {
            SettingPanel.GetComponent<Animator>().SetTrigger("Off");

            UIWhiteBox.ChoiceMapUICurrentState = currentUIState;
        }
    }

    /// <summary>
    /// 던전패널 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveDungeonPanel(bool isActive)
    {
        DungeonPanel.SetActive(isActive);
    }

    /// <summary>
    /// 월드패널 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveWorldPanel(bool isActive)
    {
        WorldPanel.SetActive(isActive);
    }

    #region 버튼 클릭 이벤트

    public void Click_Dungeon1()
    {
        Debug.Log("던전1 이동");
    }
    public void Click_Dungeon2()
    {
        Debug.Log("던전2 이동");
    }
    public void Click_Dungeon3()
    {
        SoundManager.instance.PlayUI("UI_Botton07");

        Debug.Log("던전3 이동");
        UIWhiteBox.SceneName = "New Map";
        SceneManager.LoadScene("Loading");
    }

    public void Click_World1()
    {
        SoundManager.instance.PlayUI("UI_Botton07");

        Debug.Log("월드1 던전선택창 이동");

        SetActiveWorldPanel(false);
        SetActiveDungeonPanel(true);

        UIWhiteBox.ChoiceMapUICurrentState = ChoiceMapUIState.DUNGEON;
    }
    public void Click_World2()
    {
        Debug.Log("월드2 던전선택창 이동");
    }
    public void Click_World3()
    {
        Debug.Log("월드3 던전선택창 이동");
    }
    public void Click_World4()
    {
        Debug.Log("월드4 던전선택창 이동");
    }

    #endregion
}
