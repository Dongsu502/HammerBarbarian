using UnityEngine;

public class PlayerAnimStateChecker : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsTag(string tag)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
    }

    public bool IsAttackAnim() => IsTag("Attack");
    public bool IsDiveAnim() => IsTag("Dive");
    public bool IsHitAnim() => IsTag("Hit");
    public bool IsBlockAnim() => IsTag("Block");
    public bool IsWhirlwindAnim() => IsTag("Whirlwind");
}
