using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuneChoiceUI : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("룬데이터")]
    public ItemDataReader runeData;
    [Tooltip("룬이미지리소스")]
    public ItemImageData runeResourceImages;

    [Space(2)]
    [Header("UI")]
    [SerializeField] private Text titleText;
    [SerializeField] private Button[] runeButton;
    [SerializeField] private Image[] runeImage;
    [SerializeField] private Text[] runeName;
    [SerializeField] private Text[] runeDescription;

    private int itemNumber;

    private const string RUNE_TITLE_CHOICE = "룬 선택";
    private const string RUNE_TITLE_GET = "룬 획득";

    private const int RUNE_BUTTON_POS_X_MIN = -550;
    private const int RUNE_BUTTON_POS_X_MAX = -50;

    private void Awake()
    {
        UIWhiteBox.SetRuneChoiceWB(this);
    }

    private void OnEnable()
    {
        if(UIWhiteBox.MainUICurrentState == MainUIState.NONE)
        {
            Debug.LogWarning("UIState이 NONE입니다. UIState을 설정해주세요.");
            return;
        }
        else if(UIWhiteBox.MainUICurrentState == MainUIState.RUNE_CHOICE)
        {
            //타이틀 "룬 선택"으로 변경
            titleText.text = RUNE_TITLE_CHOICE;
            //룬 갯수 3개
            itemNumber = 3;
            //룬 버튼 3개, 위치 재조정
            SwitchRuneChoiceUI(true, RUNE_BUTTON_POS_X_MIN);
        }
        else if(UIWhiteBox.MainUICurrentState == MainUIState.RUNE_GET)
        {
            //타이틀 "룬 획득"으로 변경
            titleText.text = RUNE_TITLE_GET;
            //룬 갯수 1개
            itemNumber = 1;
            //룬 버튼 1개, 위치 재조정
            SwitchRuneChoiceUI(false, RUNE_BUTTON_POS_X_MAX);
        }

        GetRandomID(itemNumber);
    }

    /// <summary>
    /// 룬 ChoiceUI <-> 룬 GetUI 변환
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="anchorPosX"></param>
    private void SwitchRuneChoiceUI(bool isActive, int anchorPosX)
    {
        for(int i = 1;  i < runeButton.Length; i++)
        {
            runeButton[i].gameObject.SetActive(isActive);
        }

        RectTransform rectT = runeButton[0].GetComponent<RectTransform>();
        rectT.anchoredPosition = new Vector2(anchorPosX, rectT.anchoredPosition.y);
        
    }

    /// <summary>
    /// 룬 ID 랜덤으로 가져오기(사용중인 룬 목록 제외)
    /// </summary>
    /// <param name="itemNumber">가져올 룬 갯수</param>
    private void GetRandomID(int itemNumber)
    {
        //현재 소유중인 룬 목록을 List으로 가져오기
        List<int> ownedRunes = UIWhiteBox.GetRuneIDs();

        // 전체 룬 데이터에서 소유중인 룬 제외
        List<ItemData> availableRunes = runeData.DataList.Where(rune => !ownedRunes.Contains(rune.id)).ToList();

        //n개 미만일때 예외처리
        if(availableRunes.Count < itemNumber)
        {
            Debug.LogWarning($"선택 가능한 룬이 {itemNumber}개 미만입니다.");
            return;
        }

        //그 중에서 랜덤으로 n개 선택
        List<ItemData> randomItems = availableRunes.OrderBy(x => Random.value).Take(itemNumber).ToList();
        
        //UI에 적용
        for (int i = 0; i < randomItems.Count; i++)
        {
            runeImage[i].sprite = runeResourceImages.itemIcon[randomItems[i].id - 1];
            runeName[i].text = randomItems[i].name.ToString();
            runeDescription[i].text = randomItems[i].description.ToString();
        }
    }

    #region ButtonEvent

    private string GetButtonName()
    {
        return EventSystem.current.currentSelectedGameObject.name;
    }

    public void Click_runeButton()
    {
        UIWhiteBox.SetActiveRunePanel(false);

        string buttonName = GetButtonName();
        int buttonNumber = int.Parse(buttonName.Substring(4, 1));

        ItemData selectedRune = runeData.DataList.FirstOrDefault(x => x.name == runeName[buttonNumber - 1].text);

        if(selectedRune != null)
        {
            UIWhiteBox.AddRuneToInventory(selectedRune);
            Debug.Log($"룬 이름: {runeName[buttonNumber - 1].text}");
            Debug.Log($"룬 설명: {runeDescription[buttonNumber - 1].text}");
        }
        else
        {
            Debug.Log("선택된 룬 정보를 찾을 수 없습니다.");
        }
    }

    #endregion
}
