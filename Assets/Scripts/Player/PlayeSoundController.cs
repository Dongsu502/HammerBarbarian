using Game;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
   

    public void PlayAttackSound(AttackType type)
    {
        switch (type)
        {
            case AttackType.Light:
                SoundManager.instance.PlayPlayerSFX("Swing07");
                break;
            case AttackType.Heavy:
                SoundManager.instance.PlayPlayerSFX("Swing02");
                break;
            case AttackType.Skill:
                SoundManager.instance.PlayPlayerSFX("Swing04");
                break;
            case AttackType.WhirlWind:
                SoundManager.instance.PlayPlayerSFX("WhirlWind01");
                break;
        }
    }

    public void AttackHitSound()
    {
        SoundManager.instance.PlayPlayerSFX("Attack08");
    }

    public void HitSound()
    {
        SoundManager.instance.PlayPlayerSFX("Damage01");
    }
}
