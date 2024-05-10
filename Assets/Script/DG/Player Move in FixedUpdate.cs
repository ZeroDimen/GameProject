using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveinFixedUpdate : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int jumpCount; //not use now but player can double jump it'll be use
    public bool isGrounded;

    public float hangTime = .1f;
    public float hangCounter;
    public float jumpBufferLength = .1f;
    public float jumpBufferCount;
    public bool isJump;
    public float arrowInput;
    public bool jumpInputUp;
    public Animator ani;
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        hangTime = .1f;
        isJump = false;
        jumpInputUp = false;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Gravity();
    }

    // Update is called once per frame
    void Update()
    {
        ArrowInput();
        Jump();
        Flip();
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(arrowInput * moveSpeed, rigid.velocity.y);
        if (jumpBufferCount>=0&& hangCounter>=0f && isGrounded)
        {
            rigid.AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
            jumpBufferCount = 0;
            isJump = true;
        }

        if (jumpInputUp) //jump velocity / 2
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * .5f);
            jumpInputUp = false;
        }
    }

    public void Gravity(int i = 0)
    {
        rigid.gravityScale = i;
    }

    private void ArrowInput()
    {
        arrowInput = Input.GetAxisRaw("Horizontal");
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

        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0 )
        {
            jumpInputUp = true;
        }
    }

    void Flip() //just flip sprite
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
