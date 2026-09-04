using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] private PlayerAnimStateChecker animChecker;

    [SerializeField]private Transform hitOrigin;

    [SerializeField] HitStopHandler hitStopHandler;

    public HashSet<MonsterHitBox> monsterHitBoxes = new HashSet<MonsterHitBox>();

    [SerializeField] private PlayerSoundController soundController;

    private void OnTriggerEnter(Collider other)
    {
        MonsterHitBox monsterHitBox = other.GetComponent<MonsterHitBox>();
        if (other.gameObject.CompareTag("EnemyHitBox") && !monsterHitBoxes.Contains(monsterHitBox))
        {
            monsterHitBoxes.Add(monsterHitBox);

            if (!animChecker.IsWhirlwindAnim())
            {
                hitStopHandler.HitStop(0.1f);
            }
            
            Vector3 hitPoint = other.ClosestPoint(hitOrigin.position);
            Vector3 hitNormal = (other.transform.position - hitOrigin.position).normalized;

            HitReceiver receiver = other.GetComponent<HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
            soundController.AttackHitSound();
        }
    }

   

}
