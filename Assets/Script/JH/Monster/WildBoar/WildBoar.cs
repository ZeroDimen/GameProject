using System.Collections;
using UnityEngine;

public class WildBoar : Monster
{
    public MonsterInfo monsterInfo;
    private enum State
    {
        Idle,
        Chase,
        Damaged,
        Death
    }
    IdleState idleState;
    WildBoarChaseState wildBoarChaseState;
    Transform playerPos;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    [SerializeField]
    private State _curState;
    private MonsterStateMachine _monsterStateMachine;
    int layerMask;
    public bool isStun;

    private void Awake()
    {
        isStun = false;
        // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
        //     LayerMask.NameToLayer("WildBoar"));
        hp = monsterInfo.hp;
        layerMask = (1 << LayerMask.NameToLayer("SeamlessLine")) | (1 << LayerMask.NameToLayer("WildBoar"));
    }
    private void Start()
    {
        Init();
        _curState = State.Idle;
        _monsterStateMachine = new MonsterStateMachine(idleState);
        StartCoroutine(WildBoar_State());
    }
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.white;
        // Gizmos.DrawWireSphere(transform.position, monsterInfo.fieldOfView);
        Gizmos.DrawRay(transform.position, Vector2.right * 15);
        Gizmos.DrawRay(transform.position, Vector2.left * 15);
    }

    IEnumerator WildBoar_State()
    {
        while (true)
        {
            playerPos = GameObject.Find("Player").transform;
            switch (_curState)
            {
                case State.Idle:
                    //가로로 15 이내에 플레이어가 있는지 체크
                    // hitRight = Physics2D.Raycast(transform.position, Vector2.right, 15f, 1 << LayerMask.NameToLayer("Player"));
                    // hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 15f, 1 << LayerMask.NameToLayer("Player"));
                    hitRight = Physics2D.Raycast(transform.position, Vector2.right, 15f, ~layerMask);
                    hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 15f, ~layerMask);

                    // if (hitRight.collider != null || hitLeft.collider != null)
                    // {
                    //     Debug.Log(hitRight.transform.name);
                    //     if (hitRight.transform.CompareTag("Player") || hitLeft.transform.CompareTag("Player"))
                    //     {
                    //         ChangeState(State.Chase);
                    //         yield return StartCoroutine(Blink_Color(gameObject, Color.red, 1));
                    //     }
                    // }
                    // else
                    // {
                    //     Debug.Log("Empty");
                    // }
                    if (hitRight.collider != null)
                    {
                        if (hitRight.transform.CompareTag("Player"))
                        {
                            ChangeState(State.Chase);
                            yield return StartCoroutine(Blink_Color(gameObject, Color.red, 1));
                        }
                    }
                    if (hitLeft.collider != null)
                    {
                        if (hitLeft.transform.CompareTag("Player"))
                        {
                            ChangeState(State.Chase);
                            yield return StartCoroutine(Blink_Color(gameObject, Color.red, 1));
                        }
                    }
                    break;
                case State.Chase:
                    if (isStun)
                    {
                        yield return StartCoroutine(Blink(gameObject, 3));
                        isStun = false;
                        ChangeState(State.Idle);
                    }
                    break;
                case State.Damaged:
                    // 애니메이션 시작
                    hp--;
                    // 애니메이션 끝
                    ChangeState(State.Idle);

                    if (hp <= 0)
                        Destroy(gameObject);
                    Debug.Log(hp);
                    break;
                case State.Death:
                    // 애니메이션 시작
                    // 애니메이션 끝
                    break;
            }
            _monsterStateMachine.UPdateStage();

            yield return new WaitForSeconds(0.03f);
        }
    }
    private void Init()
    {
        idleState = new IdleState(gameObject);
        wildBoarChaseState = new WildBoarChaseState(gameObject);
    }
    private void ChangeState(State nextState)
    {
        _curState = nextState;
        switch (_curState)
        {
            case State.Idle:
                _monsterStateMachine.ChangeState(idleState);
                break;
            case State.Chase:
                _monsterStateMachine.ChangeState(wildBoarChaseState);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Blink(other.gameObject));
        }
        if (other.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
                Destroy(gameObject);
            Debug.Log(hp);
        }
    }
}
