using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBlockHit : MonoBehaviour
{
    private PlayerBlock playerBlock;

    private void Awake()
    {
        playerBlock = GetComponentInParent<PlayerBlock>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerBlock != null)
        {
            if(other.CompareTag("Attack_Mushroom"))
            {
                Debug.Log("¸·¾Ò´Ù!");
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Attack_Golem"))
            {
                
            }
        }
    }
}
