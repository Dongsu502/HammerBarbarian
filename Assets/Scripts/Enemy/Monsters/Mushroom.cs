using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : MonoBehaviour, IMonster
{
    public string Name { get; private set; } = "Mushroom";
    public int HP { get; private set; } = 30;
    public bool IsHit { get; set; }
    public bool IsBeingHit { get; private set; }
    public bool TargetDetected { get; set; }
    public bool InAttackRange { get; set; }
    public bool IsAttacking { get; private set; }

    [Header("허수아비모드")]
    [SerializeField] private bool isDummyMode;

    [Space]
    [Header("State")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attackDelayTime;

    [Header("Effect")]
    [SerializeField] private GameObject attackReadyPrefab;
    private GameObject attackReadyObject;

    private MonsterDetection detectionClass;
    private MonsterHitBox hitBoxClass;
    private MonsterHealthUI healthUIClass;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider mushroomCollider;
    private Coroutine attackCoroutine;

    private float moveAmount;
    private bool lookTargetCheck = false;

    private void Awake()
    {
        detectionClass = GetComponentInChildren<MonsterDetection>();
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();
        healthUIClass = GetComponentInChildren<MonsterHealthUI>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        mushroomCollider = GetComponent<CapsuleCollider>();
    }

    #region Animation Eventkey

    /// <summary>
    /// Hit 애니메이션 이벤트 키
    /// </summary>
    public void OffisKinematic()
    {
        rb.isKinematic = false;
    }

    /// <summary>
    /// Hit 애니메이션 이벤트 키
    /// </summary>
    public void ResetisHiting()
    {
        //hitBoxClass.hitCollider.enabled = true;
        agent.enabled = true;

        animator.SetBool("IsHitting", false);

        IsBeingHit = false;
    }

    public void OnisKinematic()
    {
        rb.isKinematic = true;
    }

    /// <summary>
    /// 공격 중단 ( 공격 애니메이션 끝에 이벤트 키 배치)
    /// </summary>
    public void StopAttackAnim()
    {
        animator.SetBool("IsAttack", false);
    }

    /// <summary>
    /// Die 애니메이션 키 이벤트
    /// </summary>
    public void DestroyDelay()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Die 애니메이션 키 이벤트
    /// </summary>
    public void RBDestory()
    {
        //중력 제거
        rb.isKinematic = true;

        //내비 제거
        agent.enabled = false;

        //버섯 콜라이더 제거
        mushroomCollider.enabled = false;
    }

    #endregion

    /// <summary>
    /// Idle, Move 애니메이션
    /// </summary>
    /// <param name="isMoving">true: Move, false: Idle</param>
    private void MoveAnimation(bool isMoving)
    {
        float target = isMoving ? 1f : 0f;

        moveAmount = Mathf.MoveTowards(moveAmount, target, Time.deltaTime * 2f); // 2f는 속도, 조절 가능

        animator.SetFloat("Move", moveAmount);
    }

    /// <summary>
    /// 타겟 바라보기
    /// </summary>
    /// <param name="target">바라볼 목표</param>
    private void LookTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f; // 수평 회전만
        if (dir.sqrMagnitude > 0f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

            float angle = Quaternion.Angle(transform.rotation, lookRotation);
            if (angle < 1f)
            {
                //회전 거의 종료
                lookTargetCheck = true;
            }
            else
            {
                //아직 회전중..
                lookTargetCheck = false;
            }
        }
        else
        {
            lookTargetCheck = true;
        }
    }

    /// <summary>
    /// 타겟에게 도착했는지 확인
    /// </summary>
    /// <returns>true: 도착, false: 도착 전</returns>
    private bool HasArrived()
    {
        float targetToDistance = Vector3.Distance(transform.position, detectionClass.target.position);
        // 이동 중 남은 거리를 체크해서
        if (targetToDistance <= agent.stoppingDistance + 0.3f)
        {
            // 공격할 수 있는 범위에 들어왔다고 세팅
            return true;
        }
        else
        {
            // 아직 도착 안 했으면 false
            return false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDummyMode)
        {
            HP -= damage;
            Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입음.. 남은 체력: {HP}");

            healthUIClass.TakeDamageUI(damage);
        }

        HitAnimation();
        //임시코드
        if (HP <= 0)
        {
            animator.SetTrigger("IsDie");
            Destroy(this.gameObject, 10f);
        }
    }

    private void HitAnimation()
    {
        if (!hitBoxClass.IsKnockback)
        {
            //약공격 히트 애니메이션
            animator.SetFloat("HitType", 0);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");
            //약공격 히트 불값
            //약공격, 강공격을 애니메이션 키로 SetBool false하여 조절
            animator.SetBool("IsHitting", true);
        }
        else
        {
            //강공격 히트 애니메이션
            animator.SetFloat("HitType", 1);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");
            animator.SetBool("IsHitting", true);

            hitBoxClass.IsKnockback = false;
        }
    }

    private IEnumerator AttackDelay()
    {
        agent.enabled = false;
        InAttackRange = HasArrived();

        Debug.Log("버섯 공격 시작");
        IsAttacking = true;

        // 공격 모션 중 이동 막기
        MoveAnimation(false);

        //타겟을 바라보기
        Transform target = detectionClass.target;
        while (!lookTargetCheck)
        {
            LookTarget(target);
            Debug.Log("버섯 타겟팅중..");

            yield return null;
        }

        //공격 애니메이션 플레이
        animator.SetBool("IsAttack", true);

        yield return new WaitForSeconds(attackDelayTime);

        lookTargetCheck = false;

        IsAttacking = false;

        Debug.Log("버섯 공격 끝");
    }

    private void DieDelay()
    {
        //Hit 콜라이더 제거
        hitBoxClass.hitCollider.enabled = false;

        //사망 애니메이션 플레이
        animator.SetTrigger("IsDie");

        //체력바 UI 비활성화
        healthUIClass.HPBar_SetActive(false);
    }

    #region AI

    public void Death()
    {
        Debug.Log("버섯 사망");
        DieDelay();
    }
    public void Hit()
    {
        if (IsBeingHit) return; // 중복 방지

        //공격 도중 맞으면 공격 중단 후 바로 피격으로 전환
        StopAttackAnim();
        IsAttacking = false;
        StopCoroutine(attackCoroutine);

        IsHit = false;
        IsBeingHit = true;
        //hitBoxClass.hitCollider.enabled = false;
        agent.enabled = false;

        int hitDamage = PlayerStatWhiteBox.WhiteBox.playerAttackDamage(hitBoxClass.playerAttackType);
        TakeDamage(hitDamage);
    }
    public void Attack()
    {
        if (isDummyMode) return;

        if (IsAttacking) return;
        attackCoroutine = StartCoroutine(AttackDelay());
    }
    public void MoveToTarget()
    {
        Debug.Log("버섯 접근중..");
        agent.enabled = true;

        //감지된 타겟을 바라보고 추적
        Transform target = detectionClass.target;
        LookTarget(target);

        agent.SetDestination(target.position);

        InAttackRange = HasArrived();
        Debug.Log(InAttackRange);

        //걷기 애니메이션 플레이
        animator.SetBool("IsAttack", false);
        MoveAnimation(true);

    }
    public void Idle()
    {
        agent.enabled = false;

        //Idle 애니메이션 플레이
        MoveAnimation(false);
        Debug.Log("버섯 대기중");
    }

    #endregion
}
