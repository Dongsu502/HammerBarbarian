using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainUI_MouseHoverCheck : MonoBehaviour, IPointerEnterHandler
{
    private MainUIManager mainUIManager;

    private string currentItemName;

    private void Start()
    {
        mainUIManager = GetComponentInParent<MainUIManager>();
    }

    #region IPointerEnterHandler_Func

    public void OnPointerEnter(PointerEventData eventData)
    {
        string buttonName = gameObject.name;
        string hoverButtonNumber = buttonName.Substring(buttonName.Length - 1, 1);

        int buttonNumber = int.Parse(hoverButtonNumber);

        currentItemName = mainUIManager.item_NameTexts[buttonNumber];
        mainUIManager.ChangeItemName_UI(currentItemName);
    }

    #endregion
}
