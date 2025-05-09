using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game;
using Unity.VisualScripting;

public class PlayerUseItem : MonoBehaviour, IItemUseable, IAttackable
{
    [Header("References")]
    [SerializeField] private AimCameraSwitcher aimCameraSwitcher;
    [SerializeField] private HammerThrowController hammerThrowController;

    public void UseItemByType(WeaponType weaponType)
    {
        Debug.Log(weaponType.ToString());
        switch (weaponType)
        {
            case WeaponType.Hammer:
                break;
            case WeaponType.Rope:
                aimCameraSwitcher.SetAimCamera();
                break;

        }
    }

    public void EndUseItemByType(WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.Hammer:
                break;
            case WeaponType.Rope:
                aimCameraSwitcher.SetFreeLookCamera();
                break;
        }
    }

    public void AttackByType(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType .Hammer:
                break ;
            case WeaponType .Rope:
                hammerThrowController.Throw();
                break ;
        }
    }
}
