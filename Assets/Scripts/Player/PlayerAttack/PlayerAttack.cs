using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.InputSystem.Interactions;
using System;
using Game;

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
    //private bool windmillInputHeld = false;

    [SerializeField] private GameObject hammer;
    private Collider[] hammerCollider;

    [SerializeField] private InputActionReference attack1Action;

    public AttackType attackType = AttackType.None;
    private AttackType testCurrentAttackType = AttackType.Light;

    public bool equipItem = false;
    public WeaponType weaponType = WeaponType.Hammer;
    private bool isAiming = false;

    private IItemUseable useable;
    private IAttackable attackable;

    [SerializeField] private HammerThrowController hammerController;
    private GameObject activeHammer;

    [ContextMenu("약공격으로 설정")]
    public void SetLightAttackType()
    {
        testCurrentAttackType = AttackType.Light;
    }

    [ContextMenu("강공격으로 설정")]
    public void SetHeavyAttackType()
    {
        testCurrentAttackType = AttackType.Heavy;
    }

    public int currentAttackType()
    {
        return (int)attackType;
    }

    [ContextMenu("화면이동 잠금")]
    public void TestMethod()
    {
        DisableInputAction_Attack1();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hammerCollider = hammer.GetComponents<Collider>();
        useable = GetComponent<IItemUseable>();
        attackable = GetComponent<IAttackable>();
    }

    public void OnAttack1(InputAction.CallbackContext context)
    {
       
        if (!context.performed) return;

        if (!canAttack) return;

        if (equipItem && isAiming)
        {
            attackable.AttackByType(weaponType);
            return;
        }
        //activeHammer = hammerController.ActiveHammer;
        //var stuckHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();

        //if (stuckHandler.IsStuckToWall) return;

        comboStep++;
        animator.applyRootMotion = true;
        IsAttacking = true; // 공격 시작 시 true 설정
        attackType = testCurrentAttackType ;

        if (comboStep == 1)
        {
            Debug.Log("공격!!_1");
            animator.SetTrigger("Attack_1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack_2");
        }

        comboTimer = comboResetTime;
    
    
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (equipItem)
            {
                Debug.Log("조준 시작");
                isAiming = true;
                useable.UseItemByType(weaponType);
            }
            else
            {
                StartWindMill();
            }
        }

        if (context.canceled)
        {
            if (equipItem)
            {
                Debug.Log("조준 해제");
                isAiming = false;
                useable.EndUseItemByType(weaponType);
            }
            else
            {
                StopWindMill();
            }
        }
    }

    public void OnEquipItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int currentItemType = UIWhiteBox.GetCurrentItemNum();
            if (!equipItem)
            {
                Debug.Log("아이템 장착");
                equipItem = true;
                weaponType = (WeaponType)currentItemType;
            }
            else
            {
                Debug.Log("아이템 장착 해제");
                equipItem = false;
                weaponType = WeaponType.Hammer;
            }
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

        attackType = AttackType.Heavy;

        //animator.SetBool("IsWindmilling",isWindmilling);
        Debug.Log("윈드밀 시작");
    }

    private void StopWindMill()
    {
        if (!isWindmilling) return;

        isWindmilling= false;

        attackType= AttackType.None;
        //animator.SetBool("IsWindmilling",isWindmilling);
        Debug.Log("윈드밀 끝");
    }

    #region Attack Anim Event Key
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

    #endregion

    public void EnableInputAction_Attack1()
    {
        attack1Action.action.Enable();
    }

    public void DisableInputAction_Attack1()
    {
        attack1Action.action.Disable();
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
