using UnityEngine;

public class OnCamCollider : MonoBehaviour
{
    public GameObject Cam_Col;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            Cam_Col.SetActive(true);
        }
    }
}
