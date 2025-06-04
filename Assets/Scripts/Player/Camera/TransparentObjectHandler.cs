using System.Collections.Generic;
using UnityEngine;

public class TransparentObjectHandler : MonoBehaviour
{
    private List<Renderer> renderers = new();
    private Dictionary<Renderer, Color> originalColors = new();
    private bool isTransparent = false;

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (var r in renderers)
        {
            if (r.material.HasProperty("_Color"))
                originalColors[r] = r.material.color;
        }
    }

    public void SetTransparent(float alpha = 0.3f)
    {
        if (isTransparent) return;
        isTransparent = true;

        foreach (var r in renderers)
        {
            if (!r.material.HasProperty("_Color")) continue;

            Color c = r.material.color;
            c.a = alpha;
            r.material.color = c;

            r.material.SetFloat("_Mode", 2); // Fade
            r.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            r.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            r.material.SetInt("_ZWrite", 0);
            r.material.DisableKeyword("_ALPHATEST_ON");
            r.material.EnableKeyword("_ALPHABLEND_ON");
            r.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            r.material.renderQueue = 3000;
        }
    }

    public void Restore()
    {
        if (!isTransparent) return;
        isTransparent = false;

        foreach (var r in renderers)
        {
            if (!originalColors.ContainsKey(r)) continue;

            r.material.color = originalColors[r];

            r.material.SetFloat("_Mode", 0); // Opaque
            r.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            r.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            r.material.SetInt("_ZWrite", 1);
            r.material.DisableKeyword("_ALPHABLEND_ON");
            r.material.renderQueue = -1;
        }
    }
}
