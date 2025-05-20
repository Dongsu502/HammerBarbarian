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
    [SerializeField] 
    private GameObject[] runeButton;
    [SerializeField]
    private Image[] runeImage;
    [SerializeField]
    private Text[] runeName;
    [SerializeField]
    private Text[] runeDescription;

    private void Awake()
    {
        UIWhiteBox.SetRuneChoiceWB(this);
    }

    private void OnEnable()
    {
        GetRandomID();
    }

    private void GetRandomID()
    {
        //현재 소유중인 룬 목록을 List으로 가져오기
        List<int> ownedRunes = UIWhiteBox.GetRuneIDs();

        // 전체 룬 데이터에서 소유중인 룬 제외
        List<ItemData> availableRunes = runeData.DataList.Where(rune => !ownedRunes.Contains(rune.id)).ToList();

        //3개 미만일때 예외처리
        if(availableRunes.Count < 3)
        {
            Debug.LogWarning("선택 가능한 룬이 3개 미만입니다.");
            return;
        }

        //그 중에서 랜덤으로 3개 선택
        List<ItemData> randomItems = availableRunes.OrderBy(x => Random.value).Take(3).ToList();
        
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
