using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject obj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            SceneManager.LoadScene("Village", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Village_Chief_House");
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
