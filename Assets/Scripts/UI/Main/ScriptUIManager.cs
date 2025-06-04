using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ScriptUIManager : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("대사 시트 데이터")]
    public ItemDataReader scriptData;

    [Header("UI")]
    [Tooltip("이름텍스트")]
    [SerializeField] Text Name;
    [Tooltip("대사")]
    [SerializeField] Text Script;

    private UIInputAction inputAction;

    private List<ItemData> currentScriptList = new List<ItemData>();
    private int currentIndex = 0;

    #region UnityFunc

    private void Awake()
    {
        UIWhiteBox.SetScriptUIWB(this);

        inputAction = new UIInputAction();
    }

    private void OnEnable()
    {
        inputAction.MainUI.Enable();

        inputAction.MainUI.Script.started += Input_F;

        WorldWhiteBox.WhiteBox.PauseGame();
    }

    private void OnDisable()
    {
        inputAction.MainUI.Disable();

        inputAction.MainUI.Script.started -= Input_F;

        WorldWhiteBox.WhiteBox.ResumeGame();
    }

    #endregion

    /// <summary>
    /// 원하는 id값의 데이터가져오기
    /// </summary>
    /// <param name="start">시작id</param>
    /// <param name="end">마지막id</param>
    public void GetScriptData(int start,  int end)
    {
        currentScriptList.Clear();
        currentIndex = 0;

        //start부터 end까지의 데이터 리스트 가져오기
        for(int i = 0; i < scriptData.DataList.Count; i++)
        {
            int id = scriptData.DataList[i].id;
            if(id >= start && id <= end)
            {
                currentScriptList.Add(scriptData.DataList[i]);
            }
        }

        //첫번째 대사 UI에 표시
        if(currentScriptList.Count > 0)
        {
            SetScriptUI(currentScriptList[0].name, currentScriptList[0].description);
        }
        else
        {
            Debug.LogWarning("선택된 범위에 대사가 없습니다.");
        }
    }

    /// <summary>
    /// 화면 UI에 이름, 대사 텍스트 연결
    /// </summary>
    /// <param name="newName">이름</param>
    /// <param name="newScript">대사</param>
    private void SetScriptUI(string newName, string newScript)
    {
        Name.text = newName;

        Script.text = newScript;
    }

    #region InputAction

    private void Input_F(InputAction.CallbackContext context)
    {
        if(context.started && UIWhiteBox.MainUICurrentState == MainUIState.SCRIPT)
        {
            NextScript();
        }
    }

    #endregion

    #region ButtonEvent
    //버튼 클릭 이벤트로 다음 대사로 넘기기
    public void NextScript()
    {
        currentIndex++;

        if(currentIndex < currentScriptList.Count)
        {
            var data = currentScriptList[currentIndex];
            SetScriptUI (data.name, data.description);
        }
        else
        {
            UIWhiteBox.SetActiveScriptUIPanel(false);
        }
    }

    #endregion
}
