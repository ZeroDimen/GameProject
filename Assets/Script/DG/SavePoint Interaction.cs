using UnityEngine;

public class SavePointInteraction : MonoBehaviour
{
    public GameObject obj;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            GameObject.Find("Game Manager").GetComponent<Data_Manager>().SaveData(Data_Manager.instance.NowPlayer.Data_Num, "Village");
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
