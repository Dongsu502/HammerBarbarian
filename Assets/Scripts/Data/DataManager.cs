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
}
