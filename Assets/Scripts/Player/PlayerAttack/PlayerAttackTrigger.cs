using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] private PlayerAnimStateChecker animChecker;

    [SerializeField]private Transform hitOrigin;

    [SerializeField] HitStopHandler hitStopHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyHitBox"))
        {
            //cameraShakeTrigger.Shake();

            if (!animChecker.IsWhirlwindAnim())
            {
                hitStopHandler.HitStop(0.1f);
            }
            
            Vector3 hitPoint = other.ClosestPoint(hitOrigin.position);
            Vector3 hitNormal = (other.transform.position - hitOrigin.position).normalized;

            HitReceiver receiver = other.GetComponent<HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
        }
    }

   

}
