using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    [Header("Monster Setting")]
    [Tooltip("감지범위")]
    public float detectRange = 10f;
    [Tooltip("공격범위")]
    public float attackRange = 2f;
    [Tooltip("이동속도")]
    public float moveSpeed = 3.5f;
    [Tooltip("체력")]
    public int health = 100;
    [Tooltip("회전속도")]
    public float rotateSpeed = 10f;
    [Tooltip("공격딜레이")]
    public float attackDelay = 1.2f;

    [Space(10)]
    [Tooltip("목표")]
    public Transform target;

    [Space(10)]
    [Tooltip("애니메이션")]
    [SerializeField] protected Animator animator;

    public bool isMoving;
    
    protected float moveAmount;

    protected NavMeshAgent agent;

    protected Coroutine machine;

    protected MonsterHitBox hitDetection;

    protected Rigidbody rb;

    public MonsterHealthUI healthUI;

    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }
    State currentState;

    #region UnityCallFunc

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        currentState = State.IDLE;
        machine = StartCoroutine(MonsterStateMachine());

        hitDetection = GetComponentInChildren<MonsterHitBox>();
    }

    #endregion

    #region IEnumerator Machine

    protected virtual IEnumerator MonsterStateMachine()
    {
        while(health > 0)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    /// <summary>
    /// 대기
    /// </summary>
    protected virtual IEnumerator IDLE()
    {
        //Idle Animation
        isMoving = false;
        MoveAnimation(isMoving);

        if (target != null)
        {
            ChangeState(State.CHASE);
            yield break;
        }

        if (!agent.enabled)
        {
            yield break;
        }
        agent.SetDestination(transform.position);

        
    }

    /// <summary>
    /// 추적
    /// </summary>
    protected virtual IEnumerator CHASE()
    {
        //Run Animation
        isMoving = true;
        MoveAnimation(isMoving);

        if (target == null)
        {
            ChangeState(State.IDLE);
            Debug.Log("추적중단..");
            yield break;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            ChangeState(State.ATTACK);
            Debug.Log("추적중단.. 공격!");
            yield break;
        }

        if (!agent.enabled)
        {
            yield break;
        }

        agent.SetDestination(target.position);

        LookTarget();
    }

    /// <summary>
    /// 공격
    /// </summary>
    protected virtual IEnumerator ATTACK()
    {
        //ChangeState_IDLE
        if (target == null)
        {
            animator.SetBool("IsAttack", false);

            ChangeState(State.IDLE);
            Debug.Log("공격도중 적을 찾지못함..");
            yield break;
        }

        //ChangeState_CHASE
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            animator.SetBool("IsAttack", false);

            ChangeState(State.CHASE);
            Debug.Log("다시 추적!");
            yield break;
        }

        // 타겟 방향 계산
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 회전 완료될 때까지 대기
        while (Quaternion.Angle(transform.rotation, targetRotation) > 3f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        //LookTarget();


        if (!agent.enabled)
        {
            yield break;
        }
        // 회전 완료 후 공격
        agent.SetDestination(transform.position); // 정지

        //Attack Animation
        animator.SetBool("IsAttack", true);

        var curAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float delay = curAnimStateInfo.length * attackDelay;

        //Attack
        yield return new WaitForSeconds(delay);
        ChangeState(State.ATTACK);
    }

    /// <summary>
    /// 피격
    /// </summary>
    protected virtual IEnumerator HIT()
    {
        //Hit Animation
        //if (hitDetection.IsKnockback)
        //{
        //    animator.SetTrigger("IsDown");
        //}
        //else
        //{
        //    animator.SetTrigger("TakeDamage");
        //}
        
        animator.SetBool("IsAttack", false);
        Debug.Log("아프다..");

        //yield return new WaitForSeconds(hitDetection.knockbackDuration);
        rb.isKinematic = true;

        //yield return new WaitForSeconds(hitDetection.knockbackDuration);

        KnocbackActive(true);
        rb.isKinematic = false;
        

        ChangeState(State.IDLE);
        yield break;
    }

    /// <summary>
    /// 사망
    /// </summary>
    protected virtual IEnumerator DIE()
    {
        //Die Animation

        Debug.Log("사망..");
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    #endregion

    #region MonsterFunc

    protected virtual void LookTarget()
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
    /// MoveBlend의 Move값 변경
    /// </summary>
    /// <param name="isMoving">이동중이면 true, 정지상태면 false</param>
    protected virtual void MoveAnimation(bool isMoving)
    {
        float target = isMoving ? 1f : 0f;
        
        moveAmount = Mathf.MoveTowards(moveAmount, target, Time.deltaTime * 2f); // 2f는 속도, 조절 가능

        animator.SetFloat("Move", moveAmount);
    }

    /// <summary>
    /// State 변경
    /// </summary>
    /// <param name="_newState">변경하고싶은 State</param>
    protected virtual void ChangeState(State _newState)
    {
        currentState = _newState;

        if(machine != null)
        {
            StopCoroutine(machine);
        }

        machine = StartCoroutine(MonsterStateMachine());
    }

    /// <summary>
    /// 피격
    /// </summary>
    /// <param name="damage">받을 데미지값</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입음.. 남은 체력: {health}");

        if(health > 0)
        {
            Debug.Log("피격 애니메이션 호출");
            ChangeState(State.HIT);
        }
        else
        {
            MonsterDie();
        }
    }

    /// <summary>
    /// 넉백 시 비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public virtual void KnocbackActive(bool isActive)
    {
        agent.enabled = isActive;
        hitDetection.hitCollider.enabled = isActive;
        //hitDetection.IsKnockback = !isActive;
    }

    /// <summary>
    /// 사망
    /// </summary>
    protected virtual void MonsterDie()
    {
        Debug.Log($"{gameObject.name}이(가) 사망하였습니다.");

        StopCoroutine(machine);
        StartCoroutine(DIE());
    }

    #endregion
}
