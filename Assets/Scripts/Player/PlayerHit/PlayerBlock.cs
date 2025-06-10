using Google.GData.AccessControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Collider playerBlockBox;
    [SerializeField] private Player_HitReceiver playerHitReceiver;
    private bool isBlocking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        BlockStateHandler();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        float steminaGauge = UIWhiteBox.GetGauge();
        if (steminaGauge < 25f) return;
        if (context.started) 
        {
           StartBlock();
        }

       if (context.canceled)
       {
           EndBlock();
       }
    }

    private void StartBlock()
    {
        //isBlocking = true;
        UIWhiteBox.SetisGaugeRecovery(false);
        animator.SetBool("isBlocking", true);
        playerBlockBox.enabled = true;
    }

    private void EndBlock()
    {
        //isBlocking= false;
        UIWhiteBox.SetisGaugeRecovery(true);
        animator.SetBool("isBlocking", false);
        playerBlockBox.enabled = false;
    }

    private void BlockStateHandler()
    {
        float currentStemina = UIWhiteBox.GetGauge();
        if(currentStemina < 25f)
        {
            EndBlock();
        }
    }

    public void BlockHit()
    {
        animator.SetTrigger("BlockHit");
    }
}
