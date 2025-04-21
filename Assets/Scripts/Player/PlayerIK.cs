using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField]private Transform leftHandTarget;

    private float leftHandIKWeight = 1f;

    public void SetLeftHandIKWeight(float weight)
    {
        leftHandIKWeight = weight;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
}
