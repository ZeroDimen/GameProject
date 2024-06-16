using System.Collections;
using UnityEngine;

public class PlayerMoveinFixedUpdate : MonoBehaviour
{
    RaycastHit2D rightHit;
    RaycastHit2D leftHit;
    private Rigidbody2D rigid;
    private Animator ani;

    public float attackAniSpeed = 2.5f;
    public float attackKeySpeed = 0.5f;
    public float playerGrav = 5f;
    public float playerSlidingGrav = 0f;
    float hp = 10;
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
    [SerializeField]
    private bool isDamaged;
    [SerializeField]
    private bool isCoroutineRunning;
    static public bool moveFlag = true;
    static public bool stun = false;
    static public float lastTime;
    static public Vector3 lastPos;
    static public bool flag;

    private bool isRight;

    private bool CanAttack;
    private bool CanFilp;

    public GameObject Attack_Obj;

    void Start()
    {
        CanFilp = true;
        isRight = true;
        isHeading = false;
        isDamaged = false;
        CanAttack = true;
        isCoroutineRunning = false;
        hangTime = .1f;
        jumpInputUp = false;
        flag = false;
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        Application.targetFrameRate = 60;
        rigid.gravityScale = playerGrav;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            ArrowInput();
            Jump();
            Flip();
            Collision_Check();
            IsDamaged();
            if (Input.GetMouseButtonDown(0) && CanAttack && !isSliding)
            {
                CanFilp = false;
                // CanAttack = false;
                StartCoroutine(Disable_Attack(attackKeySpeed));
                Attack_Obj.SetActive(true);
                Attack_Obj.GetComponent<Attack_Test>().Attack_Ani();
                Attack_Speed_Change(attackAniSpeed);
            }

            if (isSliding)
            {
                rigid.gravityScale = playerSlidingGrav;
            }
            else
            {
                rigid.gravityScale = playerGrav;
            }
            ani.SetBool("IsJump", !isGrounded);
            ani.SetBool("IsSliding", isSliding);
            ani.SetFloat("Jump_V", rigid.velocity.y);

        }
        else
        {
            ani.SetBool("IsMove", false);
            transform.position = lastPos;
            if (stun)
            {
                if (Time.time - lastTime >= 3)
                {
                    moveFlag = true;
                    stun = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
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
        CanFilp = true;
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

    private IEnumerator Disable_Attack(float seconds)
    {
        CanAttack = false;
        ani.SetTrigger("IsAttack");
        yield return new WaitForSeconds(seconds);
        CanAttack = true;
    }
    private void PlayerMove()
    {
        if (!flag)
        {
            rigid.velocity = new Vector2(arrowInput * moveSpeed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
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
        if (CanFilp)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isRight = true;

            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isRight = false;
            }
        }
    }
    void IsDamaged()
    {
        if (isDamaged && !isCoroutineRunning)
        {
            StartCoroutine(Blink(gameObject, 2));
            hp--;
        }
    }
    void Collision_Check()
    {
        rightHit = Physics2D.Raycast(transform.position + Vector3.up, Vector3.right, 0.5f);
        leftHit = Physics2D.Raycast(transform.position + Vector3.up, Vector3.left, 0.5f);

        if (rightHit.collider != null && (rightHit.collider.CompareTag("Monster") || rightHit.collider.CompareTag("Boss")))
            isDamaged = true;
        if (leftHit.collider != null && (leftHit.collider.CompareTag("Monster") || leftHit.collider.CompareTag("Boss")))
            isDamaged = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Monster") || other.CompareTag("Stone"))
            isDamaged = true;
        if (other.CompareTag("MonterAttack"))
        {
            flag = true;
            Functions.Hit_Knock_Back(other.gameObject, gameObject);
            isDamaged = true;
            Invoke("Flag", 0.1f);
        }
    }
    void Flag()
    {
        flag = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            isDamaged = false;
    }
    IEnumerator Blink(GameObject obj, int n = 4)
    {
        isCoroutineRunning = true;
        for (int i = 0; i < n; i++)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.2f);
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.2f);
        }
        isCoroutineRunning = false;
        isDamaged = false;
    }
    private void OnCollisionStay2D(Collision2D other)
    {

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
            new Vector3(transform.position.x - 0.2f, transform.position.y + 2f, transform.position.z);
        Vector3 rightPostion =
            new Vector3(transform.position.x + 0.2f, transform.position.y + 2f, transform.position.z);

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
