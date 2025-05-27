using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTip : MonoBehaviour
{
    [Space(2)]
    [Header("TipText")]
    [Tooltip("Title")]
    [SerializeField] private Text TipTitleText;
    [Tooltip("TipContent")]
    [SerializeField] private Text TipContent;

    [Space(2)]
    [Header("Data")]
    public ItemDataReader LoadingTipData;
    private List<ItemData> LoadingTipList = new List<ItemData>();

    private void OnEnable()
    {
        SetTipText();
    }

    private void SetTipText()
    {
        if(LoadingTipData == null)
        {
            Debug.LogError("TipData가 없습니다.");
            return;
        }

        LoadingTipList = LoadingTipData.DataList;

        if(LoadingTipList == null || LoadingTipList.Count == 0)
        {
            Debug.LogError("Tip 데이터 리스트가 비어있습니다.");
            return;
        }

        int randomIndex = Random.Range(0, LoadingTipList.Count);
        ItemData randomTip = LoadingTipList[randomIndex];

        TipTitleText.text = randomTip.name;
        TipContent.text = randomTip.description;
    }
}
