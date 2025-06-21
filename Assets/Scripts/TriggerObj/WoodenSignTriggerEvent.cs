using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSignTriggerEvent : MonoBehaviour
{
    private bool isTrigger = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isTrigger = true;
            if(isTrigger)
            {
                UIWhiteBox.Spawn_InterectionUI("F", "표지판보기");
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                UIWhiteBox.TutorialUICurrentState = TutorialUIState.COMBO;
                UIWhiteBox. SetActive_KeyDescriptionPanel(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIWhiteBox.SetActiveInterectionPanel(false);
        isTrigger = false;
    }
}
