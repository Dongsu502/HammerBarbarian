using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public static class PlayerHitWhiteBox
{
    public static PlayerHitManager WhiteBox { get; private set; }

    public static void SetWhiteBox(PlayerHitManager manager)
    {
        WhiteBox = manager;
    }
}
public class PlayerHitManager : MonoBehaviour
{
    [SerializeField]CameraShakeManager cameraShakeManager;
    [SerializeField]PlayerAttack PlayerAttack;
    public void Shake(string type, AttackType attackType) => cameraShakeManager.Shake(type, attackType);
    public AttackType attacktype => PlayerAttack.attackType;

    private void Awake()
    {
        PlayerHitWhiteBox.SetWhiteBox(this);
    }


}
