using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterDetection : MonoBehaviour
{
    private Monster monster;

    [SerializeField] private SphereCollider detectCollider;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();

        detectCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        detectCollider.radius = monster.detectRange;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.target = other.transform;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.target = null;
        }
    }
}
