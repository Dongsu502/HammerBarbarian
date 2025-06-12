using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextTyping : MonoBehaviour
{
    /// <summary>
    /// 타이핑효과
    /// </summary>
    /// <param name="_newText">텍스트변수</param>
    /// <param name="_newString">텍스트변수에 들어갈 값</param>
    /// <param name="typingSpeed">타이핑 속도</param>
    /// <returns></returns>
    public IEnumerator Typing(Text _newText, string _newString, float typingSpeed)
    {
        _newText.text = string.Empty;

        StringBuilder stringBuilder = new StringBuilder();
        
        for(int i = 0; i < _newString.Length; i++)
        {
            stringBuilder.Append(_newString[i]);
            _newText.text = stringBuilder.ToString();

            //속도
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
