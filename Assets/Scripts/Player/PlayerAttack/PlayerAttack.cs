using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerStatus status;
    private PlayerAnimStateChecker animChecker;

    [Header("Attack Combo")]
    [SerializeField] private int comboStep = 0;
    [SerializeField] private float comboResetTime = 1.0f;
    private float comboTimer;
    private bool canAttack = true;
    public bool IsAttacking { get; private set; } = false;

    [Header("Windmill")]
    private bool isWindmilling = false;
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

    public AttackType attackType = AttackType.None;

    private IItemUseable useable;
    private IAttackable attackable;

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
        animChecker = GetComponent<PlayerAnimStateChecker>();
    }

    private void Update()
    {
        HandleComboResetTimer();

        if (animChecker.IsWhirlwindAnim())
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
        if (status.IsDead || !context.performed) return;
        if (animChecker.IsBlockAnim()) return;

        if (!canAttack)
        {
            if (!animChecker.IsWhirlwindAnim())
            {
                bufferedLightAttack = true;
                return;
            }
        }

        if (animChecker.IsWhirlwindAnim())
        {
            IsAttacking = true;
            canAttack = false;
            animator.SetTrigger("SAttack_1");
            Debug.Log("강공격 전환");
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
            animator.SetTrigger("Attack_1");
        else if (comboStep == 2)
            animator.SetTrigger("Attack_2");

        comboTimer = comboResetTime;
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (status.IsDead) return;
        if (animChecker.IsBlockAnim()) return;

        // 아이템 장착 중일 때: 우클릭은 조준 동작만 처리
        if (equipItem)
        {
            if (context.started && !animChecker.IsAttackAnim() && !animChecker.IsDiveAnim())
            {
                isAiming = true;
                useable.UseItemByType(weaponType);
            }
            else if (context.canceled && !animChecker.IsDiveAnim())
            {
                isAiming = false;
                useable.EndUseItemByType(weaponType);
            }

            return; // 여기서 끝
        }

        // 일반 무기 상태 (망치 등): 강공격 처리
        if (context.started)
        {
            if (!canAttack)
            {
                bufferedHeavyAttack = true;
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
            StopWindMill();
            animator.SetBool("isSpinning", false);
        }
    }


    public void OnEquipItem(InputAction.CallbackContext context)
    {
        if (status.IsDead) return;
        if (!context.performed || isAiming) return;

        int currentItemType = UIWhiteBox.GetCurrentItemNum();

        if (!equipItem)
        {
            equipItem = true;
            weaponType = (WeaponType)currentItemType;
            UIWhiteBox.SetActiveItemSelectImage(true);
        }
        else
        {
            equipItem = false;
            weaponType = WeaponType.Hammer;
            UIWhiteBox.SetActiveItemSelectImage(false);
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

    public void SetAttackTypeToLight() => attackType = AttackType.Light;
    public void SetAttackTypeToHeavy() => attackType = AttackType.Heavy;

    public void StartWindmillTimer() => isWindmilling = true;
    public void DizzyPlay() => isDizzy = true;

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

        UIWhiteBox.UseGauge(0.7f);
        currentWindMillStemina = UIWhiteBox.GetGauge();

        if (currentWindMillStemina <= 0f)
            StopWindMill();
    }

    private void StartWindMill()
    {
        if (isWindmilling) return;

        canAttack = false;
        animator.SetBool("isSpinning", true);
    }

    private void StopWindMill()
    {
        if (!isWindmilling) return;

        isWindmilling = false;
        animator.SetBool("isSpinning", false);
        dizzyTimer = 0f;
    }

    [ContextMenu("화면이동 잠금")]
    public void TestMethod() => DisableInputAction_Attack1();
}
