using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    private GameObject hitEffectObject;


    public void SpawnHitEffect(Vector3 hitPosition)
    {
        hitEffectObject = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
        Destroy(hitEffectObject,2f);
    }
}
