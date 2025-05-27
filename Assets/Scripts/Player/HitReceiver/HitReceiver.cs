using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float effectOffset = 0.05f;

    public void OnHit(Vector3 hitPoint,Vector3 hitNormal)
    {
        if (hitEffectPrefab != null)
        {
            Vector3 spawnPos = hitPoint + hitNormal * effectOffset;
            Quaternion rot = Quaternion.LookRotation(hitNormal);

            Instantiate(hitEffectPrefab, spawnPos, rot);
        }
    }
}
