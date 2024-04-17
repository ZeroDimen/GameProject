using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterTestMove : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float moveSpeed = 5f;
    public float jumpForce;
    public int jumpCount;
    public bool isGrounded;

    public float hangTime;
    public float hangCounter;
    public float jumpBufferLength = .1f;
    public float jumpBufferCount;
    public bool isJump;

    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        hangTime = .1f;
        isJump = false;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
        Flip();
    }

    void Move()
    {
        rigid.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rigid.velocity.y);
    }

    void Jump()
    {
        if (isGrounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        if (jumpBufferCount >= 0 && hangCounter >= 0f) 
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            isGrounded = false;
            jumpBufferCount = 0;
            isJump = true;
            Debug.Log("press jump");
        }

        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * .5f); 
        }

        if ((rigid.velocity.y < 0) && isJump)
        {
            //rigid.gravityScale /= 2;
            Debug.Log("max pos");
        }
    }

    void Flip()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            if (isJump)
            {
                //rigid.gravityScale *= 2;
                isJump = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
}
