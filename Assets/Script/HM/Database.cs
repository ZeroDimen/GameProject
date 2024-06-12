using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour  // 서버에 데이터를 저장하기 위한 스크립트
{
    private string path;
    private string id;
    private string pw;
    public void DataInput()
    {
        //여기서 로컬에 있는 파일 읽어서 서버로 보내야함
        path = Application.persistentDataPath + "/";
        id = GameManager.instance.id;
        pw = GameManager.instance.pw;
        string[] json = new string[3];
        for (int i = 0; i < 3; i++)
        {
            //     if (File.Exists(Data_Manager.instance.path + $"{i}"))
            //     {
            //         Debug.Log($"{i} Load Success");
            //         json[i] = File.ReadAllText(path + i.ToString());
            //     }
            //     else
            //     {
            //         json[i] = null;
            //     }
            // }
        }
        StartCoroutine(UnityWebRequestGetTest(json));
    }

    IEnumerator UnityWebRequestGetTest(string[] json)
    {

        // GET 방식
        string url = $"http://113.198.229.155:4007/update?userID={id}&userPW={pw}&json1='{json[0]}'&json2='{json[1]}'&json3='{json[2]}'";
        Debug.Log(url);

        // UnityWebRequest에 내장되있는 GET 메소드를 사용한다.
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (www.error == null)  // 에러가 나지 않으면 동작.
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}