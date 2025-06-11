using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField] private PlayerStatus status;

    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack_Golem") || other.gameObject.CompareTag("Attack_Mushroom") || other.CompareTag("Attack_Bomber"))
        {
            MonsterAttackDetection enemyAttack = other.GetComponent<MonsterAttackDetection>();
            if (status == null)
            {
                Debug.Log("¾ø³ë");
            }
            status.TakeDamage(enemyAttack.monsterAttackPower);

            animator.applyRootMotion = true;
            if(status.playerHP <= 0)
            {
                animator.SetTrigger("Die");
                status.Die();
            }
            else
            {
                //animator.SetTrigger("Hit");
            }
        }

        if (other.CompareTag("MinimapTrigger"))
        {
            UIWhiteBox.DisableMinimapFog(other);
            //Destroy(other.gameObject);
        }
    }
}
