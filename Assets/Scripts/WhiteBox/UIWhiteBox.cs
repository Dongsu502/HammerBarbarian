using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MainUIState
{
    NONE,
    RUNE_CHOICE,
    RUNE_GET,
    PAUSE,
    PAUSE_SETTING,
    RUNE_INVENTORY,
    SCRIPT
}
public enum ChoiceMapUIState
{
    DUNGEON,
    WORLD
}

public static class UIWhiteBox
{
    public static MainUIState MainUICurrentState { get; set; }
    public static ChoiceMapUIState ChoiceMapUICurrentState { get; set; }

    public static TitleUIManager TitleUIWB { get; private set; }
    public static MainUIManager MainUIWB { get; private set; }
    public static PauseUIManager PauseUIWB { get; private set; }
    public static MinimapFog MinimapFogWB { get; private set; }
    public static RuneInventoryUI RuneInventoryWB { get; private set; }
    public static RuneChoiceUI RuneChoiceWB { get; private set; }
    public static ScriptUIManager ScriptUIWB { get; private set; }

    #region SetWhiteBox

    public static void SetTitleUIWB(TitleUIManager titleUIM)
    {
        TitleUIWB = titleUIM;
    }
    public static void SetMainUIWB(MainUIManager mainUIWB)
    {
        MainUIWB = mainUIWB;
    }
    public static void SetPauseUIWB(PauseUIManager pauseUIWB)
    {
        PauseUIWB = pauseUIWB;
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
    public static void SetScriptUIWB(ScriptUIManager scriptUIWB)
    {
        ScriptUIWB = scriptUIWB;
    }

    #endregion

    #region TitleUI

    public static void GobackMenu()
    {
        TitleUIWB.GobackMenu();
    }

    public static bool GetisNewGame()
    {
        return TitleUIWB.isNewGame;
    }

    #endregion

    #region MainUI

    #region SetActivePanel
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

    public static void SetActiveScriptUIPanel(bool isActive)
    {
        MainUIWB.ScriptPanel_SetActive(isActive);
    }

    #endregion

    /// <summary>
    /// 마우스 커서 잠금 & 표시
    /// </summary>
    /// <param name="isLock">잠금 여부 true: 잠금해제 & 표시 / false: 잠금 & 표시되지않게</param>
    public static void CursorLock(bool isLock)
    {
        MainUIWB.CursorLock(isLock);
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

    public static void StartScripting(int _start, int _end)
    {
        MainUICurrentState = MainUIState.SCRIPT;

        //대사 UI 활성화
        MainUIWB.ScriptPanel_SetActive(true);
        
        //대사 가져오기
        ScriptUIWB.GetScriptData(_start, _end);
    }

    #endregion

    #region PauseUI

    /// <summary>
    /// 설정창 활성화 / 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public static void SetActive_SettingPanel(bool isActive)
    {
        PauseUIWB.SetActive_SettingPanel(isActive);
    }

    #endregion

    #region MinimapFog

    /// <summary>
    /// 미니맵 안개 제거
    /// </summary>
    /// <param name="triggerFog">비활성화 할 이미지 콜라이더</param>
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

    /// <summary>
    /// 현재 소유중인 룬 목록 가져오기
    /// </summary>
    /// <returns>인벤토리에 있는 룬 목록</returns>
    public static List<int> GetRuneIDs()
    {
        return RuneInventoryWB.runeIDs;
    }

    #endregion
}
