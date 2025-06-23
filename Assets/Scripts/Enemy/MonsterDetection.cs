using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterDetection : MonoBehaviour
{
    [SerializeField] private MonsterDetectedUI monsterDetectedUI;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (monster.TargetDetected) return;

            monsterDetectedUI.DetectionCheck(true);

            SetTarget(other.transform);

            monster.TargetDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterDetectedUI.DetectionCheck(false);

            Transform self = GetComponentInParent<Transform>();
            SetTarget(self);

            monster.TargetDetected = false;
        }
    }
}
