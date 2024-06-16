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
    private GameObject player1;
    public Vector3 to_Position;
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
        for (int i = 0; i < 3; i++)
        {
            player[i] = new PlayerData();
        }
        player1 = GameObject.Find("Player");
        player1.SetActive(false);
    }

    public void Player_teleport()
    {
        player1.SetActive(true);
        player1.transform.position = to_Position;
        
    }
}
