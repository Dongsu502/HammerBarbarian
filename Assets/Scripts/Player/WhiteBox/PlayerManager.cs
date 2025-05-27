using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;


public static class PlayerStatWhiteBox
{
    public static PlayerManager WhtieBox { get; private set; }

    public static void SetWhiteBox(PlayerManager manager)
    {
        WhtieBox = manager;
    }
}
public class PlayerManager : MonoBehaviour
{
    PlayerStatus playerStatus;

    public int playerAttackDamage(AttackType type) => playerStatus.PlayerDamagebyAttackType(type);

    private void Awake()
    {
        PlayerStatWhiteBox.SetWhiteBox(this);
        playerStatus = GetComponent<PlayerStatus>();
    }
}
