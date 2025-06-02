using UnityEngine;
using System.Collections;

public class DebrisController : MonoBehaviour
{
    public void IgnoreWeaponCollision(Collider weaponCollider)
    {
        StartCoroutine(DoIgnore(weaponCollider));
    }

    private IEnumerator DoIgnore(Collider weaponCollider)
    {
        yield return null;

        Collider[] debrisCols = GetComponentsInChildren<Collider>();
        foreach (var col in debrisCols)
        {
            Physics.IgnoreCollision(col, weaponCollider);
        }
    }
}
