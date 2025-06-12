using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextTyping : MonoBehaviour
{
    public bool IsTyping => isTyping;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private Text currentText;
    private string fullText;

    public void StartTyping(Text uiText, string newText, float speed)
    {
        //이미 실행중이라면 멈추고 새로 시작
        StopTyping();

        fullText = newText;
        currentText = uiText;

        typingCoroutine = StartCoroutine(TypingCoroutine(speed));
    }

    public void SkipTyping()
    {
        if (!isTyping) return;

        StopTyping();
        currentText.text = fullText;
    }

    private IEnumerator TypingCoroutine(float speed)
    {
        isTyping = true;
        currentText.text = "";

        StringBuilder sb = new StringBuilder();

        foreach(char c in fullText)
        {
            sb.Append(c);
            currentText.text = sb.ToString();
            
            yield return new WaitForSecondsRealtime(speed);
        }

        isTyping = false;
    }

    private void StopTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping= false;
    }

    /// <summary>
    /// 타이핑효과
    /// </summary>
    /// <param name="_newText">텍스트변수</param>
    /// <param name="_newString">텍스트변수에 들어갈 값</param>
    /// <param name="typingSpeed">타이핑 속도</param>
    /// <returns></returns>
    //public IEnumerator Typing(Text _newText, string _newString, float typingSpeed)
    //{
    //    isTyping = true;
    //    yield return null;

    //    _newText.text = string.Empty;

    //    StringBuilder stringBuilder = new StringBuilder();
        
    //    for(int i = 0; i < _newString.Length; i++)
    //    {
    //        stringBuilder.Append(_newString[i]);
    //        _newText.text = stringBuilder.ToString();

    //        //속도
    //        yield return new WaitForSecondsRealtime(typingSpeed);
    //    }
    //    yield return null;
    //    isTyping = false;
    //}
}
