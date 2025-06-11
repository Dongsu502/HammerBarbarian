using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private TextToggelController textToggelController;

    private bool addedEnemy = false;

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

            if (this.gameObject.name == "ArenaController1" && addedEnemy)
            {
                textToggelController.ToggleText(1105, 1106);
                addedEnemy = false;
            }
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
        if (!addedEnemy)
        {
            addedEnemy = true;
        }
        enemies.Add(enemy);
    }
}
