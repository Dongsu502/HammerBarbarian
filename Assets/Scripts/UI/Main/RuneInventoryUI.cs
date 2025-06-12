using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class RuneInventoryUI : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("룬 데이터")]
    public ItemDataReader runeData;
    [Tooltip("룬이미지 리소스")]
    public ItemImageData runeResourceImages;

    [Space(2)]
    [Header("Popup")]
    [SerializeField]
    private GameObject RePlacementPopup;

    [Space(2)]
    [Header("Title")]
    [SerializeField]
    private Text titleText;

    [Space(2)]
    [Header("UI")]
    [SerializeField]
    private GameObject[] runeButton;
    [SerializeField]
    private Image[] runeImage;
    [SerializeField]
    private Text[] runeName;
    [SerializeField]
    private Text[] runeDescription;

    public List<int> runeIDs { get; private set; }

    [SerializeField]
    private bool isChangeRune;
    [SerializeField]
    private int ClickRuneSlot;

    ItemData choiceRune;

    private void Awake()
    {
        UIWhiteBox.SetRuneInventoryWB(this);

        runeIDs = new List<int>();
    }

    private void OnEnable()
    {
        if (!UIWhiteBox.isStart)
        {
            SoundManager.instance.PlayUI("UI_OptionDrag_Open");
        }

        //팝업창 비활성화
        RePlacementPopup.SetActive(false);

        UIWhiteBox.MainUICurrentState = MainUIState.RUNE_INVENTORY;
    }

    private void OnDisable()
    {
        UIWhiteBox.MainUICurrentState = MainUIState.NONE;
    }

    private void Start()
    {
        InitializeSlot();
    }

    /// <summary>
    /// 슬롯 비우기
    /// </summary>
    [ContextMenu("슬롯 비우기")]
    public void InitializeSlot()
    {
        for (int i = 0; i < runeButton.Length; i++)
        {
            runeButton[i].SetActive(false);
        }

        //룬 표시 이미지 색깔 초기화
        UIWhiteBox.ResetColor_RuneShowImage();
    }

    /// <summary>
    /// 타이틀 텍스트 변경
    /// </summary>
    /// <param name="newText">변경 텍스트값</param>
    public void ChangeTitleText(string newText)
    {
        titleText.text = newText;
    }

    /// <summary>
    /// 비어있는 슬롯을 확인하여 해당 인덱스를 리턴하는 메서드(전부 차있으면 -1리턴)
    /// </summary>
    /// <returns>비어있는 슬롯인덱스</returns>
    private int CheckInventorySlot()
    {
        for(int i = 0; i < runeButton.Length; i++)
        {
            if (!runeButton[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 룬 선택 시 해당 룬이미지와 룬이름, 설명을 받아서 인벤토리에 넣기
    /// </summary>
    /// <param name="rune">추가할 룬</param>
    public void AddRuneToInventory(ItemData rune)
    {
        choiceRune = rune;
        int slotIndex = CheckInventorySlot();
        if(slotIndex == -1)
        {
            Debug.LogWarning("모든 룬 슬롯이 가득 찼습니다.");
            isChangeRune = true;

            //룬 인벤토리 교체모드로 바꿔서 열기
            UIWhiteBox.SetRuneInventoryTitleText("교체 선택");
            UIWhiteBox.SetActiveRuneInventoryPanel(true);

            return;
        }

        //슬롯 활성화
        runeButton[slotIndex].SetActive(true);
        //Ui 적용
        RuneUIApplication(slotIndex, rune);

        //인벤토리의 룬 아이디 추가
        runeIDs.Add(rune.id);
        Debug.LogWarning($"인벤토리 룬아이디: " + string.Join(", ", runeIDs));

        //룬 표시 이미지 색깔 변경
        UIWhiteBox.SetColor_RuneShowImage(slotIndex);
    }

    private void RuneUIApplication(int slotIndex, ItemData rune)
    {
        runeImage[slotIndex].sprite = runeResourceImages.itemIcon[rune.id - 1];
        runeName[slotIndex].text = rune.name;
        runeDescription[slotIndex].text = rune.description;

        //Data에 룬 추가
        if (!DataManager.Instance.GetCurrentData().ownedRunes.Contains(rune.id))
        {
            DataManager.Instance.GetCurrentData().ownedRunes.Add(rune.id);
        }
    }

    #region ButtonEvent

    public void Click_YesButton()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        //룬 UI 적용
        RuneUIApplication(ClickRuneSlot, choiceRune);

        //인벤토리의 룬 아이디 교체
        runeIDs[ClickRuneSlot] = choiceRune.id;
        Debug.LogWarning($"인벤토리 룬아이디: " + string.Join(", ", runeIDs));

        //패널 비활성화
        UIWhiteBox.SetActiveRuneInventoryPanel(false);
    }

    public void Click_NoButton()
    {
        SoundManager.instance.PlayUI("UI_Botton05");

        //패널 비활성화
        UIWhiteBox.SetActiveRuneInventoryPanel(false);
    }

    public void Click_RuneButton1()
    {
        SoundManager.instance.PlayUI("UI_Botton07");

        if (!isChangeRune) return;

        //룬 슬롯번호 변경
        ClickRuneSlot = 0;

        //팝업창 활성화
        RePlacementPopup.SetActive(true);

        isChangeRune = false;
    }

    public void Click_RuneButton2()
    {
        SoundManager.instance.PlayUI("UI_Botton07");

        if (!isChangeRune) return;

        //룬 슬롯번호 변경
        ClickRuneSlot = 1;

        //팝업창 활성화
        RePlacementPopup.SetActive(true);

        isChangeRune = false;
    }

    public void Click_RuneButton3()
    {
        SoundManager.instance.PlayUI("UI_Botton07");

        if (!isChangeRune) return;

        //룬 슬롯번호 변경
        ClickRuneSlot = 2;

        //팝업창 활성화
        RePlacementPopup.SetActive(true);

        isChangeRune = false;
    }

    #endregion

    #region AnimEventKey

    public void DisableRuneInventory()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
