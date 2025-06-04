using System.Collections.Generic;
using UnityEngine;

public class CameraObstructionFader : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask transparentLayer;
    private List<TransparentObjectHandler> currentlyFaded = new();

    void LateUpdate()
    {
        foreach (var handler in currentlyFaded)
        {
            if (handler != null)
                handler.Restore();
        }
        currentlyFaded.Clear();

        Vector3 from = transform.position;
        Vector3 to = target.position;
        Vector3 dir = to - from;
        float dist = dir.magnitude;

        float radius = 0.5f; // 조절 가능
        RaycastHit[] hits = Physics.SphereCastAll(from, radius, dir, dist, transparentLayer);

        Debug.DrawRay(from, dir.normalized * dist, Color.green);
        Debug.DrawLine(from + Vector3.left * radius, to + Vector3.left * radius, Color.red);
        Debug.DrawLine(from + Vector3.right * radius, to + Vector3.right * radius, Color.red);

        foreach (var hit in hits)
        {
            TransparentObjectHandler handler = hit.collider.GetComponentInParent<TransparentObjectHandler>();
            if (handler != null && !currentlyFaded.Contains(handler))
            {
                handler.SetTransparent();
                currentlyFaded.Add(handler);
            }
        }

    }
}
