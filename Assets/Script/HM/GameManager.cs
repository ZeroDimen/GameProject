using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{ 
    //name, level, money, etc..
    public string Location = "No Data";
    public uint Money = 10;
    public string Time = "None";
    public int Data_Num = 0;
    public Vector3 pos = new Vector3(0, 0, 0);
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string id;
    public string pw;
    public int playerNumber;
    public PlayerData[] player = new PlayerData[3];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
    }
}
