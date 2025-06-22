using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBrokenTrigger : MonoBehaviour, ITextTriggerCondition
{
    [SerializeField] private BrokenObject brokenObject;

    private TextTriggerController controller;
    private int index;

    private bool isInterated = false;

    [SerializeField] private Animator stoneAnimator;

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void Update()
    {
        if(isInterated && UIWhiteBox.GetScriptIsEnd())
        {
            Play();
            isInterated = false;
        }
    }

    public void BrokenTrigger()
    {
        Debug.LogWarning("ºÎ¼­Áü!!");
        controller.NotifyConditionMet(index);
        isInterated = true;
    }

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

        yield return new WaitForSeconds(2f);

        UIWhiteBox.OnEnableEndingUI();
    }


}
