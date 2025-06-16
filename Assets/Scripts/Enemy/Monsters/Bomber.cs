using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bomber : MonoBehaviour, IMonster, IArenaRegistrable
{
    public string Name { get; private set; } = "Bomber";
    public int HP { get; private set; } = 5;
    public bool IsHit { get; set; }
    public bool IsBeingHit { get; private set; }
    public bool TargetDetected { get; set; }
    public bool InAttackRange { get; set; }
    public bool IsAttacking { get; private set; }

    [Header("State")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Space]
    [Header("attack")]
    [SerializeField] private GameObject attackCollider;
    [SerializeField] private GameObject attackRangeObj;
    [SerializeField] private GameObject currentAttackRange;
    [SerializeField] private float attackWaitingTime;
    [SerializeField] private GameObject attackEffectPrefab;

    [Space]
    [Header("Die")]
    [SerializeField] private GameObject dieEffectPrefab;
    private GameObject dieEffectObj;

    [Space]
    [SerializeField] private Transform BigHitEffectPos;

    private MonsterDetection detectionClass;
    private MonsterHitBox hitBoxClass;

    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider bomberCollider;

    private bool isDetection;

    private void Awake()
    {
        detectionClass = GetComponentInChildren<MonsterDetection>();
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bomberCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        attackCollider.SetActive(false);
        attackRangeObj.SetActive(false);
        currentAttackRange.SetActive(false);

        rb.isKinematic = true;
    }

    private ArenaController arena;

    public void Initialize(ArenaController arena)
    {
        this.arena = arena;
    }

    public void RunDelay()
    {
        isDetection = true;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입음.. 남은 체력: {HP}");

        HitAnimation();
        if (HP <= 0)
        {
            Destroy(this.gameObject, 5f);
        }
    }

    private void HitAnimation()
    {
        hitBoxClass.hitCollider.enabled = false;
        agent.enabled = false;
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

    private IEnumerator Attacking()
    {
        Debug.Log("폭탄병 공격범위 표시!");
        //공격범위 표시
        attackRangeObj.SetActive(true);
        currentAttackRange.transform.localScale = Vector3.zero;
        currentAttackRange.SetActive(true);

        // Scale을 점점 키우기
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 10f; // (10,10,10)까지 커지게

        while (elapsed < attackWaitingTime)
        {
            float t = elapsed / attackWaitingTime;
            currentAttackRange.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 마지막 Scale 보정
        currentAttackRange.transform.localScale = targetScale;

        //폭발 이펙트
        GameObject attackEffect = Instantiate(attackEffectPrefab, transform.position, attackEffectPrefab.transform.rotation);
        //공격범위 콜라이더 잠시 활성화 -> 트리거된다면 공격 성공
        attackCollider.SetActive(true);

        yield return null;

        Debug.Log("폭탄병 공격끝!");
        Destroy(attackEffect, 3f);
        Destroy(gameObject);
    }

    private IEnumerator Die()
    {
        //Hit 콜라이더 제거
        hitBoxClass.hitCollider.enabled = false;

        //중력 제거
        rb.isKinematic = true;

        //내비 제거
        agent.enabled = false;

        //폭탄병 콜라이더 제거
        bomberCollider.enabled = false;

        //리스트 제거
        arena.RemoveEnemy(gameObject);

        yield return null;

        dieEffectObj = Instantiate(dieEffectPrefab, BigHitEffectPos.position, BigHitEffectPos.rotation);

        yield return null;

        Destroy(dieEffectObj, 3f);
        Destroy(gameObject);
    }

    #region AI

    public void Death()
    {
        Debug.Log("폭탄병 사망");
        StartCoroutine(Die());
    }
    public void Hit()
    {
        if (IsBeingHit) return; // 중복 방지

        StopAllCoroutines();

        Debug.Log("폭탄병 피격");
        IsHit = false;
        IsBeingHit = true;
        attackRangeObj.SetActive(false);

        int hitDamage = PlayerStatWhiteBox.WhiteBox.playerAttackDamage(hitBoxClass.playerAttackType);
        TakeDamage(hitDamage);
    }
    public void Attack()
    {
        agent.enabled = false;
        InAttackRange = HasArrived();

        Debug.Log("폭탄병 공격시작!");
        IsAttacking = true;

        //타겟을 바라보기
        Transform target = detectionClass.target;
        LookTarget(target);

        //Idle Animation
        animator.SetTrigger("isAttack");

        StartCoroutine(Attacking());
    }
    public void MoveToTarget()
    {
        if(!isDetection)
        {
            animator.SetTrigger("isDetection");
            return;
        }

        Debug.Log("폭탄병 접근중..");
        agent.enabled = true;

        //감지된 타겟을 바라보고 추적
        Transform target = detectionClass.target;
        LookTarget(target);

        agent.SetDestination(target.position);

        InAttackRange = HasArrived();
    }
    public void Idle()
    {
        Debug.Log("폭탄병 대기중");
    }

    #endregion
}
