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

    private const float MIN_HP = 0f;
    private float max_HP = 100f;

    private float currentHP;

    private IMonster monster;

    private void Awake()
    {
        monster = GetComponentInParent<IMonster>();
        lookTarget = PlayerStatWhiteBox.WhiteBox.FreeLookCamera.transform;

        HPBar_Initialize();
    }

    private void OnEnable()
    {
        //플레이어 화이트박스에 참조된 프리룩 카메라위치 받아오기
        //lookTarget = 
    }

    private void Update()
    {
        LookTarget(lookTarget);
    }

    private void HPBar_Initialize()
    {
        HPBar_SetActive(false);

        max_HP = monster.HP;

        currentHP = max_HP;
    }

    public void HPBar_SetActive(bool isActive)
    {
        currentHPImage.gameObject.SetActive(isActive);
        HPBar.SetActive(isActive);
    }

    public void TakeDamageUI(float damage)
    {
        if (currentHP <= MIN_HP)
        {
            currentHP = MIN_HP;
            currentHPImage.fillAmount = 0;

            return;
        }

        HPBar_SetActive(true);

        currentHP -= damage;

        currentHPImage.fillAmount = currentHP / max_HP;

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
