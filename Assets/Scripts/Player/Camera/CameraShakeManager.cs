using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using Game;

public class CameraShakeManager : MonoBehaviour
{
    [Header("Cinemachine Impulse Source")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Tooltip("모든 흔들림 프로필 목록")]
    [SerializeField] private List<CameraShakeProfile> shakeProfiles;

    [Header("Shake 제한 설정")]
    [SerializeField] private float shakeCooldown = 0.2f; // 최소 간격

    private float lastShakeTime = -999f;

    public static CameraShakeManager instance;

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("테스트 쉐이크")]
    public void TestShake()
    {
        if (impulseSource != null)
        {
            PlayerHitWhiteBox.WhiteBox.Shake("Golem", AttackType.Light);
        }
        else
        {
            Debug.LogWarning("ImpulseSource가 할당되지 않았습니다!", this);
        }
    }

    public void Shake(string monsterType, AttackType attackType)
    {
        // 쿨타임 제한
        if (Time.time - lastShakeTime < shakeCooldown) return;
        lastShakeTime = Time.time;

        // 프로필 찾기
        var profile = shakeProfiles.FirstOrDefault(p => p.mosterType == monsterType && p.attackType == attackType);

        if (profile == null)
        {
            Debug.LogWarning($"[CameraShake] 프로필 없음: {monsterType}/{attackType}");
            return;
        }

        if (profile.impulseDefinition != null)
        {
            Debug.Log("쉐이키");
            impulseSource.m_ImpulseDefinition = profile.impulseDefinition;
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.Log("쉐이킹!");
            impulseSource.GenerateImpulse(profile.amplitude);
        }
    }
}
