using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Cinemachine;


public static class PlayerStatWhiteBox
{
    public static PlayerManager WhiteBox { get; private set; }

    public static void SetWhiteBox(PlayerManager manager)
    {
        WhiteBox = manager;
    }
}
public class PlayerManager : MonoBehaviour
{
    PlayerStatus playerStatus;
    PlayerAttack playerAttack;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    public int playerAttackDamage(AttackType type) => playerStatus.PlayerDamagebyAttackType(type);
    public float playerHammerTimer() => playerAttack.hammerThrowCooldownTimer;
    public CinemachineFreeLook FreeLookCamera => freeLookCamera;

    private void Awake()
    {
        PlayerStatWhiteBox.SetWhiteBox(this);
        playerStatus = GetComponent<PlayerStatus>();
        playerAttack =GetComponent<PlayerAttack>();
    }
}
