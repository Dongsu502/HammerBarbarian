using Google.GData.AccessControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Collider playerBlockBox;
    [SerializeField] private Player_HitReceiver playerHitReceiver;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
       if(context.started) 
       {
            animator.SetBool("isBlocking",true);
       }

       if (context.canceled)
       {
            animator.SetBool("isBlocking", false);

        }
    }
}
