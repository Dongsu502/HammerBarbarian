using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCam;

    public void LockCameraRotation()
    {
        freeLookCam.m_XAxis.m_MaxSpeed = 0f;
        freeLookCam.m_YAxis.m_MaxSpeed = 0f;
    }

    public void UnlockCameraRotation(float xSpeed = 250f, float ySpeed = 2f)
    {
        freeLookCam.m_XAxis.m_MaxSpeed = xSpeed;
        freeLookCam.m_YAxis.m_MaxSpeed = ySpeed;
    }
}
