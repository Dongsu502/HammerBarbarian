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

            gaugeImage.fillAmount = time / 10f;

            float gaugeValue = time * 10;
            gaugeText.text = Mathf.RoundToInt(gaugeValue).ToString() + "%";
            Debug.LogWarning("·ÎµùÁß..");

            if(time > 0.5f)
            {
                SetActive_Tip(true);
            }

            if(time > 10)
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
