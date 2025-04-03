using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float comboTimer;

    [SerializeField] private float comboResetTime = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        comboStep++;


        if (comboStep ==1)
        {
            Debug.Log("공격!!_1");
            animator.SetTrigger("Attack_1");
        }
        else if(comboStep ==2)
        {
            animator.SetTrigger("Attack_2");
        }
        else if(comboStep==3)
        {
            animator.SetTrigger("Attack_3");
        }

        comboTimer = comboResetTime;
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("공격!!_2");
            animator.SetTrigger("Attack_2");
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
    }

    public void ComboReset()
    {
        comboStep = 0;
        comboTimer = 0;
        animator.applyRootMotion = false;
    }
}
