using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    [Header("Panel")]
    [Tooltip("일시정지 패널")]
    public GameObject PausePanel;
    [Tooltip("룬 선택 패널")]
    public GameObject RunePanel;
    [Tooltip("룬 인벤토리 패널")]
    public GameObject RuneInventoryPanel;

    [Header("PlayerUI")]
    [Tooltip("플레이어 체력이미지")]
    public Image[] healthImages;

    [Space(3)]
    [Header("ItemUI")]
    [Tooltip("아이템이미지")]
    public Image itemImage;
    [Tooltip("아이템사용키 텍스트")]
    public Text itemKey_text;

    [Space(1)]
    [Tooltip("아이템 선택창")]
    public GameObject itemChoiceUI;
    [Tooltip("아이템 이름")]
    public Text itemName_text;
    [Tooltip("아이템 선택 버튼")]
    public Button[] itemChoice_Buttons;
    [Tooltip("아이템 선택창의 아이템 이미지")]
    public Image[] itemChoice_itemImages;

    [Space(3)]
    [Header("Gauge")]
    [Tooltip("게이지 백그라운드이미지")]
    public Image gaugeBackgroundImage;
    [Tooltip("게이지 이미지")]
    public Image gaugeImage;
    [Tooltip("게이지 색상 그라데이션")]
    [SerializeField]
    private Gradient gaugeGradient;

    [Space(3)]
    [Header("Crosshair")]
    [Tooltip("원거리공격 에임")]
    public Image crossHairImage;

    [Space(3)]
    [Header("RuneShow UI")]
    public Image[] runeShowImages;

    [Space(3)]
    [Header("Image Resources")]
    [Tooltip("ItemImages")]
    public Sprite[] item_ImageResources;
    [Tooltip("HealthImages")]
    public Sprite[] health_ImageResources;

    [Space(3)]
    [Header("ItemNameText")]
    public string[] item_NameTexts;

    private UIInputAction uiInput;

    private int currentItemNum;

    private int currentHealth = 6;
    private const int MIN_HEALTH = 0;
    private const int MAX_HEALTH = 6;

    private float gaugeValue = 100f;
    private const float GAUGE_RECOVERY_VALUE = 0.1f;
    private const float GAUGE_MIN_VALUE = 0f;
    private const float GAUGE_MAX_VALUE = 100f;

#if UNITY_EDITOR

    [ContextMenu("피격1")]
    private void Hit1()
    {
        TakeDamage(1);
    }
    [ContextMenu("피격2")]
    private void Hit2()
    {
        TakeDamage(2);
    }
    [ContextMenu("피격3")]
    private void Hit3()
    {
        TakeDamage(3);
    }
    [ContextMenu("회복1")]
    private void Heal1()
    {
        Heal(1);
    }
    [ContextMenu("회복2")]
    private void Heal2()
    {
        Heal(2);
    }
    [ContextMenu("회복3")]
    private void Heal3()
    {
        Heal(3);
    }
    [ContextMenu("게이지 감소 30")]
    private void GaugeUse30()
    {
        UseGauge(30f);
    }
    [ContextMenu("룬 선택창으로 설정")]
    private void SetRuneChoice()
    {
        UIWhiteBox.MainUICurrentState = MainUIState.RUNE_CHOICE;
    }
    [ContextMenu("룬 획득창으로 설정")]
    private void SetRuneGet()
    {
        UIWhiteBox.MainUICurrentState = MainUIState.RUNE_GET;
    }
    [ContextMenu("룬 선택창 활성화")]
    private void RunePanelEnable()
    {
        RunePanel_SetActive(true);
    }

#endif

    #region UnityCall_Func
    private void Awake()
    {
        UIWhiteBox.SetMainUIWB(this);

        uiInput = new UIInputAction();
    }

    private void OnEnable()
    {
        #region UIKeyAction
        uiInput.MainUI.Enable();

        uiInput.MainUI.Setting.started += EscapeACtion;

        uiInput.MainUI.ChoiceItem.performed += ChoiceItemAction;
        uiInput.MainUI.ChoiceItem.canceled += ChoiceItemAction;

        uiInput.MainUI.Rune.started += RuneInventoryAction;

        #endregion

        UIWhiteBox.MainUICurrentState = MainUIState.NONE;

        SetItemList();
    }

    private void OnDisable()
    {
        #region UIKeyAction

        uiInput.MainUI.Disable();

        uiInput.MainUI.Setting.started -= EscapeACtion;

        uiInput.MainUI.ChoiceItem.performed -= ChoiceItemAction;
        uiInput.MainUI.ChoiceItem.canceled -= ChoiceItemAction;

        uiInput.MainUI.Rune.started -= RuneInventoryAction;

        #endregion
    }

    private void Start()
    {
        MainUI_Initialize();
    }

    private void Update()
    {
        GaugeRecovery(GAUGE_RECOVERY_VALUE);
    }

    #endregion

    #region InputAction

    private void EscapeACtion(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            switch(UIWhiteBox.MainUICurrentState)
            {
                case MainUIState.NONE:
                    PausePanel_SetActive(true);
                    UIWhiteBox.MainUICurrentState = MainUIState.PUASE;
                    break;
                case MainUIState.PUASE:
                    PausePanel_SetActive(false);
                    UIWhiteBox.MainUICurrentState = MainUIState.NONE;
                    break;
                case MainUIState.PUASE_SETTING:
                    UIWhiteBox.SetActive_SettingPanel(false);
                    UIWhiteBox.MainUICurrentState = MainUIState.PUASE;
                    break;
            }
        }
        
    }

    private void ChoiceItemAction(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ChoiceUI_SetActive(true);
        }

        if(context.canceled)
        {
            ChoiceUI_SetActive(false);
        }
    }

    private void RuneInventoryAction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(RuneInventoryPanel.activeSelf)
            {
                RuneInventoryPanel_SetActive(false);
            }
            else
            {
                UIWhiteBox.SetRuneInventoryTitleText("인벤토리");
                RuneInventoryPanel_SetActive(true);
            }
        }
    }

    #endregion

    /// <summary>
    /// 메인UI 초기화 메서드
    /// </summary>
    private void MainUI_Initialize()
    {
        //일시정지 패널 비활성화
        PausePanel_SetActive(false);

        //룬 선택 패널 비활성화
        RunePanel_SetActive(false);

        //룬 인벤토리 패널 비활성화
        RuneInventoryPanel_SetActive(false);

        //아이템선택창 비활성화
        ChoiceUI_SetActive(false);

        //게이지이미지 비활성화
        GaugeUI_SetActive(false);

        //원거리공격 에임 이미지 비활성화
        Crosshair_SetActive(false);

        //게이지 값 적용
        gaugeValue = GAUGE_MAX_VALUE;
        gaugeImage.fillAmount = gaugeValue / GAUGE_MAX_VALUE;
    }

    #region panelMethod

    public void PausePanel_SetActive(bool active)
    {
        PausePanel.SetActive(active);

        CursorLock(active);
    }

    public void RunePanel_SetActive(bool active)
    {
        RunePanel.SetActive(active);

        CursorLock(active);
    }

    public void RuneInventoryPanel_SetActive(bool active)
    {
        RuneInventoryPanel.SetActive(active);

        CursorLock(active);
    }

    #endregion

    /// <summary>
    /// 마우스 커서 잠금 & 표시
    /// </summary>
    /// <param name="isLock">잠금 여부 true: 잠금해제 & 표시 / false: 잠금 & 표시되지않게 </param>
    private void CursorLock(bool isLock)
    {
        if(isLock)
        {
            Cursor.lockState = CursorLockMode.None;

            //플레이어 마우스 입력 비활성화
            PlayerWhiteBox.WhiteBox.DisableAttackAction();
            //플레이어 화면 잠금
            PlayerWhiteBox.WhiteBox.DisableLookAction();

            Debug.Log("Cursor unlocked");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            //플레이어 마우스 입력 활성화
            PlayerWhiteBox.WhiteBox.EnableAttack1Action();
            //플레이어 화면 잠금해제
            PlayerWhiteBox.WhiteBox.EnableLookAction();

            Debug.Log("Cursor locked");
        }
        Cursor.visible = isLock;
    }

    /// <summary>
    /// 십자선 활성화, 비활성화
    /// </summary>
    /// <param name="active">활성화 여부</param>
    public void Crosshair_SetActive(bool active)
    {
        crossHairImage.gameObject.SetActive(active);
    }

    #region Health_Func

    /// <summary>
    /// 하트 UI를 현재 체력 상태에 맞게 갱신
    /// </summary>
    private void UpdateHearts()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (i < currentHealth)
                healthImages[i].sprite = health_ImageResources[1];
            else
                healthImages[i].sprite = health_ImageResources[0];
        }
    }

    /// <summary>
    /// 피격 시 체력 1 감소
    /// </summary>
    /// /// <param name="amount">사라질 하트 수</param>
    public void TakeDamage(int amount)
    {
        if (currentHealth <= MIN_HEALTH) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, MIN_HEALTH, MAX_HEALTH);
        UpdateHearts();

        Debug.Log($"피격! 현재 체력: {currentHealth}");

        if(currentHealth <= MIN_HEALTH)
        {
            Debug.Log("사망");
        }
    }

    /// <summary>
    /// 지정한 수만큼 체력 회복
    /// </summary>
    /// <param name="amount">회복할 하트 수</param>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, MIN_HEALTH, MAX_HEALTH);

        UpdateHearts();

        Debug.Log($"회복! 현재 체력: {currentHealth}");
    }

    #endregion

    #region Item_Func

    /// <summary>
    /// 아이템 번호 리턴
    /// </summary>
    /// <returns>현재 아이템 번호</returns>
    public int UseItemNumber()
    {
        return currentItemNum;
    }

    /// <summary>
    /// 아이템 선택창 활성화 / 비활성화 메서드
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    private void ChoiceUI_SetActive(bool isActive)
    {
        itemChoiceUI.SetActive(isActive);

        CursorLock(isActive);
    }

    /// <summary>
    /// 아이템 선택창에서 아이템 이름 바꾸는 메서드
    /// </summary>
    /// <param name="newName">변경 아이템 이름</param>
    public void ChangeItemName_UI(string newName)
    {
        itemName_text.text = newName;
    }

    /// <summary>
    /// 아이템 선택 시 아이템 이미지 변경
    /// </summary>
    public void ChangeItem_UI()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        string pressedButtonNumber = buttonName.Substring(buttonName.Length - 1, 1);
        
        currentItemNum = int.Parse(pressedButtonNumber);

        itemImage.sprite = item_ImageResources[currentItemNum];
    }

    /// <summary>
    /// 아이템 리스트 재설정
    /// </summary>
    public void SetItemList()
    {
        int itemCount = DataManager.Instance.GetCurrentData().currentItemList;

        for(int i = 0; i < itemChoice_Buttons.Length; i++)
        {
            itemChoice_Buttons[i].gameObject.SetActive(i <= itemCount);
        }
        Debug.Log($"활성화된 아이템 수: {itemCount}");
    } 

    /// <summary>
    /// 아이템 획득
    /// </summary>
    public void GetItem()
    {
        DataManager.Instance.GetCurrentData().currentItemList++;

        //아이템 리스트 재설정
        SetItemList();
    }

    #endregion

    #region Gauge_Func

    /// <summary>
    /// 게이지이미지 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    private void GaugeUI_SetActive(bool isActive)
    {
        gaugeBackgroundImage.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 게이지 자동 회복
    /// </summary>
    /// <param name="amount">회복값(속도)</param>
    private void GaugeRecovery(float amount)
    {
        if (gaugeValue >= GAUGE_MAX_VALUE)
        {
            //게이지 비활성화
            GaugeUI_SetActive(false);

            return;
        }

        gaugeValue += amount;
        gaugeImage.fillAmount = gaugeValue / GAUGE_MAX_VALUE;
        UpdateGaugeColor();
    }

    /// <summary>
    /// 게이지 사용
    /// </summary>
    /// <param name="amount">사용할 게이지 양</param>
    public void UseGauge(float amount)
    {
        if (gaugeValue <= GAUGE_MIN_VALUE) return;

        //게이지 활성화
        GaugeUI_SetActive(true);

        gaugeValue -= amount;
        gaugeImage.fillAmount = gaugeValue / GAUGE_MAX_VALUE;
        UpdateGaugeColor();

        Debug.Log($"게이지 감소값: {amount}");
    }

    /// <summary>
    /// 게이지 색상 업데이트
    /// </summary>
    private void UpdateGaugeColor()
    {
        float normalized = gaugeValue / GAUGE_MAX_VALUE;
        gaugeImage.color = gaugeGradient.Evaluate(normalized);
    }

    #endregion

    #region Rune

    /// <summary>
    /// 룬 표시 이미지 색깔 초기화
    /// </summary>
    public void ResetColorRuneShowImage()
    {
        for(int i = 0; i < runeShowImages.Length; i++)
        {
            runeShowImages[i].color = Color.white;
        }
    }

    /// <summary>
    /// 인벤토리에 들어간 룬 표시 이미지 색깔 변경
    /// </summary>
    /// <param name="index">인벤토리에 들어간 룬 번호</param>
    public void SetColorRuneShowImage(int index)
    {
        runeShowImages[index].color = Color.red;
    }

    #endregion
}
