using UnityEngine;

public class TextTriggerSequenceManager : MonoBehaviour
{
    public int currentStep = 0; // 현재 대사 진행 단계

    public bool CanTrigger(int index)
    {
        return index == currentStep;
    }

    public void Advance()
    {
        currentStep++;
    }
}
