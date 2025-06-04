using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Minimap_Camera : MonoBehaviour
{
    [Tooltip("µû¶ó´Ù´Ò Å¸°Ù")]
    public Transform target;

    [Tooltip("°íÁ¤ À§Ä¡")]
    public Vector3 offset;

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 newPos = target.position + offset;

        transform.position = newPos;
    }

    public void ChangeSize(float newSize)
    {
        Camera thisCamera = GetComponent<Camera>();

        thisCamera.orthographicSize = newSize;
    }
}
