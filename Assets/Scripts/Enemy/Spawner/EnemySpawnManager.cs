using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("아레나 컨트롤러 스크립트")]
    [SerializeField] private ArenaController arenaController;

    [Header("스폰 위치 오브젝트들")]
    [SerializeField] private GameObject[] spawnObjects;

    [Header("몬스터 프리팹")]
    [SerializeField] private GameObject golemPrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject bomberPrefab;
    private GameObject spawnMonster;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SpawnEnemys();
            Debug.Log("몬스터 스폰 트리거 발동");

            Destroy(this,2f);
        }
    }

    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    private void SpawnEnemys()
    {
        for (int i = 0; i < spawnObjects.Length; i++)
        {
            Debug.Log("몬스터 스폰!");
            MonsterType monsterType = spawnObjects[i].GetComponent<EnemySpawner>().CurrentMonsterType;

            if (monsterType == MonsterType.GOLEM)
            {
                spawnMonster = Instantiate(golemPrefab, spawnObjects[i].transform.position, spawnObjects[i].transform.rotation);
            }
            else if (monsterType == MonsterType.MUSHROOM)
            {
                spawnMonster = Instantiate(mushroomPrefab, spawnObjects[i].transform.position, spawnObjects[i].transform.rotation);
            }
            else if (monsterType == MonsterType.BOMBER)
            {
                spawnMonster = Instantiate(bomberPrefab, spawnObjects[i].transform.position, spawnObjects[i].transform.rotation);
            }
            else
            {
                Debug.LogError("스포너의 몬스터 타입이 잘못 되었습니다.");
            }

            IArenaRegistrable arenaUnit = spawnMonster.GetComponent<IArenaRegistrable>();
            if (arenaUnit != null)
            {
                arenaUnit.Initialize(arenaController);
            }
            arenaController.AddEnemy(spawnMonster);
        }
    }

}
