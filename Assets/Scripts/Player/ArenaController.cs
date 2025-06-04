using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemies.Count <= 0)
        {
            DeActiveWall();
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
    }
}
