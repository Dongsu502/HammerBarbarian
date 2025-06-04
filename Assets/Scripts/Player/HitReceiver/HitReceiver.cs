using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class HitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject boomEffectPrefab;
    [SerializeField] private float effectOffset = 0.05f;
    private IMonster monster;

    private void Start()
    {
        monster = GetComponentInParent<IMonster>();
        
    }

    public void OnHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.LogWarning(monster.HP);
        if (hitEffectPrefab != null)
        {
            Vector3 spawnPos = hitPoint + hitNormal * effectOffset;
            Quaternion rot = Quaternion.LookRotation(hitNormal);
            AttackType attackType = PlayerHitWhiteBox.WhiteBox.attacktype;
            if (monster.HP <= 20 && attackType == AttackType.Heavy)
            {        
                GameObject boomEffect = Instantiate(boomEffectPrefab, spawnPos, rot);
                Destroy(boomEffect, 2f);
            }         
            GameObject effect = Instantiate(hitEffectPrefab, spawnPos, rot);
            Destroy(effect, 2f); // 2초 후 자동 파괴
        }
    }
}
