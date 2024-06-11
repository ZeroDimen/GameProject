using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{

    public string path;
    public GameObject Player;
    public TMP_Text[] Text_Location;
    public TMP_Text[] Text_Time;
    private void Start()
    {
        path = Application.persistentDataPath + "/";
        Player = GameObject.Find("Player");
        
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(path+ $"{i}"))
            {
                GameManager.instance.player[i] = LoadData(i);
                if (GameManager.instance.player[i].Location != "No Data")
                {
                    Text_Location[i].text = GameManager.instance.player[i].Location;
                    Text_Time[i].text = GameManager.instance.player[i].Time;
                }
            }
            else
            {
                Text_Location[i].text = "No Data";
                Text_Time[i].text = "No Data";
            }
        }
    }
    
    public void Btn1_Data() //이거도 함수 하나로 변경하자
    {
        LoadSceneTest(LoadData(0).Location);
        GameManager.instance.playerNumber = 0;
        UIManager.instance.SlotMenu.SetActive(false);
    }
    public void Btn2_Data()
    {
        LoadSceneTest(LoadData(1).Location);
        GameManager.instance.playerNumber = 1;
        UIManager.instance.SlotMenu.SetActive(false);
    }
    public void Btn3_Data()
    {
        LoadSceneTest(LoadData(2).Location);
        GameManager.instance.playerNumber = 2;
        UIManager.instance.SlotMenu.SetActive(false);
    }
    
    public void SaveDataInGame(int num, string Location = null)
    {
        if (Location != null)
        {
            GameManager.instance.player[GameManager.instance.playerNumber].Location = $"{Location}";
        }
        GameManager.instance.player[GameManager.instance.playerNumber].Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        GameManager.instance.player[GameManager.instance.playerNumber].pos = Player.transform.position; 
        
        string Data = JsonUtility.ToJson(Player);
        
        File.WriteAllText(path + num.ToString(), Data);
    }
    public PlayerData LoadData(int num)
    {
        PlayerData player = new PlayerData();
        if (!File.Exists(path+ $"{num}"))
        {
            
            player.Data_Num = num;
            player.Location = "No Data";
            return player;
        }
        else if (File.Exists(path+ $"{num}"))
        {
            string Data = File.ReadAllText(path + num.ToString());
            player.Data_Num = num;
            player = JsonUtility.FromJson<PlayerData>(Data);
            return player;
        }
        else
        {
            return null;
        }
    }
    
    public void LoadSceneTest(string Location = null)
    {
        if (Location == "No Data")
        {
            //SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene("Village_Chief_House", LoadSceneMode.Additive);
            Player.SetActive(true);
        }
        else if (Location != null)
        {
            //SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene($"{Location}", LoadSceneMode.Additive);
            Player.transform.position = GameManager.instance.player[GameManager.instance.playerNumber].pos;
            Player.SetActive(true);
        }
        else
        {
            Debug.Log("Scene_Manager!");
        }
    }
}
