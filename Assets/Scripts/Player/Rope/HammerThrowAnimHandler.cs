using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerThrowAnimHandler : MonoBehaviour
{
    [SerializeField] private GameObject hammer;
    //[SerializeField] private Material weaponMaterial;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    public void StartThrow()
    {
        //hammer.SetActive(false);
        SkinnedMeshRenderer skinnedMeshRenderer = hammer.GetComponent<SkinnedMeshRenderer>();

        Material weaponMaterial = skinnedMeshRenderer.materials[2];
        Color c = weaponMaterial.color;
        c.a = 0f;
        weaponMaterial.color = c;
        animator.SetBool("isThrowing",true);
    }

    public void StopThrow()
    {
        //hammer.SetActive(true);
        SkinnedMeshRenderer skinnedMeshRenderer = hammer.GetComponent<SkinnedMeshRenderer>();

        Material weaponMaterial = skinnedMeshRenderer.materials[2];
        Color c = weaponMaterial.color;
        c.a = 1f;
        weaponMaterial.color = c;
        animator.SetBool("isThrowing", false);
    }
}
