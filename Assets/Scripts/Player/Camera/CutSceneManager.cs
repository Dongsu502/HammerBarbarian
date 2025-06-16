using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class CutsceneWhiteBox
{
    public static CutSceneManager WhiteBox;

    public static void SetWhiteBox(CutSceneManager manager)
    {
        WhiteBox = manager;
    }
}

public class CutSceneManager : MonoBehaviour
{
    public CinemachineVirtualCamera cutsceneCam;
    public CinemachineFreeLook freeLookCam;

    [SerializeField] private PlayableDirector director;

    private void Awake()
    {
        CutsceneWhiteBox.SetWhiteBox(this);
    }

    public void StartCutscene()
    {
        cutsceneCam.Priority = 100;
        freeLookCam.Priority = 10;
    }

    public void EndCutscene()
    {
        cutsceneCam.Priority = 10;
        freeLookCam.Priority = 20;
    }

    public void PlayerCutscene()
    {
        director.Play();
    }
}
