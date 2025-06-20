using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGauge : MonoBehaviour
{
    [SerializeField] private Image itemGaugeImage;
    private float cooldownMax = 5f;
    private bool isUse;

    public void UseItem()
    {
        itemGaugeImage.fillAmount = 0;
        isUse = true;
    }

    private void Awake()
    {
        UIWhiteBox.SetItemGaugeUIWB(this);
    }

    private void Update()
    {
        if (!isUse) return;

        //게이지 0부터 1까지 채우기
        itemGaugeImage.fillAmount = 1f - (PlayerStatWhiteBox.WhiteBox.playerHammerTimer() / cooldownMax);

        if (itemGaugeImage.fillAmount >= 1) isUse = false;
        
    }
}
