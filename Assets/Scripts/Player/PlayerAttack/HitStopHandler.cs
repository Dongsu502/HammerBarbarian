using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopHandler : MonoBehaviour
{
    private bool isHitStopping = false;
    [SerializeField]private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void HitStop(float duration)
    {
        if (!isHitStopping)
        {
            StartCoroutine(HitStopCoroutine(duration));
        }
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        isHitStopping=true;

        float originalSpeed = animator.speed;
        animator.speed = 0f;

        yield return new WaitForSeconds(duration);

        animator.speed = originalSpeed;
        isHitStopping = false;
    }
}
