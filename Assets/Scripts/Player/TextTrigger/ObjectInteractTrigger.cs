using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractTrigger : MonoBehaviour, ITextTriggerCondition
{
    private TextTriggerController controller;
    private int index;

    [SerializeField] private ArenaController arenaController;

    private bool isActivated = false;
    private bool isInteracted = false;

    private void Update()
    {
        if (isInteracted && UIWhiteBox.GetScriptIsEnd())
        {
            UIWhiteBox.TutorialUICurrentState = TutorialUIState.FIRSTITEM;
            UIWhiteBox.SetActive_KeyDescriptionPanel(true);
            isInteracted = false;
        }
    }

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void OnTriggerStay(Collider other)
    {
        if (arenaController.CurrentEnemyCount != 0 || isActivated)
            return;

        if (other.CompareTag("Player") && controller.sequenceIndex == 3)
        {
            UIWhiteBox.Spawn_InterectionUI("F", "근육 만져보기");
            if (Input.GetKeyDown(KeyCode.F))
            {
                controller.NotifyConditionMet(index);
                UIWhiteBox.SetActiveInterectionPanel(false);
                UIWhiteBox.GetItem();
                isInteracted=true;
                isActivated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIWhiteBox.SetActiveInterectionPanel(false);
        }
    }

  
}
