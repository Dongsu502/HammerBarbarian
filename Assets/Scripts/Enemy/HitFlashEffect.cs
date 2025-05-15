using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashEffect : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;
    private bool isFlashing = false;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(propertyBlock);
        originalColor = targetRenderer.material.GetColor("_Color");
    }

    public void Flash()
    {
        if (isFlashing) return;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        propertyBlock.SetColor("_Color", Color.red);
        targetRenderer.SetPropertyBlock(propertyBlock);

        yield return new WaitForSeconds(flashDuration);

        propertyBlock.SetColor("_Color", originalColor);
        targetRenderer.SetPropertyBlock(propertyBlock);

        isFlashing = false;
    }
}
