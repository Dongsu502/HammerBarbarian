using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject KeyDescriptionPanel;

    private void Awake()
    {
        UIWhiteBox.SetTutorialUIWB(this);
    }

    private void Start()
    {
        SetActiveKeyPanel(false);
    }

    public void SetActiveKeyPanel(bool isActive)
    {
        KeyDescriptionPanel.SetActive(isActive);

        UIWhiteBox.CursorLock(isActive);
    }

    #region ButtonEvent

    public void Click_ExitButton()
    {
        SetActiveKeyPanel(false);
    }

    public void Click_PreviousButton()
    {
        Debug.LogError("왼쪽화살표 클릭");
    }

    public void Click_NextButton()
    {
        Debug.LogError("오른쪽화살표 클릭");
    }

    #endregion
}
