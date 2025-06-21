using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [Tooltip("저장 데이터 버튼")]
    [SerializeField] private GameObject storageDataButton;

    public bool emptyData = false;
    private const float DATABUTTON_TEXT_COLOR = 0.6830188f;

    private void OnEnable()
    {
        //데이터 버튼 활성화 여부
        SetActive_StorageDataButton();
    }

    private void SetActive_StorageDataButton()
    {
        bool data1 = DataManager.Instance.NeedToCreateNewDataFile(0);
        bool data2 = DataManager.Instance.NeedToCreateNewDataFile(1);
        bool data3 = DataManager.Instance.NeedToCreateNewDataFile(2);
        float dataTextColor_A;

        if (data1 && data2 && data3)
        {
            emptyData = true;
            //글자 반투명
            dataTextColor_A = 0.07843138f;
        }
        else
        {
            emptyData = false;
            //글자 투명x
            dataTextColor_A = 1f;
        }
        storageDataButton.GetComponentInChildren<Text>().color
            = new Color(DATABUTTON_TEXT_COLOR, DATABUTTON_TEXT_COLOR, DATABUTTON_TEXT_COLOR, dataTextColor_A);
    }
}
