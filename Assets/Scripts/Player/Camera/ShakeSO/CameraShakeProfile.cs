using UnityEngine;
using Cinemachine;
using Game;

[CreateAssetMenu(menuName = "Camera/ShakeProfileByHit")]
public class CameraShakeProfile : ScriptableObject
{
    public string mosterType;
    public AttackType attackType;

    public float amplitude = 1f;
    public float frequency = 1f;
    public CinemachineImpulseDefinition impulseDefinition;

}
