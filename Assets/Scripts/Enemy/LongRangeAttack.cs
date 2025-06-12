using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public Transform bulletSpawnPos;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletMaxDistance;

    private GameObject bullet = null;
    //private MonsterDetection monsterDetection;

    private void Awake()
    {
        //monsterDetection = GetComponentInChildren<MonsterDetection>();
    }

    private void FixedUpdate()
    {
        if(bullet != null)
        {
            BulletDestroy();
        }
    }

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
    }

    private void FireDirection()
    {
        Vector3 direction = gameObject.transform.forward;

        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void BulletDestroy()
    {
        float distance = Vector3.Distance(bullet.transform.position, gameObject.transform.position);

        if(bulletMaxDistance <= distance)
        {
            Destroy(bullet);
        }
    }
}
