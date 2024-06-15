using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Database : MonoBehaviour  // 서버에 데이터를 저장하기 위한 스크립트
{
    private string path;
    private string id;
    private string pw;
    
    private void Start()
    {
        path = Application.persistentDataPath + "/";
    }

    public void LoadData() // when Login
    {
        id = GameManager.instance.id;
        pw = GameManager.instance.pw;
        
        //string[] json = new string[3];
        //파일 갈아끼우기
        StartCoroutine(DataLoadFromDatabase(id, pw));
    }
    public void DataInput() //when game exit
    {
        id = GameManager.instance.id;
        pw = GameManager.instance.pw;
        
        string[] json = new string[3];
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(path + $"{i}"))
            {
                Debug.Log($"{i} Load Success");
                json[i] = File.ReadAllText(path + i.ToString());
            }
            else
            { 
                json[i] = null;
            }
        }
        StartCoroutine(JsonUpdate(json));
    }

    public void CreateAccount(string s1, string s2)
    {
        StartCoroutine(CreateAccountToDatabase(s1, s2));
    }

    IEnumerator JsonUpdate(string[] json)
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
    
    IEnumerator CreateAccountToDatabase(string id, string pw)
    {

        // GET 방식
        string url = $"http://113.198.229.155:4007/create?userID={id}&userPW={pw}";
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
    
    IEnumerator DataLoadFromDatabase(string id, string pw)
    {

        // GET 방식
        //string url = $"http://113.198.229.155:4007/login?userID={id}&userPW={pw}";
        string url = $"http://113.198.229.155:4007/login?userID={id}"+$"&userPW={pw}";
        Debug.Log(url);

        // UnityWebRequest에 내장되있는 GET 메소드를 사용한다.
        UnityWebRequest www = UnityWebRequest.Get(url);
        //

        yield return www.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (www.error == null)  // 에러가 나지 않으면 동작.
        {
            Debug.Log(www.downloadHandler.text);
            
            
            string input = www.downloadHandler.text;
            if (input == "\"NO ACCOUNT\"")
            {
                Debug.Log("계정업슈");
            }
            else if(input=="\"PASSWORD INCORRECT\"")
            {
                Debug.Log("비밀번호 틀렸슈");
            }
            else
            {
                //input = input.Trim(new char[] { '[', ']' });
                input = input.Replace("[[\"", "");
                input = input.Replace("\"]]", "");
                
                string[] splitArray = input.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                for (int i = 0; i < 3; i++)
                {
                    Debug.Log(splitArray[i]);
                }
                
                for (int i = 0; i < 3; i++)
                {
                    if (splitArray[i].Contains("null"))
                    {
                        File.Delete(path+i.ToString());
                    }
                    else
                    {
                        splitArray[i] = splitArray[i].Replace("\\", "");
                        File.WriteAllText(path + i.ToString(), splitArray[i]);
                    }
                    
                }
                Debug.Log("덮어쓰기 완료");
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}