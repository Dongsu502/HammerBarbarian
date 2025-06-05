using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUICamera : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void Update()
    {
        FollowTarget(target);
    }

    private void FollowTarget(GameObject newTarget)
    {
        transform.SetPositionAndRotation(newTarget.transform.position, newTarget.transform.rotation);
    }
}
