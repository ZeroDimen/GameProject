using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Stone"))
            Destroy(gameObject);
    }
}
