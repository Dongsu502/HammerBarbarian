using UnityEngine;
using Cinemachine;
using System.Collections;

public class AimCameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera aimCam;

    public bool isAiming = false;

    private void Awake()
    {
        // Wrap과 범위는 반드시 명시적으로 설정
       
    }


    public void SetAimCamera()
    {
        aimCam.Priority = 11;
        freeLookCam.Priority = 10;
        isAiming = true;

        UIWhiteBox.MainUIWB.Crosshair_SetActive(true);

        StartCoroutine(SyncPOVFromFreeLook());
    }

    public void SetFreeLookCamera()
    {
        aimCam.Priority = 10;
        freeLookCam.Priority = 11;
        isAiming = false;

        UIWhiteBox.MainUIWB.Crosshair_SetActive(false);

        StartCoroutine(SyncFreeLookFromPOV());
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

    private IEnumerator SyncFreeLookFromPOV()
    {
        var brain = Camera.main.GetComponent<CinemachineBrain>();

        yield return new WaitUntil(() =>
            brain != null &&
            brain.IsBlending == false &&
            brain.ActiveVirtualCamera?.VirtualCameraGameObject == freeLookCam.gameObject);

        yield return new WaitForEndOfFrame(); // Live 이후 1프레임 더 기다림

        var pov = aimCam.GetCinemachineComponent<CinemachinePOV>();
        if (pov == null) yield break;

        float yaw = pov.m_HorizontalAxis.Value;
        float pitch = pov.m_VerticalAxis.Value;

        float x = yaw / 360f;
        if (x < -0.5f) x += 1f;
        if (x > 0.5f) x -= 1f;

        float y = Mathf.InverseLerp(60f, -30f, pitch);

        freeLookCam.m_XAxis.Value = x;
        freeLookCam.m_YAxis.Value = y;

        Debug.Log($" 최종 적용 x:{x}, y:{y}");
    }

    //private void Update()
    //{
    //    if (isAiming)
    //    {
    //        var pov = aimCam.GetCinemachineComponent<CinemachinePOV>();
    //        if (pov == null) return;

    //        float yaw = pov.m_HorizontalAxis.Value;
    //        float pitch = pov.m_VerticalAxis.Value;

    //        float x = yaw / 360f;
    //        if (x < -0.5f) x += 1f;
    //        if (x > 0.5f) x -= 1f;

    //        float y = Mathf.InverseLerp(60f, -30f, pitch);

    //        freeLookCam.m_XAxis.Value = x;
    //        freeLookCam.m_YAxis.Value = y;
    //    }
    //}
}
