using System.Collections;
using UnityEngine;

public class PooledEffect : MonoBehaviour
{
    private GameObject myPrefab;
    private EffectPoolManager myManager;
    [SerializeField] private float returnDelay = 2f;
    
    private ParticleSystem[] particleSystems;

    public void SetPool(GameObject prefab, EffectPoolManager manager)
    {
        myPrefab = prefab;
        myManager = manager;
        
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Play(true);
            }
        }
        
        StartCoroutine(ReturnToPoolCoroutine());
    }

    private IEnumerator ReturnToPoolCoroutine()
    {
        yield return new WaitForSeconds(returnDelay);
        
        if (myManager != null && myPrefab != null)
        {
            myManager.ReturnEffect(myPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
