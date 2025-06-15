using Game;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lightAttackClip;
    [SerializeField] private AudioClip heavyAttackClip;
    [SerializeField] private AudioClip strongAttackClip;
    [SerializeField] private AudioClip windmillClip;
    [SerializeField] private AudioClip attackHitClip;
    [SerializeField] private AudioClip hitClip;

    public void PlayAttackSound(AttackType type)
    {
        switch (type)
        {
            case AttackType.Light:
                audioSource.PlayOneShot(lightAttackClip);
                break;
            case AttackType.Heavy:
                audioSource.PlayOneShot(heavyAttackClip);
                break;
            case AttackType.Skill:
                audioSource.PlayOneShot(strongAttackClip);
                break;
            case AttackType.WhirlWind:
                audioSource.PlayOneShot(windmillClip);
                break;
        }
    }

    public void AttackHitSound()
    {
        audioSource.PlayOneShot(attackHitClip);
    }

    public void HitSound()
    {
        audioSource.PlayOneShot(hitClip);
    }
}
