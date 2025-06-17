using System.Collections;
using UnityEngine;

public class RopeWeaponCollisionHandler : MonoBehaviour
{
    private HammerThrowController controller;
    private Rigidbody hammerRb;
    private RopePullController ropePullController;

    [SerializeField] private Transform hitOrigin;
    [SerializeField] private PlayerAttackTrigger attackTrigger;

    private bool isStuckToWall = false;

    public bool IsStuckToWall => isStuckToWall;

    private void Awake()
    {
        ropePullController = FindAnyObjectByType<RopePullController>().GetComponent<RopePullController>();
        attackTrigger = FindAnyObjectByType<PlayerAttackTrigger>().GetComponent<PlayerAttackTrigger>();
        hammerRb = GetComponent<Rigidbody>();
    }

    public void SetController(HammerThrowController controller)
    {
        this.controller = controller;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("InteractionWall"))
    //    {
    //        // 물리 고정 (움직이지 않도록)

    //        hammerRb.velocity = Vector3.zero;
    //        hammerRb.isKinematic = true;

    //        isStuckToWall = true;
    //        ropePullController.TryStartPull();
    //        return;
    //    }

    //    if (collision.collider.CompareTag("EnemyHitBox"))
    //    {
    //        ContactPoint contact = collision.contacts[0];
    //        Vector3 hitPoint = contact.point;
    //        Vector3 hitNormal = contact.normal;

    //        HitReceiver receiver = collision.collider.GetComponent<HitReceiver>();
    //        if (receiver != null)
    //        {
    //            receiver.OnHit(hitPoint + hitNormal * 0.05f, hitNormal); // 표면 살짝 튀어나오게
    //        }
    //    }


    //    // 일반적인 충돌은 바로 회수
    //    controller?.Recall();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionWall"))
        {
            Debug.Log("그 벽에 닿음");

            // 물리 속도 멈추기 → Kinematic 설정 순서 중요!
            hammerRb.isKinematic = true;

            isStuckToWall = true;
            ropePullController.TryStartPull();
            return;
        }

        MonsterHitBox monsterHitBox = other.GetComponent<MonsterHitBox>();

        if (other.CompareTag("EnemyHitBox") && !attackTrigger.monsterHitBoxes.Contains(monsterHitBox))
        {
            attackTrigger.monsterHitBoxes.Add(monsterHitBox);

            Vector3 hitPoint = other.ClosestPoint(hitOrigin.position);
            Vector3 hitNormal = (other.transform.position - hitOrigin.position).normalized;

            HitReceiver receiver = other.GetComponent<HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
            controller?.Recall();
            return;
        }

        controller.Recall();
    }

    // 외부에서 다시 움직이도록 허용할 때 호출
    public void UnstickFromWall()
    {
        if (hammerRb != null)
        {
            Debug.Log("떨어져라!");
            hammerRb.isKinematic = false;
            isStuckToWall = false;
            controller?.Recall();
        }
    }
}
