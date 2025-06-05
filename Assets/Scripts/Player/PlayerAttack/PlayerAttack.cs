using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerStatus status;

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
    private float maxWindMillStemina = 100f;
    private float minWindMillStemina = 0f;
    private float currentWindMillStemina = 100f;

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

    // Interfaces
    private IItemUseable useable;
    private IAttackable attackable;

    // Buffered Input
    private bool bufferedLightAttack = false;
    private bool bufferedHeavyAttack = false;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
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

    public void OnAttack1(InputAction.CallbackContext context)
    {
        if (status.IsDead) return;
        if (!context.performed) return;

        if (!canAttack)
        {
            bufferedLightAttack = true;
            return;
        }

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
                return;
            }
        }

        comboStep++;
        IsAttacking = true;

        if (comboStep == 1)
        {
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
        if (status.IsDead) return;

        if (context.started)
        {
            if (!canAttack)
            {
                bufferedHeavyAttack = true;
                return;
            }

            if (equipItem&& !IsAttackAnim()&&!IsDiveAnim())
            {
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
                canAttack = false;
                animator.SetTrigger("SAttack_1");

            }
        }

        if (context.canceled)
        {
            if (equipItem&& !IsDiveAnim())
            {
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
        if (status.IsDead) return;

        if (context.performed && !isAiming)
        {
            int currentItemType = UIWhiteBox.GetCurrentItemNum();

            if (!equipItem)
            {
                equipItem = true;
                weaponType = (WeaponType)currentItemType;

                //아이템 장착 표시
                UIWhiteBox.SetActiveItemSelectImage(true);
            }
            else
            {
                equipItem = false;
                weaponType = WeaponType.Hammer;

                //아이템 장착 표시 해제
                UIWhiteBox.SetActiveItemSelectImage(false);
            }
        }
    }

    public void EndAttack()
    {
        IsAttacking = false;
        canAttack = true;

        if (bufferedHeavyAttack)
        {
            bufferedHeavyAttack = false;
            OnAttack2(new InputAction.CallbackContext());
        }
        else if (bufferedLightAttack)
        {
            bufferedLightAttack = false;
            OnAttack1(new InputAction.CallbackContext());
        }
    }

    public void ComboReset()
    {
        comboStep = 0;
        comboTimer = 0f;
        animator.SetBool("isSpinning", false);
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

    public void SetAttackTypeToLight()
    {
        attackType = AttackType.Light;
    }

    public void SetAttackTypeToHeavy()
    {
        attackType = AttackType.Heavy;
    }

    public void StartWindmillTimer()
    {
        isWindmilling = true;
    }

    public void EnableInputAction_Attack1() => attack1Action.action.Enable();
    public void DisableInputAction_Attack1() => attack1Action.action.Disable();
    public int currentAttackType() => (int)attackType;

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

    private void HandleWindmillState()
    {
        if (!isWindmilling)
        {
            currentWindMillStemina = UIWhiteBox.GetGauge();
            return;
        }
        UIWhiteBox.UseGauge(0.3f);
        currentWindMillStemina = UIWhiteBox.GetGauge();

        if (currentWindMillStemina <= 0f)
        {
            StopWindMill();
        }
    }

    private void StartWindMill()
    {
        if (isWindmilling) return;

        canAttack = false;
        windmillTimer = 0f;
        animator.SetBool("isSpinning", true);
    }

    private void StopWindMill()
    {
        if (!isWindmilling) return;

        isWindmilling = false;
        animator.SetBool("isSpinning", false);

        isDizzy = true;
        dizzyTimer = 0f;
    }

    public bool IsAttackAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Attack");
    }

    public bool IsDiveAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Dive");
    }

    public bool IsWhirlwindAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Whirlwind");
    }

    [ContextMenu("화면이동 잠금")]
    public void TestMethod() => DisableInputAction_Attack1();
}
