using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthUI : MonoBehaviour
{
    private GameObject hpBar;

    private float currentHP;

    private const float MIN_HP = 0f;
    private const float MAX_HP = 100f;

    private void Awake()
    {
        HPBar_Initialize();
    }

    private void HPBar_Initialize()
    {
        hpBar = transform.GetChild(0).gameObject;
        HPBar_SetActive(false);

        currentHP = MAX_HP;
    }

    private void HPBar_SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void TakeDamageUI(float damage)
    {
        if (currentHP <= MIN_HP) return;

        HPBar_SetActive(true);

        currentHP -= damage;

        Vector3 newScale = new(currentHP/100, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
        hpBar.transform.localScale = newScale;

        Debug.Log($"currentHP: {currentHP}");
    }
}
