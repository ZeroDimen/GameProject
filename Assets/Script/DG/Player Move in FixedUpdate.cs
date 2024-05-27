using UnityEngine;

public class PlayerMoveinFixedUpdate : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator ani;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int jumpCount; //not use now but player can double jump it'll be use

    public float hangTime = .1f;
    public float hangCounter;
    public float jumpBufferLength = .1f;
    public float jumpBufferCount;
    public float arrowInput;
    public bool jumpInputUp;
    
    public bool isGrounded; // != IsJump
    public bool isSliding;
    
    public SpriteRenderer sprite;
  
    void Start()
    {
        hangTime = .1f;
        jumpInputUp = false;
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ArrowInput();
        Jump();
        Flip();
        if (Input.GetKeyDown(KeyCode.J))
        {
            ani.SetTrigger("IsAttack");
        }
        ani.SetBool("IsJump", !isGrounded);
        ani.SetBool("IsSliding", isSliding);
        ani.SetFloat("Jump_V", rigid.velocity.y);
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(arrowInput * moveSpeed, rigid.velocity.y);
        if (jumpBufferCount>=0&& hangCounter>=0f && isGrounded)
        {
            rigid.AddForce(new Vector2(0, jumpForce));
            jumpBufferCount = 0;
        }

        if (jumpInputUp) //jump velocity / 2
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * .5f);
            jumpInputUp = false;
        }

        Landing_Platform();
    }
    

    private void ArrowInput()
    {
        arrowInput = Input.GetAxisRaw("Horizontal");
        if (arrowInput == 0)
        {
            ani.SetBool("IsMove",false);
        }
        else
        {
            ani.SetBool("IsMove",true);
        }
        
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
            if (!isGrounded)
            {
                if (rigid.velocity.y < 0 && arrowInput != 0)
                {
                    isSliding = true;
                }
            }
            else
            {
                isSliding = false;
            }
        }
    }

    private void Landing_Platform()
    {
        var rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1f,
            LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < 0.15f)
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
            isSliding = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isSliding = false;
        }
    }
}
