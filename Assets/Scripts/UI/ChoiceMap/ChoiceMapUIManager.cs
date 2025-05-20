using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoiceMapUIManager : MonoBehaviour
{
    [Header("던전 맵")]
    public GameObject DungeonPanel;

    [Header("월드 맵")]
    public GameObject WorldPanel;

    private UIInputAction uiInput;

    private bool worldPanelIsActive = false;

    private void Awake()
    {
        uiInput = new UIInputAction();
    }

    private void OnEnable()
    {
        uiInput.ChoiceMapUI.Enable();

        uiInput.ChoiceMapUI.OpenWorldMap.started += R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started += RightMouseDown;
    }
    private void OnDisable()
    {
        uiInput.ChoiceMapUI.Disable();

        uiInput.ChoiceMapUI.OpenWorldMap.started -= R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started -= RightMouseDown;
    }

    private void Start()
    {
        InitializeChoiceUI();
    }

    private void InitializeChoiceUI()
    {
        DungeonPanel.SetActive(true);
        WorldPanel.SetActive(false);
    }

    private void R_KeyDown(InputAction.CallbackContext context)
    {
        if(worldPanelIsActive)
        {
            return;
        }
        else
        {
            SetActiveWorldPanel(true);
            SetActiveDungeonPanel(false);

            worldPanelIsActive = true;
        }
    }

    private void RightMouseDown(InputAction.CallbackContext context)
    {
        Debug.Log("야영지로 돌아가기");
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
        Debug.Log("던전3 이동");
    }

    public void Click_World1()
    {
        Debug.Log("월드1 이동");
    }
    public void Click_World2()
    {
        Debug.Log("월드2 이동");
    }
    public void Click_World3()
    {
        Debug.Log("월드3 이동");
    }
    public void Click_World4()
    {
        Debug.Log("월드4 이동");
    }
    public void Click_World5()
    {
        Debug.Log("월드5 이동");
    }


    #endregion
}
