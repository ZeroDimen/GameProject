using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone2 : MonoBehaviour
{
    Vector3 playerPos;
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        playerPos = GameObject.Find("Player").transform.position;

        if (Vector3.Distance(playerPos, transform.position) <= 7)
            if (Input.GetKeyDown(KeyCode.J))
                rigid.velocity = -rigid.velocity;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Boss"))
            Destroy(gameObject);
    }
}
