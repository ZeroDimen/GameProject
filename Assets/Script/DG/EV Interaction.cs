using System.Collections;
using UnityEngine;

public class EVInteraction : MonoBehaviour
{
    public GameObject obj;
    
    public float moveSpeed = 5f;
    public float moveTime = 4f;

    private bool EVon = false;
    private bool EV_high = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf && EVon == false && EV_high == false)
        {
            StartCoroutine(EVMoveUpwards(moveSpeed));
            EV_high = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf && EVon == false && EV_high == true) 
        {
            StartCoroutine(EVMoveUpwards(-moveSpeed));
            EV_high = false;
        }
    }
    
    private IEnumerator EVMoveUpwards(float speed = 0f)
    {
        EVon = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * speed; 

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition;
        EVon = false;
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
