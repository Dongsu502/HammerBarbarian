using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFog : MonoBehaviour
{
    [Header("미니맵안개 이미지들")]
    public List<GameObject> fogObjectList;

    private Dictionary<string, GameObject> fogObjectDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        UIWhiteBox.SetMinimapFogWB(this);

        gameObject.transform.position = new Vector3(59f, -26.6f, 123.1f);
        gameObject.transform.localScale = new Vector3(33.36097f, 6.808361f, 33.36097f);

        InitializeFog();
    }

    /// <summary>
    /// 미니맵 안개이미지들 초기화
    /// </summary>
    private void InitializeFog()
    {
        foreach(GameObject obj in fogObjectList)
        {
            if(obj != null && !fogObjectDict.ContainsKey(obj.name))
            {
                fogObjectDict.Add(obj.name, obj);
                Debug.Log(obj.name);
            }
        }
    }

    /// <summary>
    /// 미니맵안개 이미지 비활성화
    /// </summary>
    /// <param name="triggerObj">비활성화 할 이미지 콜라이더</param>
    public void DisableFog(Collider triggerObj)
    {
        string targetName = triggerObj.gameObject.name;

        if (fogObjectDict.TryGetValue(targetName, out GameObject targetObj))
        {
            if(targetObj != null && targetObj.activeSelf)
            {
                targetObj.SetActive(false);
                Debug.Log($"{targetName} 비활성화");
            }
        }
        else
        {
            Debug.Log($"{targetName} 는 등록된 오브젝트가 아닙니다.");
        }
    }
}
