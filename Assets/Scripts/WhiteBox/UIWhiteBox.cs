using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIWhiteBox
{
    public static TitleUIManager TitleUIWB { get; private set; }
    public static MainUIManager MainUIWB { get; private set; }

    public static void SetTitleUIWB(TitleUIManager titleUIM)
    {
        TitleUIWB = titleUIM;
    }
    public static void SetMainUIWB(MainUIManager mainUIWB)
    {
        MainUIWB = mainUIWB;
    }

    #region MainUI

    /// <summary>
    /// 피격 시 하트 감소
    /// </summary>
    /// <param name="damage">감소할 하트 갯수</param>
    public static void TakeDamage(int  damage)
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
        return MainUIWB.UseItemNumber();
    }

    /// <summary>
    /// 게이지 감소 / 게이지 최소:0 최대:100
    /// </summary>
    /// <param name="amount">감소할 게이지 값</param>
    public static void UseGauge(float amount)
    {
        MainUIWB.UseGauge(amount);
    }

    #endregion
}
