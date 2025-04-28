using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterDetection : MonoBehaviour
{
    private IMonster monster;
    public Transform target { get; private set; }

    private void SetTarget(Transform _newTarget)
    {
        target = _newTarget;
    }

    private void Awake()
    {
        monster = GetComponentInParent<IMonster>();
        target = GetComponentInParent<Transform>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);

            monster.TargetDetected = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform self = GetComponentInParent<Transform>();
            SetTarget(self);

            monster.TargetDetected = false;
        }
    }
}
