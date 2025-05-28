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

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Effect")]
    [SerializeField] private GameObject attackReadyPrefab;
    private GameObject attackReadyObject;

    private MonsterDetection detectionClass;
    private MonsterHitBox hitBoxClass;
    private MonsterHealthUI healthUIClass;
    private LongRangeAttack rangeAttackClass;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider mushroomCollider;

    private float moveAmount;

    private void Awake()
    {
        detectionClass = GetComponentInChildren<MonsterDetection>();
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();
        healthUIClass = GetComponentInChildren<MonsterHealthUI>();
        rangeAttackClass = GetComponent<LongRangeAttack>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        mushroomCollider = GetComponent<CapsuleCollider>();
    }

    #region Animation Eventkey

    /// <summary>
    /// 기모으기 이펙트 생성
    /// </summary>
    public void AttackReady()
    {
        Vector3 spawnPos = rangeAttackClass.bulletSpawnPos.position;
        attackReadyObject = Instantiate(attackReadyPrefab, spawnPos, Quaternion.identity);
    }

    /// <summary>
    /// 기모으기 끝
    /// </summary>
    public void AttackReadyFinish()
    {
        Destroy(attackReadyObject);
    }

    /// <summary>
    /// Hit 애니메이션 이벤트 키
    /// </summary>
    public void ReSetIsHitting()
    {
        animator.SetBool("IsHitting", false);

        rb.isKinematic = false;

        IsBeingHit = false;
    }

    /// <summary>
    /// Hit 애니메이션 이벤트 키
    /// </summary>
    public void ResetCollider()
    {
        hitBoxClass.hitCollider.enabled = true;
        agent.enabled = true;
    }

    public void OnisKinematic()
    {
        rb.isKinematic = true;
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

        //골렘 콜라이더 제거
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

        HitAnimation();
    }

    private void HitAnimation()
    {
        hitBoxClass.hitCollider.enabled = false;
        agent.enabled = false;

        Debug.LogWarning($"IsKnockback: {hitBoxClass.IsKnockback}");

        if (!hitBoxClass.IsKnockback)
        {
            //약공격 히트 애니메이션
            animator.SetFloat("HitType", 0);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");
            //약공격 히트 불값
            //약공격, 강공격을 애니메이션 키로 SetBool false하여 조절
            animator.SetBool("IsHitting", true);

            Debug.LogWarning("약공격");
        }
        else
        {
            //강공격 히트 애니메이션
            animator.SetFloat("HitType", 1);
            //피격 애니메이션 플레이
            animator.SetTrigger("TakeDamage");
            animator.SetBool("IsHitting", true);

            hitBoxClass.IsKnockback = false;

            Debug.LogWarning("강공격");
        }
    }

    private void DieDelay()
    {
        //Hit 콜라이더 제거
        hitBoxClass.hitCollider.enabled = false;

        //사망 애니메이션 플레이
        animator.SetTrigger("IsDie");
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
        StopAttack();
        IsHit = false;
        IsBeingHit = true;

        int hitDamage = PlayerStatWhiteBox.WhtieBox.playerAttackDamage(hitBoxClass.playerAttackType);
        TakeDamage(hitDamage);
    }
    public void Attack()
    {
        agent.enabled = false;
        InAttackRange = HasArrived();
        Debug.Log(InAttackRange);

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
