using UnityEngine;
using Cinemachine;

public class AimCameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera aimCam;

    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) // 우클릭 시 조준
        {
            aimCam.Priority = 20;
            freeLookCam.Priority = 10;
        }
        else
        {
            aimCam.Priority = 10;
            freeLookCam.Priority = 20;
        }
    }
}
