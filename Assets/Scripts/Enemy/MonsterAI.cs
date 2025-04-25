using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    private BTNode root;
    private IMonster monster;

    private void Start()
    {
        monster = GetComponent<IMonster>();

        root = new Selector(new List<BTNode> {
            new Sequence(new List<BTNode> {
                new IsDeadNode(monster), new DeadAction(monster) }),
            new Sequence(new List<BTNode> {
                new IsHitNode(monster), new HitAction(monster) }),
            new Sequence(new List<BTNode> {
                new IsAttackNode(monster), new AttackAction(monster) }),
            new Sequence(new List<BTNode> {
                new IsTargetDetectedNode(monster), new MoveAction(monster) }),
            new IdleAction(monster)
            });
    }

    private void Update()
    {
        root.Evaluate();
    }
}
