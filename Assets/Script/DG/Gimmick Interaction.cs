using UnityEngine;
using System.Collections;
using Cinemachine;

public class GimmickInteraction : MonoBehaviour
{
    public GameObject obj;
    public GameObject Wall;

    // private GameObject Player;

    private GameObject Player_ob;
    
    // public CinemachineBlendListCamera Blend;
    public GameObject Blend;
    public CinemachineVirtualCamera B_1;
    
    private void Start()
    {
        // Player = GameObject.Find("Player");
        Player_ob = GameObject.Find("Player").transform.Find("GameObject").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            if (B_1 != null && Blend != null)
            {
                B_1.Follow = Player_ob.transform;
                Blend.SetActive(true);
            }
            
            StartCoroutine(MoveObjectDown());
            
        }
    }
    
    IEnumerator MoveObjectDown()
    {
        Vector3 startPosition = Wall.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + 7f, startPosition.z);

        float elapsedTime = -2f;
        float duration = 1.5f;

        while (elapsedTime < duration + 2f)
        {
            if ((elapsedTime < duration) && elapsedTime >= 0f)
            {
                Wall.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Wall.transform.position = endPosition;
        
        if (B_1 != null && Blend != null)
        {
            Blend.SetActive(false);
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
