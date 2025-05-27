using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int maxPlayerHp = 6;
    public int playerHP = 6;
    public int playerAttackDamage = 10;

    private void Start()
    {
        playerHP = maxPlayerHp;
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        UIWhiteBox.TakeDamage(damage);
    }
}
