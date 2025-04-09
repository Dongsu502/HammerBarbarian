using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] CameraShakeTrigger cameraShakeTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            cameraShakeTrigger.Shake();
        }
    }


}
