using System;
using System.IO;
using UnityEngine;



public class Data_Manager : MonoBehaviour
{
    public static Data_Manager instance;

    public PlayerData NowPlayer = new PlayerData();
    public GameObject Scene_Manager;
    private GameObject Player;
    public string path;
    public string id;
    public string pw;
    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        #endregion

        DontDestroyOnLoad(this.gameObject);
        path = Application.persistentDataPath + "/";
    }

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    #region Button
    public void Btn1_Data()
    {
        Scene_Manager.GetComponent<Scene_Manager>().Test(LoadData(0).Location);
    }
    public void Btn2_Data()
    {
        Scene_Manager.GetComponent<Scene_Manager>().Test(LoadData(1).Location);
    }
    public void Btn3_Data()
    {
        Scene_Manager.GetComponent<Scene_Manager>().Test(LoadData(2).Location);
    }
    #endregion
    
    public void SaveData(int num, string Location = null)
    {
        if (Location != null)
        {
            NowPlayer.Location = $"{Location}";
        }
        NowPlayer.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        NowPlayer.pos = Player.transform.position; 
        
        string Data = JsonUtility.ToJson(NowPlayer);
        
        File.WriteAllText(path + num.ToString(), Data);
    }
    public PlayerData LoadData(int num)
    {
        if (!File.Exists(Data_Manager.instance.path+ $"{num}"))
        {
            NowPlayer = new PlayerData();
            NowPlayer.Data_Num = num;
            NowPlayer.Location = "No Data";
            return NowPlayer;
        }
        else if (File.Exists(Data_Manager.instance.path+ $"{num}"))
        {
            string Data = File.ReadAllText(path + num.ToString());
            NowPlayer.Data_Num = num;
            NowPlayer = JsonUtility.FromJson<PlayerData>(Data);
            return NowPlayer;
        }
        else
        {
            return null;
        }
    }
    
}