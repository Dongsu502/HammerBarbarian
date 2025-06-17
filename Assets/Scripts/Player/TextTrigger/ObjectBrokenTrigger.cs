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
        controller.NotifyConditionMet(index);
        isInterated = true;
    }

    public void Play()
    {
        StartCoroutine(PlayerDirectorCoroutine());

    }

    private IEnumerator PlayerDirectorCoroutine()
    {
        // 컷씬 시작
        CutsceneWhiteBox.WhiteBox.StartCutscene();
        CutsceneWhiteBox.WhiteBox.PlayerCutscene();

        // 4초 대기
        yield return new WaitForSeconds(4f);

        // 돌 삭제 트리거
        stoneAnimator.SetTrigger("StoneDelet");

        // 다시 4초 대기
        yield return new WaitForSeconds(4f);

        // 컷씬 종료
        CutsceneWhiteBox.WhiteBox.EndCutscene();
    }


}
