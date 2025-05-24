using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [Header("Panel")]
    [Tooltip("메뉴 패널")]
    [SerializeField] private GameObject MenuPanel;
    [Tooltip("저장데이터 패널")]
    [SerializeField] private GameObject StorageDataPanel;

    #region AnimEventKey

    public void StorageDataPanelIsActive()
    {
        MenuPanel.SetActive(false);
        StorageDataPanel.SetActive(true);
    }

    public void DisableImage()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
