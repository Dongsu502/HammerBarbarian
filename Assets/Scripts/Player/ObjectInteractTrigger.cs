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

    [SerializeField]private Animator stoneAnimator;

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
            UIWhiteBox.Spawn_InterectionUI("F", "±ÙÀ° ¸¸Á®º¸±â");
            if (Input.GetKeyDown(KeyCode.F))
            {
                controller.NotifyConditionMet(index);
                UIWhiteBox.SetActiveInterectionPanel(false);
                isActivated = true;
                isInteracted = true;
            }
        }
    }

    private void Update()
    {
        if (isInteracted && UIWhiteBox.GetScriptIsEnd())
        {
            StartCoroutine(PlayerDirectorCoroutine());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIWhiteBox.SetActiveInterectionPanel(false);
        }
    }

    [ContextMenu("ÄÆ¾À Àç»ý")]
    public void Play()
    {
        StartCoroutine(PlayerDirectorCoroutine());

    }

    private IEnumerator PlayerDirectorCoroutine()
    {
        // ÄÆ¾À ½ÃÀÛ
        CutsceneWhiteBox.WhiteBox.StartCutscene();
        CutsceneWhiteBox.WhiteBox.PlayerCutscene();

        // 4ÃÊ ´ë±â
        yield return new WaitForSeconds(4f);

        // µ¹ »èÁ¦ Æ®¸®°Å
        stoneAnimator.SetTrigger("StoneDelet");

        // ´Ù½Ã 4ÃÊ ´ë±â
        yield return new WaitForSeconds(4f);

        // ÄÆ¾À Á¾·á
        CutsceneWhiteBox.WhiteBox.EndCutscene();
    }
}
