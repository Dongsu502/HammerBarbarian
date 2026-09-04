using System.Collections.Generic;
using UnityEngine;

public class CameraObstructionFader : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask transparentLayer;
    [SerializeField] private float radius = 1f;

    private HashSet<TransparentObjectHandler> currentlyFaded = new();
    private HashSet<TransparentObjectHandler> detectedThisFrame = new();

    private readonly RaycastHit[] hitBuffer = new RaycastHit[64];

    private void LateUpdate()
    {
        if (target == null)
        {
            RestoreAll();
            return;
        }

        Vector3 from = transform.position;
        Vector3 direction = target.position - from;
        float distance = direction.magnitude;

        if (distance <= Mathf.Epsilon)
        {
            RestoreAll();
            return;
        }

        detectedThisFrame.Clear();

        int hitCount = Physics.SphereCastNonAlloc(
            from,
            radius,
            direction.normalized,
            hitBuffer,
            distance,
            transparentLayer,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider hitCollider = hitBuffer[i].collider;

            if (hitCollider == null)
                continue;

            TransparentObjectHandler handler =
                hitCollider.GetComponentInParent<TransparentObjectHandler>();

            if (handler != null)
                detectedThisFrame.Add(handler);
        }

        foreach (var handler in currentlyFaded)
        {
            if (handler != null && !detectedThisFrame.Contains(handler))
            {
                handler.Restore();
            }
        }

        foreach (var handler in detectedThisFrame)
        {
            if (!currentlyFaded.Contains(handler))
            {
                handler.SetTransparent();
            }
        }

        var temp = currentlyFaded;
        currentlyFaded = detectedThisFrame;
        detectedThisFrame = temp;
    }

    private void OnDisable()
    {
        RestoreAll();
    }

    private void RestoreAll()
    {
        foreach (var handler in currentlyFaded)
        {
            if (handler != null)
                handler.Restore();
        }

        currentlyFaded.Clear();
        detectedThisFrame.Clear();
    }
}