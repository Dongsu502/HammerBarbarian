using System.Collections;
using UnityEngine;

public class RopeWeaponCollisionHandler : MonoBehaviour
{
    private HammerThrowController controller;
    private Rigidbody hammerRb;
    private RopePullController ropePullController;

    private bool isStuckToWall = false;

    public bool IsStuckToWall => isStuckToWall;

    private void Awake()
    {
        ropePullController = FindAnyObjectByType<RopePullController>().GetComponent<RopePullController>();
        hammerRb = GetComponent<Rigidbody>();
    }

    public void SetController(HammerThrowController controller)
    {
        this.controller = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("InteractionWall"))
        {
            // 물리 고정 (움직이지 않도록)
           
            hammerRb.velocity = Vector3.zero;
            hammerRb.isKinematic = true;

            isStuckToWall = true;
            ropePullController.TryStartPull();
            return;
        }

        if (collision.collider.CompareTag("EnemyHitBox"))
        {

        }

        // 일반적인 충돌은 바로 회수
        controller?.Recall();
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
