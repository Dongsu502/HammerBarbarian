using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Rendering;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private int comboStep = 0;
    private float comboTimer;
    [SerializeField] private float comboResetTime = 1.0f;

    public bool IsAttacking { get; private set; } = false;

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

    public void EndAttack()
    {
        IsAttacking = false; 
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
}
