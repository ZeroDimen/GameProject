using System.Collections;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    private float fallTime = 5f, destroyTime = 2f , recoverTime = 5f; 
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.name.Equals("Player"))
        {
            StartCoroutine(FallingPlat());
        }
    }

    private IEnumerator FallingPlat()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.down * 10f;
        
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y , transform.eulerAngles.z + 180f);

        float elapsedTime = 0f;

        while (elapsedTime < destroyTime + fallTime + recoverTime) 
        {
            if (elapsedTime > destroyTime)
            {
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / fallTime);
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / fallTime);
            }

            if (elapsedTime > destroyTime + recoverTime)
            {
                transform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime / fallTime);
            }
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        transform.rotation = startRotation;
        
    }
}