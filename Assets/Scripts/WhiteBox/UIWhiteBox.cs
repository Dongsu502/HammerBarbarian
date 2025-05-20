using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class UIWhiteBox
{
    public static TitleUIManager TitleUIWB { get; private set; }
    public static MainUIManager MainUIWB { get; private set; }

    public static MinimapFog MinimapFogWB { get; private set; }

    public static RuneInventoryUI RuneInventoryWB { get; private set; }
    public static RuneChoiceUI RuneChoiceWB { get; private set; }

    public static void SetTitleUIWB(TitleUIManager titleUIM)
    {
        TitleUIWB = titleUIM;
    }
    public static void SetMainUIWB(MainUIManager mainUIWB)
    {
        MainUIWB = mainUIWB;
    }
    public static void SetMinimapFogWB(MinimapFog minimapFogWB)
    {
        MinimapFogWB = minimapFogWB;
    }
    public static void SetRuneInventoryWB(RuneInventoryUI runeInventoryWB)
    {
        RuneInventoryWB = runeInventoryWB;
    }
    public static void SetRuneChoiceWB(RuneChoiceUI runeChoiceWB)
    {
        RuneChoiceWB = runeChoiceWB;
    }

    #region TitleUI

    public static void GobackMenu()
    {
        TitleUIWB.GobackMenu();
    }

    #endregion

    #region MainUI

    public static void SetActivePausePanel(bool isActive)
    {
        MainUIWB.PausePanel_SetActive(isActive);
    }

    public static void SetActiveRunePanel(bool isActive)
    {
        MainUIWB.RunePanel_SetActive(isActive);
    }

    public static void SetActiveRuneInventoryPanel(bool isActive)
    {
        MainUIWB.RuneInventoryPanel_SetActive(isActive);
    }

    /// <summary>
    /// 피격 시 하트 감소
    /// </summary>
    /// <param name="damage">감소할 하트 갯수</param>
    public static void TakeDamage(int damage)
    {
        MainUIWB.TakeDamage(damage);
    }

    /// <summary>
    /// 하트 회복
    /// </summary>
    /// <param name="heal">회복할 하트 갯수</param>
    public static void Healing(int heal)
    {
        MainUIWB.Heal(heal);
    }

    /// <summary>
    /// 현재 아이템 번호
    /// </summary>
    /// <returns>MainUIManager의 currentItemNum</returns>
    public static int GetCurrentItemNum()
    {
        return MainUIWB.UseItemNumber() + 1;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 아이템 리스트 재설정(테스트용)
    /// </summary>
    public static void SetItemList()
    {
        MainUIWB.SetItemList();
    }
#endif

    /// <summary>
    /// 아이템 획득
    /// </summary>
    public static void GetItem()
    {
        MainUIWB.GetItem();
    }

    /// <summary>
    /// 게이지 감소 / 게이지 최소:0 최대:100
    /// </summary>
    /// <param name="amount">감소할 게이지 값</param>
    public static void UseGauge(float amount)
    {
        MainUIWB.UseGauge(amount);
    }

    /// <summary>
    /// 원거리공격 에임 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화여부</param>
    public static void SetActiveCrosshair(bool isActive)
    {
        MainUIWB.Crosshair_SetActive(isActive);
    }

    /// <summary>
    /// 룬 표시 이미지 색깔 초기화
    /// </summary>
    public static void ResetColor_RuneShowImage()
    {
        MainUIWB.ResetColorRuneShowImage();
    }

    /// <summary>
    /// 룬 표시 이미지 색깔 변경
    /// </summary>
    /// <param name="index">변경할 룬 표시 이미지 번호(인벤토리에 들어온 룬 번호)</param>
    public static void SetColor_RuneShowImage(int index)
    {
        MainUIWB.SetColorRuneShowImage(index);
    }

    #endregion

    #region MinimapFog

    public static void DisableMinimapFog(Collider triggerFog)
    {
        MinimapFogWB.DisableFog(triggerFog);
    }

    #endregion

    #region Rune

    /// <summary>
    /// 룬 인벤토리 타이틀 텍스트 설정
    /// </summary>
    /// <param name="newText">변경할 텍스트값</param>
    public static void SetRuneInventoryTitleText(string newText)
    {
        RuneInventoryWB.ChangeTitleText(newText);
    }

    /// <summary>
    /// 룬 인벤토리에 적용
    /// </summary>
    /// <param name="rune">추가할 룬</param>
    public static void AddRuneToInventory(ItemData rune)
    {
        RuneInventoryWB.AddRuneToInventory(rune);
    }

    public static List<int> GetRuneIDs()
    {
        return RuneInventoryWB.runeIDs;
    }

    #endregion
}
