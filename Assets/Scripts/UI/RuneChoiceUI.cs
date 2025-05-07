using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    private void OnEnable()
    {
        GetRandomID();
    }

    private void GetRandomID()
    {
        // 전체 리스트에서 랜덤하게 3개 선택 (중복 없음)
        List<ItemData> randomItems = runeData.DataList.OrderBy(x => Random.value).Take(3).ToList();

        for (int i = 0; i < runeData.DataList.Count; i++)
        {
            runeName[i].text = randomItems[i].name.ToString();
            runeDescription[i].text = randomItems[i].description.ToString();
        }
    }
}
