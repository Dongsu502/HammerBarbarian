using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] CameraShakeTrigger cameraShakeTrigger;
    [SerializeField] HitStopHandler hitStopHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            cameraShakeTrigger.Shake();
            hitStopHandler.HitStop(0.1f);

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = (other.transform.position - transform.position).normalized;

            HitReceiver receiver = other.GetComponent<HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
        }
    }


}
