using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    public void Test(string Location = null)
    {
        if (Location == "No Data")
        {
            SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene("Village_Chief_House", LoadSceneMode.Additive);
            Player.SetActive(true);
            GameObject.Find("Player").GetComponent<PlayerMoveinFixedUpdate>().Gravity(5);
        }
        else if (Location != null)
        {
            SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene($"{Location}", LoadSceneMode.Additive);
            Player.transform.position = Data_Manager.instance.NowPlayer.pos;
            Player.SetActive(true);
            GameObject.Find("Player").GetComponent<PlayerMoveinFixedUpdate>().Gravity(5);
        }
        else
        {
            Debug.Log("Scene_Manager!");
        }
        
    }
}
