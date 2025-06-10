using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    GOLEM,
    MUSHROOM,
    BOMBER
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private MonsterType currentMonsterType;  // 인스펙터에서 보이고

    public MonsterType CurrentMonsterType => currentMonsterType; // 코드에서는 읽기 전용
}
