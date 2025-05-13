using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class TitleDataUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Panel")]
    [SerializeField]
    private GameObject ResetDataPopup;

    private UIInputAction uiInput;

    private bool[] isClickButton = new bool[3];

    [SerializeField]
    private int selectIndex = -1;

    #region UnityFunc

    private void Awake()
    {
        uiInput = new UIInputAction();
    }

    private void OnEnable()
    {
        SetActive_ResetDataPopup(false);

        uiInput.TitleUI.Enable();

        uiInput.TitleUI.DeleteData.started += DeleteKeyAction;
        uiInput.TitleUI.Mouse.started += RightMouseKeyAction;
    }

    private void OnDisable()
    {
        uiInput.TitleUI.Disable();

        uiInput.TitleUI.DeleteData.started -= DeleteKeyAction;
        uiInput.TitleUI.Mouse.started -= RightMouseKeyAction;
    }

    #endregion

    #region InputAction

    private void DeleteKeyAction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            SetActive_ResetDataPopup(true);
        }
    }

    private void RightMouseKeyAction(InputAction.CallbackContext context)
    {
        Debug.Log("메뉴로 돌아가기");
        selectIndex = -1;
    }

    #endregion

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData)
    {
        selectIndex = -1;
    }

    #endregion

    private void SetActive_ResetDataPopup(bool isActive)
    {
        ResetDataPopup.SetActive(isActive);
    }

    /// <summary>
    /// 모든 버튼 isClickButton true로 초기화
    /// </summary>
    private void DeselectAll()
    {
        for (int i = 0; i < isClickButton.Length; i++)
        {
            isClickButton[i] = false;
        }
    }

    /// <summary>
    /// 데이터 불러오기
    /// </summary>
    private void LoadData(int index)
    {
        DataManager.Instance.SetCurrentData(index);

        switch (index)
        {
            case 0:
                string dataFile1 = DataManager.Instance.currentDataFileName;
                DataManager.Instance.data1 = DataManager.Instance.LoadGameData<Data1>(dataFile1);
                break;

            case 1:
                string dataFile2 = DataManager.Instance.currentDataFileName;
                DataManager.Instance.data2 = DataManager.Instance.LoadGameData<Data2>(dataFile2);
                break;

            case 2:
                string dataFile3 = DataManager.Instance.currentDataFileName;
                DataManager.Instance.data3 = DataManager.Instance.LoadGameData<Data3>(dataFile3);
                break;
        }
        Debug.Log($"현재 데이터 파일: {DataManager.Instance.currentDataFileName}");
    }

    #region ButtonEvent

    public void Click_YesButton()
    {
        switch (selectIndex)
        {
            case -1:
                Debug.LogWarning("선택된 버튼이 없습니다.");
                return;

            case 0:
                DataManager.Instance.ResetData(DataManager.Instance.data1);
                break;

            case 1:
                DataManager.Instance.ResetData(DataManager.Instance.data2);
                break;

            case 2:
                DataManager.Instance.ResetData(DataManager.Instance.data3);
                break;
        }
        SetActive_ResetDataPopup(false);
    }

    public void Click_NoButton()
    {
        SetActive_ResetDataPopup(false);
        selectIndex = -1;
    }

    public void Click_Data1Button()
    {
        selectIndex = 0;

        if (!isClickButton[selectIndex]) 
        {
            DeselectAll();
            isClickButton[selectIndex] = true;
            
            return;
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
    }

    public void Click_Data2Button()
    {
        selectIndex = 1;

        if (!isClickButton[selectIndex])
        {
            DeselectAll();
            isClickButton[selectIndex] = true;

            return;
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
    }

    public void Click_Data3Button()
    {
        selectIndex = 2;

        if (!isClickButton[selectIndex])
        {
            DeselectAll();
            isClickButton[selectIndex] = true;

            return;
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
    }

    #endregion
}
