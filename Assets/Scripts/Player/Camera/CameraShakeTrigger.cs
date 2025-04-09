using UnityEngine;
using Cinemachine;

public class CameraShakeTrigger : MonoBehaviour
{
    [Header("Cinemachine Impulse Source")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Shake Strength")]
    [Range(0f, 5f)]
    [SerializeField] private float baseShakePower = 1f;

    /// <summary>
    /// 일반 타격용 쉐이크
    /// </summary>
    public void Shake()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(baseShakePower);
        }
    }

    /// <summary>
    /// 원하는 강도만큼 직접 지정해서 쉐이크
    /// </summary>
    public void Shake(float intensity)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }
}
