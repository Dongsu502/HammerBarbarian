using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RuneChoiceUI : MonoBehaviour
{
    [Header("Data")]
    public ItemDataReader runeData;

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

    [Space(2)]
    [Header("룬 이미지")]
    [SerializeField]
    private Sprite[] runeResourceImages;

    private void OnEnable()
    {
        GetRandomID();
    }

    private void GetRandomID()
    {
        // 전체 리스트에서 랜덤하게 3개 선택 (중복 없음)
        List<ItemData> randomItems = runeData.DataList.OrderBy(x => Random.value).Take(3).ToList();

        for (int i = 0; i < randomItems.Count; i++)
        {
            runeImage[i].sprite = runeResourceImages[randomItems[i].id - 1];
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
        string buttonName = GetButtonName();

        int buttonNumber = int.Parse(buttonName.Substring(4, 1));

        Debug.Log($"룬 이름: {runeName[buttonNumber - 1].text}");
        Debug.Log($"룬 설명: {runeDescription[buttonNumber - 1].text}");

        UIWhiteBox.SetActiveRunePanel(false);
    }


    #endregion
}
