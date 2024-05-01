using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePointInteraction : MonoBehaviour
{
    public GameObject obj;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            //Warning
            for (int i = 0; i < SceneManager.sceneCount; i++) 
            {
                // Scene scene = SceneManager.GetSceneAt(i);
                // Debug.Log(scene.name);
                GameObject.Find("Game Manager").GetComponent<Data_Manager>().SaveData(Data_Manager.instance.NowPlayer.Data_Num, SceneManager.GetSceneAt(i).name);
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        obj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        obj.SetActive(false);
    }
}
