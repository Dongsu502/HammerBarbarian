using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractTrigger : MonoBehaviour, ITextTriggerCondition
{
    private TextTriggerController controller;
    private int index;

    [SerializeField] private ArenaController arenaController;

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void OnTriggerStay(Collider other)
    {
        if (arenaController.CurrentEnemyCount != 0)
            return;

        if (other.CompareTag("Player") &&controller.sequenceIndex ==3)
        {
            UIWhiteBox.Spawn_InterectionUI("F", "근육 만져보기");
            if (Input.GetKeyDown(KeyCode.F))
            {
                controller.NotifyConditionMet(index);
                UIWhiteBox.SetActiveInterectionPanel(false);
                StartCoroutine(playerDiretor());
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

    private IEnumerator playerDiretor()
    {
        yield return new WaitForSeconds(1f);
        CutsceneWhiteBox.WhiteBox.StartCutscene();
        CutsceneWhiteBox.WhiteBox.PlayerCutscene();

        new WaitForSeconds(5f);

        CutsceneWhiteBox.WhiteBox.EndCutscene();

    }
}
