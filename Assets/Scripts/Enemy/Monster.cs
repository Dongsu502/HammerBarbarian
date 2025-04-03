using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    public int health = 100; //체력

    [Space(10)]
    [Tooltip("감지 콜라이더")]
    [SerializeField] protected SphereCollider detectCollider;

    protected NavMeshAgent agent;
    protected Transform target;

    protected Coroutine machine;

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

        detectCollider.radius = detectRange;
    }

    protected virtual void Start()
    {
        currentState = State.IDLE;
        machine = StartCoroutine(MonsterStateMachine());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
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
        Debug.Log("정지 정지!");

        if (target != null)
        {
            ChangeState(State.CHASE);
            yield break;
        }
    }

    /// <summary>
    /// 추적
    /// </summary>
    protected virtual IEnumerator CHASE()
    {
        if (target == null)
        {
            ChangeState(State.IDLE);
            Debug.Log("추적중단..");
            yield break;
        }
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            ChangeState(State.ATTACK);
            Debug.Log("추적중단.. 공격!");
            yield break;
        }

        //Run Animation

        agent.SetDestination(target.position);
        Debug.Log("추적중..");
    }

    /// <summary>
    /// 공격
    /// </summary>
    protected virtual IEnumerator ATTACK()
    {
        //Attack Animation
        Debug.Log($"{gameObject.name}이(가) 플레이어를 공격!!");

        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            ChangeState(State.CHASE);
            Debug.Log("다시 추적!");
            yield break;
        }
    }

    /// <summary>
    /// 피해
    /// </summary>
    protected virtual IEnumerator HIT()
    {
        //Hit Animation

        Debug.Log("아프다..");

        yield return new WaitForSeconds(1f);

        ChangeState(State.IDLE);
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

    /// <summary>
    /// State 변경
    /// </summary>
    /// <param name="_newState">변경하고싶은 State</param>
    protected virtual void ChangeState(State _newState)
    {
        currentState = _newState;
    }

    /// <summary>
    /// 피해
    /// </summary>
    /// <param name="damage">받을 데미지값</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 피해를 입음.. 남은 체력: {health}");

        if(health > 0)
        {
            ChangeState(State.HIT);
        }
        else
        {
            MonsterDie();
        }
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
