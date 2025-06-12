using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerStatus : MonoBehaviour
{
    private int maxPlayerHp = 6;
    public int playerHP = 6;
    public int playerAttackDamage = 10;
    public InputAction inputAction;

    public bool IsDead { get; private set; } = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        playerHP = maxPlayerHp;
    }

    private void Update()
    {
        Debug.LogWarning(IsDead);
    }

    public void TakeDamage(int damage)
    {
        
        playerHP -= damage;
        UIWhiteBox.TakeDamage(damage); 
    }

    public void Die()
    {
        if(IsDead) return;

        IsDead = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true; // 물리 정지

        // Input 차단
        GetComponent<PlayerInput>().DeactivateInput();
    }

    public void OnAnimEnd()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public int PlayerDamagebyAttackType(AttackType attackType)
    {
        if (attackType == AttackType.None)
            return 0;
        if (attackType == AttackType.Light)
            return 10;
        if (attackType == AttackType.Heavy)
            return 20;
        if(attackType==AttackType.Skill)
            return 30;
        if (attackType == AttackType.WhirlWind)
            return 5;

        return 0;
    }

    public void OffInputAction()
    {
        inputAction.Disable();
    }

    public void OnInputAction()
    {
        inputAction.Enable();
    }
}
