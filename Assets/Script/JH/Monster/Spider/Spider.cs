using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Transform playerPos;
    [SerializeField]
    private State _curState;
    private MonsterStateMachine _monsterStateMachine;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight_Chase;
    RaycastHit2D hitLeft_Chase;
    RaycastHit2D hitDown_Chase;
    int layerMask;
    int layerMask_Chase;
    float animeTime;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        hp = monsterInfo.hp;
        layerMask = 1 << LayerMask.NameToLayer("SeamlessLine") | 1 << LayerMask.NameToLayer("Spider");
        layerMask_Chase = 1 << LayerMask.NameToLayer("Platform");
    }
    private void Start()
    {
        Init();
        _curState = State.Idle;
        _monsterStateMachine = new MonsterStateMachine(spider_Idle_State);
        spider_Idle_State.StateEnter();
        StartCoroutine(Goblin_State());
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
            Gizmos.DrawWireSphere(transform.position, monsterInfo.fieldOfView);
        }
    }
    IEnumerator Goblin_State()
    {
        while (true)
        {
            playerPos = GameObject.Find("Player").transform;
            switch (_curState)
            {
                case State.Idle:
                    hitRight = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.right, monsterInfo.fieldOfView, ~layerMask);
                    hitLeft = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.left, monsterInfo.fieldOfView, ~layerMask);
                    if (hitRight.collider != null) Debug.Log(hitRight.collider.name);
                    if (hitRight.collider != null && hitRight.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    if (hitLeft.collider != null && hitLeft.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        ChangeState(State.Chase);

                    break;
                case State.Chase:
                    hitRight_Chase = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.right, 2, layerMask_Chase);
                    hitLeft_Chase = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.left, 2, layerMask_Chase);
                    hitDown_Chase = Physics2D.Raycast(transform.position + Vector3.up * 2, Vector2.down, 2, layerMask_Chase);

                    if (hitRight_Chase.collider != null || hitLeft_Chase.collider != null || hitDown_Chase.collider == null)
                        ChangeState(State.Idle);
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
                    animeTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animeTime == 0 || animeTime >= 1f)
                    {
                        if (Vector3.Distance(playerPos.position, transform.position) >= monsterInfo.attackRange)
                            ChangeState(State.Chase);
                        else
                            ChangeState(State.Attack);
                    }
                    break;
                case State.Death:
                    if (hp <= 0)
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
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_curState == State.Idle && other.gameObject.layer == LayerMask.NameToLayer("Goblin"))
            ChangeState(State.Greet);
        if (other.CompareTag("Player"))
            StartCoroutine(Blink(other.gameObject, 1));
        if (other.CompareTag("Weapon"))
            ChangeState(State.Damaged);
    }
}
