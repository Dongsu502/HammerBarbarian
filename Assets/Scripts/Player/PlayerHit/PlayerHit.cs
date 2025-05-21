using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField] private PlayerStatus status;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack_Golem"))
        {
            animator.applyRootMotion = true;
            animator.SetTrigger("Hit");
        }
    }
}
