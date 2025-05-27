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

    private UIInputAction uiInput;

    private void Awake()
    {
        uiInput = new UIInputAction();
    }

    private void OnEnable()
    {
        uiInput.ChoiceMapUI.Enable();

        uiInput.ChoiceMapUI.OpenWorldMap.started += R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started += RightMouseDown;

        InitializeChoiceUI();
    }
    private void OnDisable()
    {
        uiInput.ChoiceMapUI.Disable();

        uiInput.ChoiceMapUI.OpenWorldMap.started -= R_KeyDown;
        uiInput.ChoiceMapUI.Mouse.started -= RightMouseDown;
    }

    private void InitializeChoiceUI()
    {
        DungeonPanel.SetActive(true);
        WorldPanel.SetActive(false);

        UIWhiteBox.ChoiceMapUICurrentState = ChoiceMapUIState.DUNGEON;

        //SetActiveDungeonButtons(0, 3);
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

    /// <summary>
    /// 던전패널 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveDungeonPanel(bool isActive)
    {
        DungeonPanel.SetActive(isActive);
    }

    /// <summary>
    /// 던전 버튼 활성화 (일단 보류)
    /// </summary>
    /// <param name="worldNumber">월드 번호</param>
    /// <param name="dungeonIndex">던전 갯수</param>
    //private void SetActiveDungeonButtons(int worldNumber, int dungeonIndex)
    //{
    //    BaseData currentData = DataManager.Instance.GetCurrentData();

    //    for(int i = 0; i < dungeonIndex; i++)
    //    {
    //        DungeonButtons[i].gameObject.SetActive(false);

    //        if (currentData.worlds[worldNumber].dungeons[i] == true)
    //        {
    //            DungeonButtons[i].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            return;
    //        }
    //    }
        
    //}

    /// <summary>
    /// 월드패널 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActiveWorldPanel(bool isActive)
    {
        WorldPanel.SetActive(isActive);
    }

    /// <summary>
    /// 월드 버튼 활성화 (일단 보류)
    /// </summary>
    /// <param name="index"></param>
    //private void SetActiveWorldButtons(int index)
    //{
    //    BaseData currentData = DataManager.Instance.GetCurrentData();

    //    for(int i = 0; i < index; i++)
    //    {
            
    //    }
    //}

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
        UIWhiteBox.SceneName = "Map";
        SceneManager.LoadScene("Loading");
    }

    public void Click_World1()
    {
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
