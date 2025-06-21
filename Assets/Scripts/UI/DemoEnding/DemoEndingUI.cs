using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEndingUI : MonoBehaviour
{
    [SerializeField] private GameObject backImage;
    [SerializeField] private GameObject logoImage;
    [SerializeField] private GameObject demoText;
    [SerializeField] private GameObject goTitleButton;

#if UNITY_EDITOR
    [ContextMenu("엔딩UI호출")]
    public void Ending()
    {
        UIWhiteBox.OnEnableEndingUI();
    }
#endif

    private void Awake()
    {
        UIWhiteBox.SetDemoEndingUIWB(this);
    }

    private void Start()
    {
        if (backImage.activeSelf) backImage.SetActive(false);
        if (logoImage.activeSelf) logoImage.SetActive(false);
        if (demoText.activeSelf) demoText.SetActive(false);
        if (goTitleButton.activeSelf) goTitleButton.SetActive(false);
    }
    public void OnEnable_backImage()
    {
        backImage.SetActive(true);

        UIWhiteBox.CursorLock(true);
        WorldWhiteBox.WhiteBox.PauseGame();

        StartCoroutine(OnEnable_EndingUIs());
    }

    private IEnumerator OnEnable_EndingUIs()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        OnEnable_logoImage();

        yield return new WaitForSecondsRealtime(1f);

        OnEnable_demoText();

        yield return new WaitForSecondsRealtime(1f);

        OnEnable_goTitleButton();
    }

    private void OnEnable_logoImage()
    {
        logoImage.SetActive(true);
    }
    private void OnEnable_demoText()
    {
        demoText.SetActive(true);
    }
    private void OnEnable_goTitleButton()
    {
        goTitleButton.SetActive(true);
    }

    #region ButtonEvent

    public void Click_GoTitleButton()
    {
        WorldWhiteBox.WhiteBox.ResumeGame();
        SceneManager.LoadScene("Title");
    }

    #endregion
}
