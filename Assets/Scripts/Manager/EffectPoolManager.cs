using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPoolManager : MonoBehaviour
{
    public static EffectPoolManager Instance { get; private set; }

    private Dictionary<GameObject, ObjectPool<GameObject>> effectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[EffectPoolManager] Initialized.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject SpawnEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        if (!effectPools.ContainsKey(prefab))
        {
            effectPools[prefab] = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject obj = Instantiate(prefab);
                    PooledEffect pooledEffect = obj.GetComponent<PooledEffect>();
                    if (pooledEffect == null)
                    {
                        pooledEffect = obj.AddComponent<PooledEffect>();
                    }
                    pooledEffect.SetPool(prefab, this);
                    obj.SetActive(false);
                    return obj;
                },
                actionOnGet: (obj) => { },
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        GameObject spawned = effectPools[prefab].Get();
        spawned.transform.position = position;
        spawned.transform.rotation = rotation;
        spawned.SetActive(true);

        return spawned;
    }

    public void ReturnEffect(GameObject prefab, GameObject instance)
    {
        if (effectPools.ContainsKey(prefab))
        {
            effectPools[prefab].Release(instance);
        }
        else
        {
            Destroy(instance);
        }
    }
}
