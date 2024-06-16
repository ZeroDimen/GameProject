using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Platform") || other.transform.CompareTag("Boss") || other.transform.CompareTag("Player"))
            Destroy(gameObject);

    }
}
