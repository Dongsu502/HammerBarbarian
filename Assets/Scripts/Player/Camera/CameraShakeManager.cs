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

    public static CameraShakeManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Shake(string monsterType,AttackType attackType)
    {
        var profile = shakeProfiles.FirstOrDefault(p => p.mosterType == monsterType && p.attackType == attackType);

        if (profile == null)
        {
            Debug.LogWarning($"[CameraShake] 프로필 없음:{monsterType}/{attackType}");
        }

        if(profile.impulseDefinition != null)
        {
            impulseSource.m_ImpulseDefinition = profile.impulseDefinition;
            impulseSource.GenerateImpulse();
        }
        else
        {
            impulseSource.GenerateImpulse(profile.amplitude);
        }
    }
}
