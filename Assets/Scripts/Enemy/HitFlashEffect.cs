using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashEffect : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [Tooltip("이펙트 머티리얼 1")]
    [SerializeField] private Material flashMaterial;
    [Tooltip("이펙트 머티리얼 2")]
    [SerializeField] private Material flashMateria2;
    [Tooltip("이펙트 시간")]
    [SerializeField] private float flashDuration = 0.1f;

    private Material[] originalMaterials;
    [SerializeField] private bool isFlashing = false;

    private void Awake()
    {
        originalMaterials = targetRenderer.materials;
    }

    public void Flash()
    {
        if (isFlashing) return;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        // 흰색 머티리얼로 변경
        targetRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        //은색 머티리얼로 변경
        targetRenderer.material = flashMateria2;

        yield return new WaitForSeconds(flashDuration);

        // 원래대로 복구
        targetRenderer.materials = originalMaterials;
        isFlashing = false;
    }
}