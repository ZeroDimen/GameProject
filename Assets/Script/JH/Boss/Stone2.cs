using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone2 : MonoBehaviour
{
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
            rigid.velocity = -rigid.velocity;
        if (other.CompareTag("Platform"))
            Destroy(gameObject);
    }
}
