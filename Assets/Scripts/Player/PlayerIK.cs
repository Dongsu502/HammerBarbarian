using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    private PlayerMove playerMove;
    [SerializeField] private Animator animator;
    [SerializeField]private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;

    private float leftHandIKWeight = 1f;
    private float rightHandIKWeight = 1f;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    public void SetLeftHandIKWeight(float weight)
    {
        leftHandIKWeight = weight;
    }

    public void SetRightHandIKWeight(float weight)
    {
        rightHandIKWeight = weight;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (leftHandTarget != null&&playerMove.currentAnimSpeed <0.1f)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
}
