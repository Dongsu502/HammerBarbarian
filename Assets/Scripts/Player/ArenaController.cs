using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    private bool hasRegisteredEnemy = false;

    //private int currentEnemyCount;

    public int CurrentEnemyCount => enemies.Count;

    private bool addedEnemy = false;

    private void Start()
    {
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].SetActive(true);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            DeActiveWall();
        }
    }

    private void DeActiveWall()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        hasRegisteredEnemy = true;
    }

    public bool HasEnemyEverSpawned => hasRegisteredEnemy;

    [ContextMenu("모든 적 삭제")]
    public void TestRemoveAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        
        DeActiveWall();
    }
}
