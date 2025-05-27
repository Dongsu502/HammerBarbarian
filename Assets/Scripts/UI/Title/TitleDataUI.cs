using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TitleDataUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Panel")]
    [SerializeField] private GameObject ResetDataPopup;

    [Header("Button")]
    [SerializeField] private Text button1WorldName;
    [SerializeField] private Text button2WorldName;
    [SerializeField] private Text button3WorldName;

    private UIInputAction uiInput;

    [SerializeField] private bool[] isClickButton = new bool[3];

    [SerializeField] private int selectIndex = -1;

    #region UnityFunc

    private void Awake()
    {
        uiInput = new UIInputAction();
    }

    private void OnEnable()
    {
        SetActive_ResetDataPopup(false);
        DeselectAll();
        selectIndex = -1;

        uiInput.TitleUI.Enable();

        uiInput.TitleUI.DeleteData.started += DeleteKeyAction;
        uiInput.TitleUI.Mouse.started += RightMouseKeyAction;

        SetButtonWorldName();
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
        UIWhiteBox.GobackMenu();
    }

    #endregion

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData)
    {
        DeselectAll();
        selectIndex = -1;
    }

    #endregion

    private void SetButtonWorldName()
    {
        bool isExist1 = DataManager.Instance.NeedToCreateNewDataFile(0);
        if (isExist1)
        {
            button1WorldName.text = "데이터 없음";
        }
        else
        {
            int worldNumber = DataManager.Instance.data1.GetCurrentWorldDataNumber();
            button1WorldName.text = "월드 " + worldNumber;
        }

        bool isExist2 = DataManager.Instance.NeedToCreateNewDataFile(1);
        if (isExist2)
        {
            button2WorldName.text = "데이터 없음";
        }
        else
        {
            int worldNumber = DataManager.Instance.data2.GetCurrentWorldDataNumber();
            button2WorldName.text = "월드 " + worldNumber;
        }

        bool isExist3 = DataManager.Instance.NeedToCreateNewDataFile(2);
        if (isExist3)
        {
            button3WorldName.text = "데이터 없음";
            
        }
        else
        {
            int worldNumber = DataManager.Instance.data3.GetCurrentWorldDataNumber();
            button3WorldName.text = "월드 " + worldNumber;
        }
    }

    private void SetActive_ResetDataPopup(bool isActive)
    {
        ResetDataPopup.SetActive(isActive);
    }

    /// <summary>
    /// 모든 버튼 isClickButton false로 초기화
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
                DataManager.Instance.DeleteDataFile(selectIndex);
                break;

            case 1:
                DataManager.Instance.DeleteDataFile(selectIndex);
                break;

            case 2:
                DataManager.Instance.DeleteDataFile(selectIndex);
                break;
        }
        
        //새 게임 시작인지 확인
        if(UIWhiteBox.GetisNewGame())
        {
            LoadData(selectIndex);
            isClickButton[selectIndex] = false;
            SceneManager.LoadScene("ChoiceDungeon");
        }
        else
        {
            SetActive_ResetDataPopup(false);
            DeselectAll();
            selectIndex = -1;
        }
    }

    public void Click_NoButton()
    {
        SetActive_ResetDataPopup(false);
        DeselectAll();
        selectIndex = -1;
    }

    public void Click_Data1Button()
    {
        selectIndex = 0;

        //버튼 선택되었는지 확인
        if (!isClickButton[selectIndex]) 
        {
            DeselectAll();
            isClickButton[selectIndex] = true;
            
            return;
        }

        //새 게임 시작인지 확인
        if(UIWhiteBox.GetisNewGame())
        {
            bool isExist = DataManager.Instance.NeedToCreateNewDataFile(0);

            //데이터 파일이 존재한다면
            if(!isExist)
            {
                SetActive_ResetDataPopup(true);

                return;
            }
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
        UIWhiteBox.SceneName = "ChoiceDungeon";
        SceneManager.LoadScene("Loading");
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

        //새 게임 시작인지 확인
        if (UIWhiteBox.GetisNewGame())
        {
            bool isExist = DataManager.Instance.NeedToCreateNewDataFile(1);

            //데이터 파일이 존재한다면
            if (!isExist)
            {
                SetActive_ResetDataPopup(true);

                return;
            }
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
        UIWhiteBox.SceneName = "ChoiceDungeon";
        SceneManager.LoadScene("Loading");
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

        //새 게임 시작인지 확인
        if (UIWhiteBox.GetisNewGame())
        {
            bool isExist = DataManager.Instance.NeedToCreateNewDataFile(2);

            //데이터 파일이 존재한다면
            if (!isExist)
            {
                SetActive_ResetDataPopup(true);

                return;
            }
        }

        LoadData(selectIndex);
        isClickButton[selectIndex] = false;
        UIWhiteBox.SceneName = "ChoiceDungeon";
        SceneManager.LoadScene("Loading");
    }

    #endregion
}
