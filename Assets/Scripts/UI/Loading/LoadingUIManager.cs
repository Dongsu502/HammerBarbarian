using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUIManager : MonoBehaviour
{
    [Header("LoadingBar")]
    [Tooltip("GaugeImage")]
    [SerializeField] private Image gaugeImage;
    [Tooltip("GaugeText")]
    [SerializeField] private Text gaugeText;
    [Tooltip("로딩 시간")]
    [SerializeField] private float maxLoadingTime;
    private float time;

    [Header("TipPanel")]
    [SerializeField] private GameObject tipPanel;

    private string sceneName;

    private void OnEnable()
    {
        sceneName = UIWhiteBox.SceneName;
        Debug.LogWarning(sceneName);

        SetActive_Tip(false);
    }

    private void Start()
    {
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            time += Time.deltaTime;

            gaugeImage.fillAmount = time / maxLoadingTime;

            float gaugeValue = time * 20;
            gaugeText.text = Mathf.RoundToInt(gaugeValue).ToString() + "%";

            if(time > 0.5f)
            {
                SetActive_Tip(true);
            }

            if(time > maxLoadingTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void SetActive_Tip(bool isActive)
    {
        tipPanel.SetActive(isActive);
    }
}
