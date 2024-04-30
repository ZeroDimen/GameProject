using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void Test(string Location = null)
    {
        if (Location == "No Data")
        {
            SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene("Village_Chief_House", LoadSceneMode.Additive);
        }
        else if (Location != null)
        {
            SceneManager.UnloadSceneAsync("StartScene");
            SceneManager.LoadScene($"{Location}", LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("Location !");
        }
        
    }
}
