using UnityEditor.Build.Content;
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
    [Tooltip("대사 패널")]
    public GameObject ScriptPanel;
    [Tooltip("사망 패널")]
    public GameObject DiePanel;
    [Tooltip("상호작용 패널")]
    public GameObject InterectionPanel; 

    [Header("PlayerUI")]
    [Tooltip("플레이어 체력이미지")]
    public Image[] healthImages;

    [Space(3)]
    [Header("ItemUI")]
    [Tooltip("아이템이미지")]
    public Image itemImage;
    [Tooltip("아이템사용키 텍스트")]
    public Text itemKey_text;
    [Tooltip("아이템 활성화 이미지")]
    [SerializeField] private Image itemSelectImage;

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
    [Header("Map")]
    [Tooltip("미니맵")]
    [SerializeField] private Image minimapImage;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private GameObject playerMarker;
    [Space]
    [SerializeField] private GameObject MapPanel;

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

    [Space(3)]
    [Header("Interection")]
    public Text interectionText;

    [Space(3)]
    [Header("Animator")]
    [SerializeField] private Animator Anim_RuneInventory;
    [Tooltip("옵션 패널 애니메이션")]
    [SerializeField] private Animator Anim_Setting;

    [Space(3)]
    [Header("HitEffect")]
    [SerializeField] private Image hitEffect;

    private UIInputAction uiInput;

    private int currentItemNum;

    private int currentHealth = 6;
    private const int MIN_HEALTH = 0;
    private const int MAX_HEALTH = 6;

    private float gaugeValue = 100f;
    private const float GAUGE_RECOVERY_VALUE = 0.1f;
    private const float GAUGE_MIN_VALUE = 0f;
    private const float GAUGE_MAX_VALUE = 100f;

    private bool isBigMapSize = false;
    private const float PLAYER_MARKER_BIGSIZE = 17f;
    private const float PLAYER_MARKER_SMALLSIZE = 5f;

    private bool isGaugeRecovery = true;

#if UNITY_EDITOR

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
    [ContextMenu("대사 시작")]
    private void StartScript()
    {
        UIWhiteBox.StartScripting(1100, 1104);
    }
    [ContextMenu("상호작용UI 활성화")]
    private void OnEnableInterectionUI()
    {
        Spawn_InterectionUI("F", "유물획득");
    }
    [ContextMenu("상호작용UI 비활성화")]
    private void OnDisableInterectionUI()
    {
        InterectionPanel_SetActive(false);
    }
    [ContextMenu("아이템 갯수 증가")]
    private void PlusItem()
    {
        GetItem();
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

        uiInput.MainUI.Map.started += MapAction;

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

        uiInput.MainUI.Map.started -= MapAction;

        #endregion
    }

    private void Start()
    {
        MainUI_Initialize();
    }

    private void Update()
    {
        if(isGaugeRecovery)
        {
            GaugeRecovery(GAUGE_RECOVERY_VALUE);
        }
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
                    UIWhiteBox.MainUICurrentState = MainUIState.PAUSE;
                    break;
                case MainUIState.PAUSE:
                    PausePanel_SetActive(false);
                    UIWhiteBox.MainUICurrentState = MainUIState.NONE;
                    break;
                case MainUIState.PAUSE_SETTING:
                    Anim_Setting.SetTrigger("Off");
                    UIWhiteBox.MainUICurrentState = MainUIState.PAUSE;
                    break;
                case MainUIState.Die:
                    Debug.LogWarning("MainUIState가 Die입니다.");
                    return;
                default:
                    Debug.LogWarning("MainUIState가 NONE, PAUSE, PAUSE_SETTING이 아닙니다.");
                    PausePanel_SetActive(true);
                    UIWhiteBox.MainUICurrentState = MainUIState.PAUSE;
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
                Anim_RuneInventory.SetTrigger("Off");
            }
            else
            {
                UIWhiteBox.SetRuneInventoryTitleText("인벤토리");
                RuneInventoryPanel_SetActive(true);
            }
        }
    }

    private void MapAction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(!isBigMapSize)
            {
                isBigMapSize = true;

                Vector3 bigSize = new Vector3(PLAYER_MARKER_BIGSIZE, PLAYER_MARKER_BIGSIZE, PLAYER_MARKER_BIGSIZE);
                playerMarker.transform.localScale = bigSize;
                MapPanel_SetActive(true);
            }
            else
            {
                isBigMapSize = false;

                Vector3 smallSize = new Vector3(PLAYER_MARKER_SMALLSIZE, PLAYER_MARKER_SMALLSIZE, PLAYER_MARKER_SMALLSIZE);
                playerMarker.transform.localScale = smallSize;
                MapPanel_SetActive(false);
            }
        }
    }

    #endregion

    /// <summary>
    /// 메인UI 초기화 메서드
    /// </summary>
    private void MainUI_Initialize()
    {
        //상호작용 패널 비활성화
        InterectionPanel_SetActive(false);

        //일시정지 패널 비활성화
        PausePanel_SetActive(false);

        //룬 선택 패널 비활성화
        RunePanel_SetActive(false);

        //룬 인벤토리 패널 비활성화
        RuneInventoryPanel_SetActive(false);

        //대사 패널 비활성화
        ScriptPanel_SetActive(false);

        //맵 패널 비활성화
        MapPanel_SetActive(false);

        //사망 패널 비활성화
        DiePanel_SetActive(false);

        //아이템선택창 비활성화
        ChoiceUI_SetActive(false);

        //아이템 선택 표시 비활성화
        ItemSelectImage_SetActive(false);

        //게이지이미지 비활성화
        GaugeUI_SetActive(false);

        //원거리공격 에임 이미지 비활성화
        Crosshair_SetActive(false);

        //게이지 값 적용
        gaugeValue = GAUGE_MAX_VALUE;
        gaugeImage.fillAmount = gaugeValue / GAUGE_MAX_VALUE;
    }

    #region SetActiveFunc

    public void ItemSelectImage_SetActive(bool active)
    {
        itemSelectImage.gameObject.SetActive(active);
    }

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

    public void ScriptPanel_SetActive(bool active)
    {
        ScriptPanel.SetActive(active);

        CursorLock(active);
    }

    public void MapPanel_SetActive(bool active)
    {
        MapPanel.SetActive(active);

        CursorLock(active);
    }

    public void DiePanel_SetActive(bool active)
    {
        DiePanel.SetActive(active);

        CursorLock(active);
    }

    public void InterectionPanel_SetActive(bool active)
    {
        InterectionPanel.SetActive(active);

        CursorLock(active);
    }

    #endregion

    /// <summary>
    /// 마우스 커서 잠금 & 표시
    /// </summary>
    /// <param name="isLock">잠금 여부 true: 잠금해제 & 표시 / false: 잠금 & 표시되지않게 </param>
    public void CursorLock(bool isLock)
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
        hitEffect.gameObject.SetActive(true);

        Debug.Log($"피격! 현재 체력: {currentHealth}");

        if(currentHealth <= MIN_HEALTH)
        {
            UIWhiteBox.MainUICurrentState = MainUIState.Die;
            DiePanel_SetActive(true);
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

        itemImage.sprite = item_ImageResources[currentItemNum + 1];
    }

    /// <summary>
    /// 아이템 리스트 재설정
    /// </summary>
    public void SetItemList()
    {
        int itemCount = DataManager.Instance.GetCurrentData().currentItemList;

        if(itemCount == 0)
        {
            for(int i = 0; i < itemChoice_Buttons.Length; i++)
            {
                itemChoice_Buttons[i].gameObject.SetActive(false);
            }

            //아이템 이미지
            itemImage.sprite = item_ImageResources[itemCount];

            //아이템 텍스트
            ChangeItemName_UI("");
            Debug.LogWarning($"현재 아이템의 갯수가 {itemCount}개입니다.");

            return;
        }

        for(int i = 0; i < itemChoice_Buttons.Length; i++)
        {
            itemChoice_Buttons[i].gameObject.SetActive(i < itemCount);
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

    public float GetGaugeValue()
    {
        return gaugeValue;
    }

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

    /// <summary>
    /// 게이지 회복 여부 결정
    /// </summary>
    /// <param name="newValue">true: 회복 / false: 멈춤</param>
    public void SetisGaugeRecovery(bool newValue)
    {
        isGaugeRecovery = newValue;
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

    #region InterectionUI

    /// <summary>
    /// 상호작용 UI소환
    /// </summary>
    /// <param name="interectionBindKey">상호작용 키</param>
    /// <param name="interectionReturnValue">상호작용 텍스트</param>
    public void Spawn_InterectionUI(string interectionBindKey, string interectionReturnValue)
    {
        interectionText.text = "\"" + interectionBindKey + "\" " + interectionReturnValue;

        InterectionPanel_SetActive(true);
    }

    #endregion
}
