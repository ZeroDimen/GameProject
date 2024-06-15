using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{

    public string path;
    public GameObject Player;
    public TMP_Text[] Text_Location;
    public TMP_Text[] Text_Time;

    public Button Slot1, Slot2, Slot3;
    
    private void Start()
    {
        path = Application.persistentDataPath + "/";
        
        Slot1.onClick.AddListener(()=>SlotLoadBtn(0));
        Slot2.onClick.AddListener(()=>SlotLoadBtn(1));
        Slot3.onClick.AddListener(()=>SlotLoadBtn(2));
    }

    public void SlotLoad() //load slot from local
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.instance.player[i].Location != "No Data")
            {
                Text_Location[i].text = GameManager.instance.player[i].Location;
                Text_Time[i].text = GameManager.instance.player[i].Time;
                
            }
            else
            {
                Text_Location[i].text = "No Data";
                Text_Time[i].text = "No Data";
            }
        }
    }
    
    // public void Btn1_Data() //이거도 함수 하나로 변경하자
    // {
    //     LoadSceneTest(LoadData(0).Location);
    //     GameManager.instance.playerNumber = 0;
    //     UIManager.instance.SlotMenu.SetActive(false);
    // }
    // public void Btn2_Data()
    // {
    //     LoadSceneTest(LoadData(1).Location);
    //     GameManager.instance.playerNumber = 1;
    //     UIManager.instance.SlotMenu.SetActive(false);
    // }
    // public void Btn3_Data()
    // {
    //     LoadSceneTest(LoadData(2).Location);
    //     GameManager.instance.playerNumber = 2;
    //     UIManager.instance.SlotMenu.SetActive(false);
    // }

    public void SlotLoadBtn(int index)
    {
        LoadSceneTest(GameManager.instance.player[index].Location);
        GameManager.instance.playerNumber = index;
        UIManager.instance.SlotMenu.SetActive(false);
        UIManager.instance.gameStatus = GameStatus.Ingame;
    }
    
    public void SaveDataInGame(int num, string Location = null)
    {
        if (Location != null)
        {
            GameManager.instance.player[num].Location = $"{Location}";
        }
        GameManager.instance.player[num].Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        GameManager.instance.player[num].pos = Player.transform.position; 
        
        string Data = JsonUtility.ToJson(GameManager.instance.player[num]);
        
        File.WriteAllText(path + num.ToString(), Data);
    }
    public void LoadData()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!File.Exists(path + $"{i}"))
            {
                Debug.Log($"There is no File{i}");
                GameManager.instance.player[i].Data_Num = i;
                GameManager.instance.player[i].Location = "No Data";
            }
            else
            {
                string Data = File.ReadAllText(path + i.ToString());
                //GameManager.instance.player[i].Data_Num = i;
                GameManager.instance.player[i] = JsonUtility.FromJson<PlayerData>(Data);
            }
        }
    }
    
    public void LoadSceneTest(string Location = null) // scene load when slot btn click
    {
        if (Location == "No Data")
        {
            //SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene("Village", LoadSceneMode.Additive);
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
