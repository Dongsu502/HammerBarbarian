using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerWhiteBox
{
    public static PlayerInputManager WhiteBox { get; private set; }

    public static void SetWhiteBox(PlayerInputManager manager)
    {
        WhiteBox = manager;
    }
}
public class PlayerInputManager : MonoBehaviour
{
    PlayerAttack playerAttack;
    PlayerMove playerMove;

    public void EnableAttack1Action() => playerAttack.EnableInputAction_Attack1();

    public void DisableAttackAction() => playerAttack.DisableInputAction_Attack1();

    public void EnableLookAction() => playerMove.EnableActionLook();
    
    public void DisableLookAction()=> playerMove.DisableActionLook();

    public int currentAttackType => playerAttack.currentAttackType();

    void Start()
    {
        PlayerWhiteBox.SetWhiteBox(this);
        playerAttack = GetComponent<PlayerAttack>();
        playerMove = GetComponent<PlayerMove>();
    }

}
