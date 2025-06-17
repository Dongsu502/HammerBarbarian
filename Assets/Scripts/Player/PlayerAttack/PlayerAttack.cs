using UnityEngine;
using UnityEngine.InputSystem;
using Game;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerStatus status;
    private PlayerAnimStateChecker animChecker;
    private PlayerAttackTrigger attackTrigger;

    [Header("Light Attack Combo")]
    [SerializeField] private float lightComboResetTime = 2.0f;
    [SerializeField] private int lightComboStep = 0;
    private float lightComboTimer;

    [Header("Heavy Attack Combo")]
    [SerializeField] private float heavyComboResetTime = 2.0f;
    private int heavyComboStep = 0;
    private float heavyComboTimer;

    [Header("Spin Combo")]
    [SerializeField] private float spinComboTimeout = 1.5f;
    private float spinComboTimer = 0f;
    private bool isSpinCombo = false;
    private int spinComboStep = 0;

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

    [SerializeField] private bool bufferedLightAttack = false;
    private bool bufferedHeavyAttack = false;

    [SerializeField] private GameObject idleSnake;
    [SerializeField] private GameObject attackSnake;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hammerCollider = hammer.GetComponents<Collider>();
        useable = GetComponent<IItemUseable>();
        attackable = GetComponent<IAttackable>();
        animChecker = GetComponent<PlayerAnimStateChecker>();
        attackTrigger = GetComponentInChildren<PlayerAttackTrigger>();
    }

    private void Update()
    {
        HandleLightComboReset();
        HandleHeavyComboReset();
        HandleSpinComboTimeout();

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

            if (lightComboStep == 1 && !isSpinCombo)
            {
                StartSpinCombo();
                return;
            }

            if (isSpinCombo && spinComboStep == 1)
            {
                ContinueSpinCombo();
                return;
            }

            if (lightComboStep >= 2)
            {
                StartWindMill();
                return;
            }

            heavyComboStep++;
            IsAttacking = true;

            if (heavyComboStep == 1)
                animator.SetTrigger("SAttack_1");
            else if (heavyComboStep == 2)
                animator.SetTrigger("SAttack_2");

            heavyComboTimer = heavyComboResetTime;
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
        int currentItemType = UIWhiteBox.GetCurrentItemNum();
        Debug.Log(currentItemType);
        if (currentItemType == 0) return;

        if (context.performed && !isAiming)
        {
            if (!equipItem)
            {
                equipItem = true;
                weaponType = (WeaponType)currentItemType;
                idleSnake.SetActive(true);

                //아이템 장착 표시
                UIWhiteBox.SetActiveItemSelectImage(true);
            }
            else
            {
                equipItem = false;
                weaponType = WeaponType.Hammer;
                idleSnake.SetActive(false);

                //아이템 장착 표시 해제
                UIWhiteBox.SetActiveItemSelectImage(false);
            }
        }
    }

    public void EndAttack()
    {
        IsAttacking = false;
        canAttack = true;

        if (isSpinCombo && spinComboStep == 1)
        {
            return; // 기다리기
        }
        else if (isSpinCombo && spinComboStep == 2)
        {
            ResetSpinCombo();
            return;
        }

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

    private void StartSpinCombo()
    {
        isSpinCombo = true;
        spinComboStep = 1;
        IsAttacking = true;
        animator.SetTrigger("SPAttack_1");
        spinComboTimer = spinComboTimeout;
    }

    private void ContinueSpinCombo()
    {
        Debug.Log("회전 내려치기!!");
        spinComboStep = 2;
        IsAttacking = true;
        canAttack = false;
        animator.SetTrigger("SPAttack_2");
    }

    private void ResetSpinCombo()
    {
        animator.ResetTrigger("SPAttack_2");
        isSpinCombo = false;
        spinComboStep = 0;
        spinComboTimer = 0f;
    }

    public void ComboReset()
    {
        lightComboStep = 0;
        heavyComboStep = 0;
        lightComboTimer = 0f;
        heavyComboTimer = 0f;
        animator.ResetTrigger("Attack_2");
        animator.ResetTrigger("SAttack_2");
        ResetSpinCombo();
        animator.SetBool("isSpinning", false);
    }

    private void HandleLightComboReset()
    {
        if (lightComboStep <= 0) return;
        lightComboTimer -= Time.deltaTime;
        if (lightComboTimer <= 0f)
        {
            lightComboStep = 0;
            lightComboTimer = 0f;
        }
    }

    private void HandleHeavyComboReset()
    {
        if (heavyComboStep <= 0) return;
        heavyComboTimer -= Time.deltaTime;
        if (heavyComboTimer <= 0f)
        {
            heavyComboStep = 0;
            heavyComboTimer = 0f;
        }
    }



    private void HandleSpinComboTimeout()
    {
        if (!isSpinCombo || spinComboStep != 1) return;
        spinComboTimer -= Time.deltaTime;
        if (spinComboTimer <= 0f)
        {
            ResetSpinCombo();
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

    public void MosnterHitTriggerInit()
    {  
        if(attackTrigger.monsterHitBoxes.Count != 0)
        {
            for (int i = 0; i < attackTrigger.monsterHitBoxes.Count; i++)
            {
                attackTrigger.monsterHitBoxes.ElementAt(i).isTriggerHit = false;
            }
            attackTrigger.monsterHitBoxes.Clear();
        }
    }

    public void SnakeAttackObjEnable()
    {
        idleSnake.SetActive(false);
        attackSnake.SetActive(true);
    }

    public void SnakeAttackObjDisable()
    {
        idleSnake.SetActive(true);
        attackSnake.SetActive(false);
    }
    public void SetAttackTypeToLight() => attackType = AttackType.Light;
    public void SetAttackTypeToHeavy() => attackType = AttackType.Heavy;
    public void SetAttackTypeToStrong() => attackType = AttackType.Skill;
    public void SetAttackTypeToWhirlWind()=>attackType = AttackType.WhirlWind;

    public void StartWindmillTimer() => isWindmilling = true;
    public void DizzyPlay() => isDizzy = true;
    public void EnableInputAction_Attack1() => attack1Action.action.Enable();
    public void DisableInputAction_Attack1() => attack1Action.action.Disable();
    public int currentAttackType() => (int)attackType;

    [ContextMenu("화면이동 잠금")]
    public void TestMethod() => DisableInputAction_Attack1();
}
