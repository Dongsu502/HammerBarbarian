using UnityEngine;

public class WeaponAttacher : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerIK playerIK; // IK 제어 스크립트

    private Transform rightHandBone;
    private GameObject currentWeapon;

}
