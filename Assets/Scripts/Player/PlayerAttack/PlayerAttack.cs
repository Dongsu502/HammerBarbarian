using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.InputSystem.Interactions;
using System;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    [Header("AttackCombo")]
    [SerializeField] private int comboStep = 0;
    [SerializeField] private float comboResetTime = 1.0f;
    private float comboTimer;
    private bool canAttack = true;


    public bool IsAttacking { get; private set; } = false;

    [SerializeField] private float maxWindmillTime = 3f;
    private float windmillTimer = 0f;
    private bool isWindmilling = false;
    private bool windmillInputHeld = false;

    [SerializeField] private GameObject hammer;
    private Collider[] hammerCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hammerCollider = hammer.GetComponents<Collider>();
    }

    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!canAttack) return;

        comboStep++;
        animator.applyRootMotion = true;
        IsAttacking = true; // 공격 시작 시 true 설정
        Debug.Log(IsAttacking);

        if (comboStep == 1)
        {
            Debug.Log("공격!!_1");
            animator.SetTrigger("Attack_1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack_2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack_3");
        }

        comboTimer = comboResetTime;
    
    
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartWindMill();
        }

        if (context.canceled)
        {
            StopWindMill();
        }
    }

    private void Update()
    {
        if (comboStep > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                comboStep = 0;
                comboTimer = 0f;
            }
        }

        if (isWindmilling)
        {
            windmillTimer += Time.deltaTime;
            UIWhiteBox.UseGauge(0.15f);
            if(windmillTimer >= maxWindmillTime)
            {
                StopWindMill();
            }
        }
    }

    private void StartWindMill()
    {
        if (isWindmilling) return;

        windmillTimer = 0f;
        isWindmilling = true;

        //animator.SetBool("IsWindmilling",isWindmilling);
        Debug.Log("윈드밀 시작");
    }

    private void StopWindMill()
    {
        if (!isWindmilling) return;

        isWindmilling= false;
        //animator.SetBool("IsWindmilling",isWindmilling);
        Debug.Log("윈드밀 끝");
    }


    public void ComboReset()
    {
        comboStep = 0;
        comboTimer = 0;
        animator.applyRootMotion = false;
    }

    public void DIsableAttackInput()
    {
        canAttack = false;
    }

    public void EndAttack()
    {
        IsAttacking = false;
        canAttack = true;
    }

    public void EnableHammerCollider()
    {
        for (int i = 0; i < hammerCollider.Length; i++)
        {
            hammerCollider[i].enabled = true;
        }
    }

    public void DisableHammerCollider()
    {
        for (int i = 0; i < hammerCollider.Length; i++)
        {
            hammerCollider[i].enabled = false;
        }
       
    }

    private void OnAnimatorMove()
    {
        if (animator && animator.applyRootMotion)
        {
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0f;
            transform.position += deltaPosition;
        }
    }

    public bool IsAttackAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Attack");
    }
}
