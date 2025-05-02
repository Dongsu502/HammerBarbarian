using UnityEngine;
using Cinemachine;
using System.Collections;

public class AimCameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera aimCam;

    public bool isAiming = false;

   // private bool applyAimSync = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            aimCam.Priority = 20;
            freeLookCam.Priority = 10;
            isAiming = true;

            UIWhiteBox.MainUIWB.Crosshair_SetActive(true);

            StartCoroutine(SyncPOVFromFreeLook());
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            aimCam.Priority = 10;
            freeLookCam.Priority = 20;
            isAiming = false;

            UIWhiteBox.MainUIWB.Crosshair_SetActive(false);
        }
    }

    IEnumerator SyncPOVFromFreeLook()
    {
        yield return new WaitForEndOfFrame();

        var pov = aimCam.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            //  FreeLook의 실제 바라보는 방향
            Quaternion lookRotation = freeLookCam.State.RawOrientation;
            Vector3 forward = lookRotation * Vector3.forward;

            //  수평 각도 추출
            float yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;

            //  수직 각도 추출
            float pitch = -Mathf.Asin(forward.y) * Mathf.Rad2Deg;

            //  POV 카메라에 전달
            pov.m_HorizontalAxis.Value = yaw;
            pov.m_VerticalAxis.Value = Mathf.Clamp(pitch, pov.m_VerticalAxis.m_MinValue, pov.m_VerticalAxis.m_MaxValue);
        }
    }



}
