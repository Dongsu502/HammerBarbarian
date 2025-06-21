using System;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    #region Singleton
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent<DataManager>();
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    #endregion

    //게임 데이터 파일 이름 설정
    public string currentDataFileName = "";
    public string GameDataFileName1 = "GameData1.json";
    public string GameDataFileName2 = "GameData2.json";
    public string GameDataFileName3 = "GameData3.json";

    //저장용 클래스 변수
    public Data1 data1 = new();
    public Data2 data2 = new();
    public Data3 data3 = new();

#if UNITY_EDITOR

    [ContextMenu("데이터 초기화1")]
    public void DeleteData1()
    {
        DeleteDataFile(0);
    }
    [ContextMenu("데이터 초기화2")]
    public void DeleteData2()
    {
        DeleteDataFile(1);
    }
    [ContextMenu("데이터 초기화3")]
    public void DeleteData3()
    {
        DeleteDataFile(2);
    }
    [ContextMenu("데이터 저장1")]
    public void SaveData1()
    {
        SaveGameData(data1, GameDataFileName1);
    }
    [ContextMenu("데이터 저장2")]
    public void SaveData2()
    {
        SaveGameData(data2, GameDataFileName2);
    }
    [ContextMenu("데이터 저장3")]
    public void SaveData3()
    {
        SaveGameData(data3, GameDataFileName3);
    }
    [ContextMenu("데이터 존재하는지 확인")]
    public void DataCheck()
    {
        Debug.Log($"데이터1: {NeedToCreateNewDataFile(0)}");
        Debug.Log($"데이터2: {NeedToCreateNewDataFile(1)}");
        Debug.Log($"데이터3: {NeedToCreateNewDataFile(2)}");
    }

#endif

    /// <summary>
    /// 데이터 삭제
    /// </summary>
    /// <param name="index">삭제할 데이터 번호</param>
    public void DeleteDataFile(int index)
    {
        string fileName = "";

        switch (index)
        {
            case 0: fileName = GameDataFileName1; break;
            case 1: fileName = GameDataFileName2; break;
            case 2: fileName = GameDataFileName3; break;
            default:
                Debug.LogError("잘못된 슬롯 인덱스");
                return;
        }

        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"{fileName} 삭제 완료");
        }
        else
        {
            Debug.LogWarning($"{fileName} 파일이 존재하지 않아 삭제할 수 없음");
        }

        Debug.Log("[Delete] 삭제 대상 파일 경로: " + filePath);
    }

    /// <summary>
    /// 게임 데이터 불러오기
    /// </summary>
    /// <typeparam name="T">불러올 데이터</typeparam>
    /// <param name="fileName">파일 이름(DataManager에 만들어진 이름 사용)</param>
    /// <returns>불러온 데이터</returns>
    public T LoadGameData<T>(string fileName) where T : new()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        //저장된 게임이 있다면
        if(File.Exists(filePath))
        {
            //저장된 파일 불러오고 Json을 클래스 형식으로 전환해서 할당
            string FromJsonData = File.ReadAllText(filePath);
            T loadedData = JsonUtility.FromJson<T>(FromJsonData);

            Debug.Log("불러오기 완료");
            return loadedData;
        }

        Debug.LogWarning($"파일 없음: {fileName}, 새 인스턴스 반환");
        return new T(); // 없으면 새 인스턴스 반환
    }

    /// <summary>
    /// 게임 데이터 저장
    /// </summary>
    /// <typeparam name="T">저장할 데이터</typeparam>
    /// <param name="data"></param>
    /// <param name="fileName">파일 이름(DataManager에 만들어진 이름 사용)</param>
    public void SaveGameData<T>(T data, string fileName)
    {
        //클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        //이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);

        Debug.Log("[Save] 파일 경로: " + filePath);

        //올바르게 저장되었는지 확인
        Debug.Log("저장 완료");
    }

    /// <summary>
    /// 현재 데이터파일의 Data 가져오는 메서드
    /// </summary>
    /// <returns>data1/2/3</returns>
    public BaseData GetCurrentData()
    {
        if(currentDataFileName == GameDataFileName1)
        {
            return data1;
        }
        else if(currentDataFileName == GameDataFileName2)
        {
            return data2;
        }
        else if( currentDataFileName == GameDataFileName3)
        {
            return data3;
        }
        else
        {
            Debug.LogError("알 수 없는 데이터 파일 이름입니다.");
            return null;
        }
    }

    /// <summary>
    /// 현재파일이름 설정
    /// </summary>
    /// <param name="index">설정할 데이터 번호</param>
    public void SetCurrentData(int index)
    {
        switch (index)
        {
            case 0:
                currentDataFileName = GameDataFileName1;
                break;

            case 1:
                currentDataFileName = GameDataFileName2;
                break;

            case 2:
                currentDataFileName = GameDataFileName3;
                break;
        }
    }

    /// <summary>
    /// 슬롯에 있는 데이터파일 경로를 가져오는 메서드
    /// </summary>
    /// <param name="slotNumber">슬롯 번호</param>
    /// <returns>데이터파일 경로</returns>
    private string GetDataFilePath(int slotNumber)
    {
        switch (slotNumber)
        {
            case 0:
                return Path.Combine(Application.persistentDataPath, GameDataFileName1);
            case 1:
                return Path.Combine(Application.persistentDataPath, GameDataFileName2);
            case 2:
                return Path.Combine(Application.persistentDataPath, GameDataFileName3);

            default: return null;
        }
    }

    /// <summary>
    /// 데이터 파일이 존재하는지 확인하는 메서드
    /// </summary>
    /// <param name="slotNumber">슬롯 번호</param>
    /// <returns> 데이터파일 존재 유무(존재: false) </returns>
    public bool NeedToCreateNewDataFile(int slotNumber)
    {
        string path = GetDataFilePath(slotNumber);

        Debug.Log("[Check] 존재 여부 확인 파일 경로: " + path);

        // 없으면 true (새로운 데이터 파일을 만들 수 있음)
        if (!File.Exists(path))
        {
            return true;
        }

        // 존재하면 false (더 이상 만들 수 없음)
        return false;
    }
}
