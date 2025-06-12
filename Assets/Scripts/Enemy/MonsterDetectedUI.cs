using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetectedUI : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Space]
    [Header("감지 리소스이미지")]
    [SerializeField] private Sprite[] detectedSprite;

    [Space]
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float rotateSpeed;

    private Image detectedImage;

    private void Awake()
    {
        detectedImage = GetComponent<Image>();
    }

    private void Update()
    {
        LookTarget(target);
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

    /// <summary>
    /// 감지 확인하여 이미지 변경
    /// </summary>
    /// <param name="isDetection">감지 여부(true: 감지 / false: 감지 안됨)</param>
    public void DetectionCheck(bool isDetection)
    {
        //이미지 변경
        ChangeImage(isDetection);
        //페이드인아웃
        FadeInOut(isDetection);
    }

    /// <summary>
    /// 이미지 변경
    /// </summary>
    /// <param name="isDetection">감지 여부</param>
    private void ChangeImage(bool isDetection)
    {
        if(isDetection)
        {
            //느낌표, 빨간색으로 변경
            detectedImage.sprite = detectedSprite[1];
            detectedImage.color = Color.red;
        }
        else
        {
            //물음표로 변경
            detectedImage.sprite = detectedSprite[0];
        }
    }

    /// <summary>
    /// 페이드인아웃
    /// </summary>
    /// <param name="isIn">감지 여부</param>
    private void FadeInOut(bool isDetection)
    {
        if (isDetection)
        {
            //일정 시간 지난 후 페이드아웃
            StartCoroutine(FadeOut(fadeOutTime));
        }
        else
        {
            //색깔 활성화
            detectedImage.color = Color.white;
        }
    }

    private IEnumerator FadeOut(float fadeOutTime)
    {
        yield return new WaitForSeconds(fadeOutTime);

        Color color = detectedImage.color;

        for (int i = 0; i < 100; i++)
        {
            color.a = color.a - 0.01f;
            detectedImage.color = color;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
