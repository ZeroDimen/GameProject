using System.Collections;
using UnityEngine;

public class PlayerMoveinFixedUpdate1 : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator ani;

    public float attackSpeed = 2.5f;
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
    private bool isHeading;
    
    private bool CanAttack;

    public GameObject Attack_Obj;

    void Start()
    {
        isHeading = false;
        CanAttack = true;
        hangTime = .1f;
        jumpInputUp = false;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        ArrowInput();
        Jump();
        Flip();
        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            CanAttack = false;
            Attack_Obj.SetActive(true);
            Attack_Obj.GetComponent<Attack_Test>().Attack_Ani();
            Attack_Speed_Change(attackSpeed);
        }

        ani.SetBool("IsJump", !isGrounded);
        ani.SetBool("IsSliding", isSliding);
        ani.SetFloat("Jump_V", rigid.velocity.y);
        ani.SetBool("IsAttack", !CanAttack);
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(arrowInput * moveSpeed, rigid.velocity.y);


        Landing_Platform();
        Heading_Platform();
    }

    private void Attack_Speed_Change(float Speed)
    {
        ani.SetFloat("AttackSpeed", Speed);
        Attack_Obj.GetComponent<Attack_Test>().Attack_Effact_Speed_Change(Speed);
    }

    public void Attack_Ani_End()
    {
        CanAttack = true;
    }
    private void ArrowInput()
    {
        arrowInput = Input.GetAxisRaw("Horizontal");
        if (arrowInput == 0)
        {
            ani.SetBool("IsMove", false);
        }
        else
        {
            ani.SetBool("IsMove", true);
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

        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0)
        {
            jumpInputUp = true;
        }
        if (jumpBufferCount >= 0 && hangCounter >= 0f && isGrounded)
        {
            rigid.AddForce(new Vector2(0, jumpForce));
            jumpBufferCount = 0;
        }

        if (jumpInputUp) //jump velocity / 2
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * .5f);
            jumpInputUp = false;
        }
    }

    void Flip() //just flip sprite
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            // sprite.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            // sprite.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            StartCoroutine(Blink(gameObject, 2));
    }
    IEnumerator Blink(GameObject obj, int n = 4)
    {
        for (int i = 0; i < n; i++)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.2f);
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            if (!isGrounded)
            {
                if (rigid.velocity.y < 0 && arrowInput != 0 && !isHeading)
                {
                    isSliding = true;
                    CanAttack = false;
                }
            }
            else
            {
                isSliding = false;
                CanAttack = true;
            }
        }
    }
    private void Landing_Platform()
    {
        Vector3 leftPostion =
            new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        Vector3 rightPostion =
            new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);

        // Debug.DrawRay(leftPostion, Vector3.down, new Color(0, 1, 0));
        // Debug.DrawRay(rightPostion, Vector3.down, new Color(0, 1, 0));
        
        var leftrayHit = Physics2D.Raycast(leftPostion, Vector2.down, 1f,
            LayerMask.GetMask("Platform"));
        
        var rightrayHit = Physics2D.Raycast(rightPostion, Vector2.down, 1f,
            LayerMask.GetMask("Platform"));
        
        if (leftrayHit.collider != null || rightrayHit.collider != null)
        {
            if (leftrayHit.distance < 0.5f || rightrayHit.distance < 0.5f)
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

    private void Heading_Platform()
    {
        Vector3 leftPostion =
            new Vector3(transform.position.x - 0.2f, transform.position.y+2f, transform.position.z);
        Vector3 rightPostion =
            new Vector3(transform.position.x + 0.2f, transform.position.y+2f, transform.position.z);
        
        // Debug.DrawRay(leftPostion, Vector3.up, new Color(0, 1, 0));
        // Debug.DrawRay(rightPostion, Vector3.up, new Color(0, 1, 0));
        
        var leftrayHit = Physics2D.Raycast(leftPostion, Vector2.up, 1f,
            LayerMask.GetMask("Platform"));
        
        var rightrayHit = Physics2D.Raycast(rightPostion, Vector2.up, 1f,
            LayerMask.GetMask("Platform"));

        if (leftrayHit.collider != null || rightrayHit.collider != null)
        {
            if (leftrayHit.distance < 0.3f || rightrayHit.distance < 0.3f)
            {
                isHeading = true;
            }
        }
        else
        {
            isHeading = false;
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
