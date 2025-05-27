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
        hammer.SetActive(false);
        animator.SetBool("isThrowing",true);
    }

    public void StopThrow()
    {
        hammer.SetActive(true);
        animator.SetBool("isThrowing", false);
    }
}
