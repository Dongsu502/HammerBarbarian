using UnityEngine;

public class AllEnemiesDefeatedCondition : MonoBehaviour, ITextTriggerCondition
{
    [SerializeField] private ArenaController arenaController;

    private TextTriggerController controller;
    private int index;
    private bool alreadyTriggered = false;

    public void Init(TextTriggerController controller, int conditionIndex)
    {
        this.controller = controller;
        this.index = conditionIndex;
    }

    private void Start()
    {
        if (arenaController == null)
            Debug.LogWarning("arenaController가 연결되지 않았습니다. 자동으로 연결하거나 인스펙터에서 할당하세요.", this);
    }

    private void Update()
    {
        if (alreadyTriggered) return;

        if (!arenaController.HasEnemyEverSpawned) return;

        if (arenaController.CurrentEnemyCount == 0)
        {
            alreadyTriggered = true;
            controller.NotifyConditionMet(index);
        }
    }

}
