using UnityEngine;

public class TriggerEnterCondition : MonoBehaviour, ITextTriggerCondition
{
    private TextTriggerController controller;
    private int index;

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.NotifyConditionMet(index);
        }
    }
}