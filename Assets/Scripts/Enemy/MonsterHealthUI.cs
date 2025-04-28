using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject HPBar;
    [SerializeField] private Image currentHPImage;

    [SerializeField] private Transform lookTarget;
    [SerializeField] private float rotateSpeed;

    private float currentHP;

    private const float MIN_HP = 0f;
    private const float MAX_HP = 100f;

    private void Awake()
    {
        HPBar_Initialize();
    }

    private void Update()
    {
        LookTarget(lookTarget);
    }

    private void HPBar_Initialize()
    {
        HPBar_SetActive(false);

        currentHP = MAX_HP;

        //lookTarget = FindObjectOfType<PlayerMove>().transform;
    }

    private void HPBar_SetActive(bool isActive)
    {
        currentHPImage.gameObject.SetActive(isActive);
        HPBar.SetActive(isActive);
    }

    public void TakeDamageUI(float damage)
    {
        if (currentHP <= MIN_HP) return;

        HPBar_SetActive(true);

        currentHP -= damage;

        currentHPImage.fillAmount = currentHP / 100;

        Debug.Log($"currentHP: {currentHP}");
    }

    private void LookTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f; // 수평 회전만
        if (dir.sqrMagnitude > 0f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
