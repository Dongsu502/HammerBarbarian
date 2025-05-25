using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float destroyDelayTime;

    private GameObject bullet = null;

    public void Fire()
    {
        Spawn(bulletSpawnPos);
    }

    /// <summary>
    /// 총알 스폰
    /// </summary>
    /// <param name="spawnPos">스폰 위치</param>
    public void Spawn(Transform spawnPos)
    {
        bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);

        FireDirection();
        DestroyBullet(bullet, destroyDelayTime);
    }

    private void FireDirection()
    {
        Vector3 direction = transform.forward.normalized;

        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void DestroyBullet(GameObject bullet, float delayTime)
    {
        Destroy(bullet, delayTime);
    }
}
