using System.Collections;
using UnityEngine;

public class Goblin : Monster
{
    public MonsterInfo monsterInfo;
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Damaged,
        Greet
    }
    Goblin_Idle_State goblin_Idle_State;
    Goblin_Chase_State goblin_Chase_State;
    AttackState attackState;
    DamagedState damagedState;
    Transform playerPos;
    [SerializeField]
    private State _curState;
    private MonsterStateMachine _monsterStateMachine;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight_Chase;
    RaycastHit2D hitLeft_Chase;
    RaycastHit2D hitDown_Chase;
    Rigidbody2D rigid;
    public Vector3 firstPos;
    int layerMask;
    int layerMask_Chase;

    private void Awake()
    {
        hp = monsterInfo.hp;
        layerMask = 1 << LayerMask.NameToLayer("SeamlessLine") | 1 << LayerMask.NameToLayer("Goblin");
        layerMask_Chase = 1 << LayerMask.NameToLayer("Platform");
        firstPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Init();
        _curState = State.Idle;
        _monsterStateMachine = new MonsterStateMachine(goblin_Idle_State);
        goblin_Idle_State.StateEnter();
        StartCoroutine(Goblin_State());
    }
    private void OnDrawGizmos()
    {
        if (_curState == State.Idle)
        {
            Gizmos.DrawRay(transform.position, Vector3.right * monsterInfo.fieldOfView);
            Gizmos.DrawRay(transform.position, Vector3.left * monsterInfo.fieldOfView);
        }
        else if (_curState == State.Chase)
        {
            Gizmos.DrawWireSphere(transform.position, monsterInfo.fieldOfView);
        }
    }
    IEnumerator Goblin_State()
    {
        while (true)
        {
            if (GameObject.Find("Player"))
                playerPos = GameObject.Find("Player").transform;
            switch (_curState)
            {
                case State.Idle:
                    hitRight = Physics2D.Raycast(transform.position, Vector2.right, monsterInfo.fieldOfView, ~layerMask);
                    hitLeft = Physics2D.Raycast(transform.position, Vector2.left, monsterInfo.fieldOfView, ~layerMask);

                    if (hitRight.collider != null && hitRight.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    if (hitLeft.collider != null && hitLeft.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    break;
                case State.Chase:
                    hitRight_Chase = Physics2D.Raycast(transform.position, Vector2.right, 2, layerMask_Chase);
                    hitLeft_Chase = Physics2D.Raycast(transform.position, Vector2.left, 2, layerMask_Chase);
                    hitDown_Chase = Physics2D.Raycast(transform.position, Vector2.down, 2, layerMask_Chase);

                    // if (hitRight_Chase.collider != null || hitLeft_Chase.collider != null)
                    //     ChangeState(State.Idle);
                    if (Vector3.Distance(playerPos.position, transform.position) >= monsterInfo.fieldOfView)
                        ChangeState(State.Idle);
                    if (Vector3.Distance(playerPos.position, transform.position) <= monsterInfo.attackRange)
                        ChangeState(State.Attack);
                    break;
                case State.Attack:
                    Debug.Log("Attack");
                    if (Vector3.Distance(playerPos.position, transform.position) >= monsterInfo.attackRange)
                        ChangeState(State.Chase);
                    break;
                case State.Greet:
                    yield return StartCoroutine(Blink_Color(gameObject, Color.green, 2));
                    _curState = State.Idle;
                    goblin_Idle_State.StateEnter();
                    break;
                case State.Damaged:
                    hp--;
                    if (hp <= 0)
                        Destroy(gameObject);
                    Debug.Log(hp);
                    break;
            }
            _monsterStateMachine.UPdateStage();
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void Init()
    {
        goblin_Idle_State = new Goblin_Idle_State(gameObject);
        goblin_Chase_State = new Goblin_Chase_State(gameObject);
        attackState = new AttackState(gameObject);
        damagedState = new DamagedState(gameObject);
    }
    public void ChangeState(State nextState)
    {
        _curState = nextState;
        switch (_curState)
        {
            case State.Idle:
                _monsterStateMachine.ChangeState(goblin_Idle_State);
                break;
            case State.Chase:
                _monsterStateMachine.ChangeState(goblin_Chase_State);
                break;
            case State.Attack:
                _monsterStateMachine.ChangeState(attackState);
                break;
            case State.Damaged:
                _monsterStateMachine.ChangeState(damagedState);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_curState == State.Idle && other.gameObject.layer == LayerMask.NameToLayer("Goblin"))
            ChangeState(State.Greet);
        if (other.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
                Destroy(gameObject);
        }
    }
}
