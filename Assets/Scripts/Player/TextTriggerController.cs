using UnityEngine;

public class TextTriggerController : MonoBehaviour
{
    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;

    [Header("조건 컴포넌트들을 MonoBehaviour 배열로 등록")]
    [SerializeField] private MonoBehaviour[] conditionComponents;

    private bool triggered = false;
    private bool[] conditionStates;

    [SerializeField] private int sequenceIndex;
    [SerializeField] private TextTriggerSequenceManager sequenceManager;

    private void Start()
    {
        conditionStates = new bool[conditionComponents.Length];

        for (int i = 0; i < conditionComponents.Length; i++)
        {
            if (conditionComponents[i] is ITextTriggerCondition condition)
                condition.Init(this, i);
        }
    }

    public void NotifyConditionMet(int index)
    {
        conditionStates[index] = true;

        foreach (bool complete in conditionStates)
        {
            if (!complete) return;
        }

        TriggerText();
    }

    public void TriggerText()
    {
        if (triggered || !sequenceManager.CanTrigger(sequenceIndex)) return;

        triggered = true;
        UIWhiteBox.StartScripting(minCount, maxCount);
        sequenceManager.Advance();
    }
}