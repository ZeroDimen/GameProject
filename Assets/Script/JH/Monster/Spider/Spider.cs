using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spider : Monster
{
    public MonsterInfo monsterInfo;
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Damaged,
        Greet,
        Death
    }
    Animator anime;
    Spider_Idle_State spider_Idle_State;
    Spider_Chase_State spider_Chase_State;
    Spider_Attack_State spider_Attack_State;
    Spider_Damaged_State spider_Damaged_State;
    Spider_Death_State spider_Death_State;
    Transform playerPos;
    [SerializeField]
    private State _curState;
    private MonsterStateMachine _monsterStateMachine;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    Rigidbody2D rigid;
    public Vector3 firstPos;
    int layerMask;
    public bool isdead = false;
    bool isFall;

    private void Awake()
    {
        isFall = false;
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        hp = monsterInfo.hp;
        layerMask = 1 << LayerMask.NameToLayer("SeamlessLine") | 1 << LayerMask.NameToLayer("Spider") | 1 << LayerMask.NameToLayer("MonsterAttack");
        firstPos = transform.position;
    }
    private void Start()
    {
        Init();
        _curState = State.Idle;
        _monsterStateMachine = new MonsterStateMachine(spider_Idle_State);
        spider_Idle_State.StateEnter();
        StartCoroutine(Spider_State());
    }
    private void Update()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Spider"), true);
    }
    private void OnDrawGizmos()
    {
        if (_curState == State.Idle)
        {
            Gizmos.DrawRay(transform.position + Vector3.up * 2, Vector3.right * monsterInfo.fieldOfView);
            Gizmos.DrawRay(transform.position + Vector3.up * 2, Vector3.left * monsterInfo.fieldOfView);
        }
        else if (_curState == State.Chase)
        {
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2, monsterInfo.fieldOfView);
        }
    }
    IEnumerator Spider_State()
    {
        while (true)
        {
            playerPos = GameObject.Find("Player").transform;
            switch (_curState)
            {
                case State.Idle:
                    hitRight = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.right, monsterInfo.fieldOfView, ~layerMask);
                    hitLeft = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.left, monsterInfo.fieldOfView, ~layerMask);

                    if (hitRight.collider != null && hitRight.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    if (hitLeft.collider != null && hitLeft.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    break;
                case State.Chase:
                    if (Vector3.Distance(playerPos.position, transform.position) >= monsterInfo.fieldOfView)
                        ChangeState(State.Idle);
                    if (Vector3.Distance(playerPos.position, transform.position) <= monsterInfo.attackRange)
                        ChangeState(State.Attack);
                    break;
                case State.Attack:
                    if (Vector3.Distance(playerPos.position, transform.position) >= monsterInfo.attackRange)
                        ChangeState(State.Chase);
                    break;
                case State.Greet:
                    yield return StartCoroutine(Blink_Color(gameObject, Color.green, 2));
                    _curState = State.Idle;
                    spider_Idle_State.StateEnter();
                    break;
                case State.Damaged:
                    if (anime.GetCurrentAnimatorStateInfo(0).IsName("Damage") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    {
                        if (hp <= 0)
                            ChangeState(State.Death);
                        else if (Vector3.Distance(playerPos.position, transform.position) <= monsterInfo.attackRange)
                            ChangeState(State.Attack);
                        else if (Vector3.Distance(playerPos.position, transform.position) <= monsterInfo.fieldOfView)
                            ChangeState(State.Chase);
                        else
                            ChangeState(State.Idle);

                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                case State.Death:
                    if (isdead)
                        Destroy(gameObject);
                    break;
            }
            _monsterStateMachine.UPdateStage();
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void Init()
    {
        spider_Idle_State = new Spider_Idle_State(gameObject);
        spider_Chase_State = new Spider_Chase_State(gameObject);
        spider_Attack_State = new Spider_Attack_State(gameObject);
        spider_Damaged_State = new Spider_Damaged_State(gameObject);
        spider_Death_State = new Spider_Death_State(gameObject);
    }
    public void ChangeState(State nextState)
    {
        _curState = nextState;
        switch (_curState)
        {
            case State.Idle:
                _monsterStateMachine.ChangeState(spider_Idle_State);
                break;
            case State.Chase:
                _monsterStateMachine.ChangeState(spider_Chase_State);
                break;
            case State.Attack:
                _monsterStateMachine.ChangeState(spider_Attack_State);
                break;
            case State.Damaged:
                _monsterStateMachine.ChangeState(spider_Damaged_State);
                break;
            case State.Death:
                _monsterStateMachine.ChangeState(spider_Death_State);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_curState == State.Idle && other.gameObject.layer == LayerMask.NameToLayer("Spider"))
            ChangeState(State.Greet);
        if (other.CompareTag("Weapon") && _curState != State.Death)
        {
            Functions.Hit_Knock_Back(other.gameObject, gameObject, 14f);
            Invoke("Velocity_Zero", 0.2f);
            hp--;
            ChangeState(State.Damaged);
        }
    }
    void Velocity_Zero()
    {
        rigid.velocity = Vector3.zero;
    }
    void Animation_Event()
    {
        GameObject obj = transform.GetChild(0).gameObject;
        if (obj.activeSelf == true)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
}
