using System.Collections.Generic;
using UnityEngine;

public class TransparentObjectHandler : MonoBehaviour
{
    private List<Renderer> renderers = new();
    private Dictionary<Renderer, Color> originalColors = new();
    private bool isTransparent = false;

    [SerializeField] private Material transparentMaterial;
    private Dictionary<Renderer, Material> originalMats = new();

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                originalColors[r] = r.material.color;
        }
    }

    public void SetTransparent(float alpha = 0.2f)
    {
        if (isTransparent) return;
        isTransparent = true;

        foreach (var r in renderers)
        {
            if (!originalMats.ContainsKey(r))
                originalMats[r] = r.sharedMaterial;

            r.material = transparentMaterial;
        }
    }

    public void Restore()
    {
        if (!isTransparent) return;
        isTransparent = false;

        foreach (var r in renderers)
        {
            if (originalMats.TryGetValue(r, out var mat))
                r.material = mat;
        }
    }
}
