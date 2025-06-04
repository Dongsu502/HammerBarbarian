using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    GOLEM,
    MUSHROOM
}

public class EnemySpawner : MonoBehaviour
{
    public MonsterType currentMonsterType { get; private set; }
}
