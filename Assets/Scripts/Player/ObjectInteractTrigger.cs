using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractTrigger : MonoBehaviour, ITextTriggerCondition
{
    private TextTriggerController controller;
    private int index;

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") &&controller.sequenceIndex ==3)
        {
            UIWhiteBox.Spawn_InterectionUI("F", "근육 만져보기");
            if (Input.GetKeyDown(KeyCode.F))
            {
                controller.NotifyConditionMet(index);
                UIWhiteBox.SetActiveInterectionPanel(false);
            }
        }
    }
}
