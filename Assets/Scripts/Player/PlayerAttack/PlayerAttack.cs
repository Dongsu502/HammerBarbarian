using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerStatus status;
    private PlayerAnimStateChecker animChecker;

    [Header("Light Attack Combo")]
    [SerializeField] private float lightComboResetTime = 2.0f;
    [SerializeField] private int lightComboStep = 0;
    private float lightComboTimer;

    [Header("Heavy Attack Combo")]
    [SerializeField] private float heavyComboResetTime = 2.0f;
    private int heavyComboStep = 0;
    private float heavyComboTimer;

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

    [SerializeField]private bool bufferedLightAttack = false;
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
        HandleLightComboReset();
        HandleHeavyComboReset();

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
                return;
        }

        lightComboStep++;
        IsAttacking = true;
        //canAttack = false;

        if (lightComboStep == 1)
            animator.SetTrigger("Attack_1");
        else if (lightComboStep == 2)
            animator.SetTrigger("Attack_2");

        lightComboTimer = lightComboResetTime;
        attackType = AttackType.Light;
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (status.IsDead || animChecker.IsBlockAnim()) return;

        // 아이템 장착 상태에서는 조준 기능만 수행
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
            return;
        }

        if (context.started)
        {
            if (!canAttack)
            {
                bufferedHeavyAttack = true;
                return;
            }

            // light 콤보가 2타까지 진행된 경우 → 윈드밀 발동
            if (lightComboStep >= 2)
            {
                StartWindMill();
                return;
            }

            // 일반 강공격 콤보 흐름
            heavyComboStep++;
            IsAttacking = true;
            //canAttack = false;

            if (heavyComboStep == 1)
                animator.SetTrigger("SAttack_1");
            else if (heavyComboStep == 2)
                animator.SetTrigger("SAttack_2");
            // 필요 시 SAttack_3 등 추가 가능

            heavyComboTimer = heavyComboResetTime;
            attackType = AttackType.Heavy;
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

        int currentItemType = DataManager.Instance.GetCurrentData().currentItemList;
        if (currentItemType == 0) return;

        if (!equipItem)
        {
            equipItem = true;
            weaponType = (WeaponType)currentItemType;
            UIWhiteBox.SetActiveItemSelectImage(true);
        }
        else
        {
            equipItem = false;
            weaponType = (WeaponType)currentItemType;
            UIWhiteBox.SetActiveItemSelectImage(false);
        }
    }

    public void EndAttack()
    {
        IsAttacking = false;
        canAttack = true;
        
        if (bufferedHeavyAttack)
        {
            Debug.Log($"Attack1 called: lightComboStep={lightComboStep}, canAttack={canAttack}, isAttacking={IsAttacking}");
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
        lightComboStep = 0;
        heavyComboStep = 0;
        lightComboTimer = 0f;
        heavyComboTimer = 0f;
        animator.ResetTrigger("Attack_2");
        animator.ResetTrigger("SAttack_2");
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

    private void HandleLightComboReset()
    {
        
        if (lightComboStep <= 0) return;

        lightComboTimer -= Time.deltaTime;
        if (lightComboTimer <= 0f)
            lightComboStep = 0;
    }

    private void HandleHeavyComboReset()
    {
        if (heavyComboStep <= 0) return;

        heavyComboTimer -= Time.deltaTime;
        if (heavyComboTimer <= 0f)
            heavyComboStep = 0;
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
