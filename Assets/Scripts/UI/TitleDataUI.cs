using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDataUI : MonoBehaviour
{
    #region ButtonEvent

    public void Click_Data1Button()
    {
        string data1fileName = DataManager.Instance.GameDataFileName1;
        DataManager.Instance.currentDataFileName = data1fileName;
        DataManager.Instance.data1 = DataManager.Instance.LoadGameData<Data1>(data1fileName);

        Debug.Log($"현재 데이터 파일: {DataManager.Instance.currentDataFileName}");

        //아이템 리스트 초기화
        UIWhiteBox.SetItemList();
    }

    public void Click_Data2Button()
    {
        string data1fileName = DataManager.Instance.GameDataFileName2;
        DataManager.Instance.currentDataFileName = data1fileName;
        DataManager.Instance.data2 = DataManager.Instance.LoadGameData<Data2>(data1fileName);

        Debug.Log($"현재 데이터 파일: {DataManager.Instance.currentDataFileName}");

        //아이템 리스트 초기화
        UIWhiteBox.SetItemList();
    }

    public void Click_Data3Button()
    {
        string data1fileName = DataManager.Instance.GameDataFileName3;
        DataManager.Instance.currentDataFileName = data1fileName;
        DataManager.Instance.data3 = DataManager.Instance.LoadGameData<Data3>(data1fileName);

        Debug.Log($"현재 데이터 파일: {DataManager.Instance.currentDataFileName}");

        //아이템 리스트 초기화
        UIWhiteBox.SetItemList();
    }

    #endregion
}
