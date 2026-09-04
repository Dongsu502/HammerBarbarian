using System.Collections.Generic;
using UnityEngine;

public class TransparentObjectHandler : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;

    private readonly List<Renderer> renderers = new();
    private readonly Dictionary<Renderer, Material> originalMaterials = new();

    private bool isTransparent;

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>(true));

        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                originalMaterials[renderer] = renderer.sharedMaterial;
            }
        }
    }

    public void SetTransparent()
    {
        if (isTransparent || transparentMaterial == null)
            return;

        isTransparent = true;

        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.sharedMaterial = transparentMaterial;
            }
        }
    }

    public void Restore()
    {
        if (!isTransparent)
            return;

        isTransparent = false;

        foreach (var renderer in renderers)
        {
            if (renderer != null &&
                originalMaterials.TryGetValue(renderer, out Material originalMaterial))
            {
                renderer.sharedMaterial = originalMaterial;
            }
        }
    }
}