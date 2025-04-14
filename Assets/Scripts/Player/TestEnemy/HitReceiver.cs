using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;

    public void OnHit(Vector3 hitPoint,Vector3 hitNormal)
    {
        if (hitEffectPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(hitNormal);
            Instantiate(hitEffectPrefab,hitPoint, rot);
        }
    }
}
