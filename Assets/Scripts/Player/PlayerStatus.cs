using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float maxPlayerHp = 200f;
    public float playerHP = 200f;
    public int playerAttackDamage = 10;

    private void Start()
    {
        playerHP = maxPlayerHp;
    }
}
