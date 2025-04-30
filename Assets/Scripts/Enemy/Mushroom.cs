using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : MonoBehaviour, IMonster
{
    public string Name { get; private set; } = "Mushroom";
    public int HP { get; private set; } = 50;
    public bool IsHit { get; set; }
    public bool IsBeingHit { get; private set; }
    public bool TargetDetected { get; set; }
    public bool InAttackRange { get; set; }
    public bool IsAttacking { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float hitDelayTime;
    [SerializeField] private float knockbackDelayTime;
    [SerializeField] private float dieDelayTime;
    [SerializeField] private Transform bulletSpawnPos;

    private MonsterDetection detectionClass;
    private MonsterHitBox hitBoxClass;
    private MonsterHealthUI healthUIClass;
    private LongRangeAttack longAttackClass;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider golemCollider;

    private float moveAmount;

    private void Awake()
    {
        detectionClass = GetComponentInChildren<MonsterDetection>();
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();
        healthUIClass = GetComponentInChildren<MonsterHealthUI>();
        longAttackClass = GetComponent<LongRangeAttack>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        golemCollider = GetComponent<CapsuleCollider>();
    }

    public void ReSetIsHitting()
    {
        animator.SetBool("IsHitting", false);
    }

    /// <summary>
    /// 공격 중단 ( 공격 애니메이션 끝에 이벤트 키 배치)
    /// </summary>
    public void StopAttack()
    {
        IsAttacking = false;
        animator.SetBool("IsAttack", false);
        Debug.Log("공격 중단");
    }

    public void LongAttack()
    {
        longAttackClass.Spawn(bulletSpawnPos);
    }

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
        HP -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입음.. 남은 체력: {HP}");

        healthUIClass.TakeDamageUI(damage);

        StartCoroutine(HitDelay());
    }

    private IEnumerator HitDelay()
    {
        hitBoxClass.hitCollider.enabled = false;
        agent.enabled = false;

        if (!hitBoxClass.IsKnockback)
        {
            //약공격 히트 애니메이션
            animator.SetFloat("HitType", 0);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");
            //약공격 히트 불값
            animator.SetBool("IsHitting", true);

            yield return new WaitForSeconds(hitDelayTime);
        }
        else
        {
            //강공격 히트 애니메이션
            animator.SetFloat("HitType", 1);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");

            hitBoxClass.IsKnockback = false;

            yield return new WaitForSeconds(knockbackDelayTime);
        }

        hitBoxClass.hitCollider.enabled = true;
        agent.enabled = true;

        rb.isKinematic = true;

        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;

        IsBeingHit = false;
    }

    private IEnumerator DieDelay()
    {
        //콜라이더 제거
        hitBoxClass.hitCollider.enabled = false;
        golemCollider.enabled = false;

        //중력 제거
        rb.useGravity = false;

        //내비 제거
        agent.enabled = false;

        //사망 애니메이션 플레이
        animator.SetBool("IsDie", true);

        yield return new WaitForSeconds(dieDelayTime);

        Destroy(gameObject);
    }

    #region AI

    public void Death()
    {
        Debug.Log("버섯 사망");
        StartCoroutine(DieDelay());
    }
    public void Hit()
    {
        if (IsBeingHit) return; // 중복 방지

        //공격 도중 맞으면 공격 중단 후 바로 피격으로 전환
        StopAttack();
        IsHit = false;
        IsBeingHit = true;
        TakeDamage(10);
    }
    public void Attack()
    {
        agent.enabled = false;
        InAttackRange = HasArrived();

        Debug.Log("버섯 공격 시작");
        IsAttacking = true;

        // 공격 모션 중 이동 막기
        MoveAnimation(false);

        //타겟을 바라보기
        Transform target = detectionClass.target;
        LookTarget(target);

        //공격 애니메이션 플레이
        animator.SetBool("IsAttack", true);
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
