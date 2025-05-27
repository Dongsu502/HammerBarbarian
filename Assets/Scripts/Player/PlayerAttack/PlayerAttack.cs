using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Attack Combo")]
    [SerializeField] private int comboStep = 0;
    [SerializeField] private float comboResetTime = 1.0f;
    private float comboTimer;
    private bool canAttack = true;
    public bool IsAttacking { get; private set; } = false;

    [Header("Windmill")]
    [SerializeField] private float maxWindmillTime = 3f;
    private float windmillTimer = 0f;
    private bool isWindmilling = false;

    [Header("Weapon")]
    [SerializeField] private GameObject hammer;
    private Collider[] hammerCollider;
    [SerializeField] private HammerThrowController hammerController;
    private GameObject activeHammer;

    [Header("Item & Input")]
    [SerializeField] private InputActionReference attack1Action;
    public WeaponType weaponType = WeaponType.Hammer;
    public bool equipItem = false;
    private bool isAiming = false;

    [Header("Dizzy")]
    private bool isDizzy = false;
    [SerializeField] private float dizzyMoveDuration = 0.5f;
    [SerializeField] private float dizzyMoveSpeed = 1.5f;
    private float dizzyTimer = 0f;

    // Attack types
    public AttackType attackType = AttackType.None;
    private AttackType testCurrentAttackType = AttackType.Light;

    // Interfaces
    private IItemUseable useable;
    private IAttackable attackable;

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hammerCollider = hammer.GetComponents<Collider>();
        useable = GetComponent<IItemUseable>();
        attackable = GetComponent<IAttackable>();
    }

    private void Update()
    {
        HandleComboResetTimer();
        HandleWindmillState();
    }

    private void FixedUpdate()
    {
        if (isDizzy)
        {
            dizzyTimer += Time.fixedDeltaTime;
            Vector3 forward = transform.forward;
            rb.MovePosition(rb.position + forward * dizzyMoveSpeed * Time.fixedDeltaTime);

            if (dizzyTimer >= dizzyMoveDuration)
                isDizzy = false;
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

    #endregion

    #region Input Methods

    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (!context.performed || !canAttack) return;

        if (equipItem && isAiming)
        {
            attackable.AttackByType(weaponType);
            return;
        }

        activeHammer = hammerController.ActiveHammer;
        if (activeHammer != null)
        {
            var stuckHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();
            if (stuckHandler != null && stuckHandler.IsStuckToWall)
            {
                Debug.Log("안할게");
                return;
            }
        }

        comboStep++;
        IsAttacking = true;
        attackType = testCurrentAttackType;

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
                return;
            }

            if (comboStep >= 2)
            {
                StartWindMill();
            }
            else
            {
                IsAttacking = true;
                attackType = AttackType.Heavy;
                animator.SetTrigger("SAttack_1");
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
                animator.SetBool("isSpinning", false);
            }
        }
    }

    public void OnEquipItem(InputAction.CallbackContext context)
    {
        if (context.performed && !isAiming)
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

    #endregion

    #region Attack Logic

    private void HandleComboResetTimer()
    {
        if (comboStep <= 0) return;

        comboTimer -= Time.deltaTime;
        if (comboTimer <= 0f)
        {
            comboStep = 0;
            comboTimer = 0f;
        }
    }

    private void StartWindMill()
    {
        if (isWindmilling) return;

        windmillTimer = 0f;
        isWindmilling = true;
        attackType = AttackType.Heavy;

        animator.SetBool("isSpinning", true);
        Debug.Log("윈드밀 시작");
    }

    private void StopWindMill()
    {
        if (!isWindmilling) return;

        isWindmilling = false;
        attackType = AttackType.None;
        animator.SetBool("isSpinning", false);

        isDizzy = true;
        dizzyTimer = 0f;

        Debug.Log("윈드밀 끝");
    }

    private void HandleWindmillState()
    {
        if (!isWindmilling) return;

        windmillTimer += Time.deltaTime;
        UIWhiteBox.UseGauge(0.15f);

        if (windmillTimer >= maxWindmillTime)
            StopWindMill();
    }

    #endregion

    #region Animation Events

    public void ComboReset()
    {
        comboStep = 0;
        comboTimer = 0f;
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
        foreach (var col in hammerCollider)
            col.enabled = true;
    }

    public void DisableHammerCollider()
    {
        foreach (var col in hammerCollider)
            col.enabled = false;
    }

    #endregion

    #region Utility

    public void EnableInputAction_Attack1()
    {
        attack1Action.action.Enable();
    }

    public void DisableInputAction_Attack1()
    {
        attack1Action.action.Disable();
    }

    public int currentAttackType()
    {
        return (int)attackType;
    }

    public void SetLightAttackType() => testCurrentAttackType = AttackType.Light;
    public void SetHeavyAttackType() => testCurrentAttackType = AttackType.Heavy;

    [ContextMenu("화면이동 잠금")]
    public void TestMethod() => DisableInputAction_Attack1();

    public bool IsAttackAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Attack");
    }

    #endregion
}
