using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldWhiteBox
{
    public static WorldManager WhiteBox { get; private set; }

    public static void SetWhiteBox(WorldManager manager)
    {
        WhiteBox = manager;
    }
}
public class WorldManager : MonoBehaviour
{
    private void Awake()
    {
        WorldWhiteBox.SetWhiteBox(this);
    }

    public void PauseGame()
    {
        Debug.Log("¸ØÃç!!");
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Debug.Log("¿òÁ÷¿©!!");

        Time.timeScale = 1f;
    }

    
}
