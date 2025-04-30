using UnityEngine;
using Cinemachine;

public class CharacterLookRotator : MonoBehaviour
{
    public Transform characterRoot; // 캐릭터 루트 (회전시킬 대상)
    public CinemachineVirtualCamera aimCamera; // 조준용 카메라
    public float rotateSpeed = 10f;

    private CinemachinePOV pov;

    void Start()
    {
        if (aimCamera != null)
            pov = aimCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        // 에임 카메라가 현재 Live 상태일 때만 작동
        if (aimCamera != null && aimCamera.Priority == GetHighestPriority())
        {
            if (pov == null) return;

            float yaw = pov.m_HorizontalAxis.Value;
            Quaternion targetRotation = Quaternion.Euler(0, yaw, 0);
            characterRoot.rotation = Quaternion.Slerp(characterRoot.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private int GetHighestPriority()
    {
        // 현재 활성화된 가상 카메라의 Priority 확인
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            return brain.ActiveVirtualCamera.Priority;
        }
        return -1;
    }
}
