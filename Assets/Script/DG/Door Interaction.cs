using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject obj;
    private GameObject Player;
    
    public string from_SceneName;
    public string to_SceneName;
    
    public Vector3 to_Position;
    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            SceneManager.LoadScene(to_SceneName, LoadSceneMode.Additive);
            Player.transform.position = to_Position;
            SceneManager.UnloadSceneAsync(from_SceneName);
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
