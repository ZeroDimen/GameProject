using System.Collections;
using UnityEngine;

public class WildBoar : Monster
{
    private enum State
    {
        Idle,
        Chase,
        Damaged,
        Death
    }
    public MonsterInfo monsterInfo;
    IdleState idleState;
    WildBoarChaseState wildBoarChaseState;
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
        hp = monsterInfo.hp;
        // SeamlessLine과 WildBoar 레이어를 무시
        // 지금 보니 Platform과 Player만 인식 되도록 해도 되었을듯
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
        Gizmos.DrawRay(transform.position, Vector2.right * 15);
        Gizmos.DrawRay(transform.position, Vector2.left * 15);
    }

    IEnumerator WildBoar_State()
    {
        while (true)
        {
            switch (_curState)
            {
                case State.Idle:
                    //가로로 15 이내에 충돌체가 있는지 체크
                    hitRight = Physics2D.Raycast(transform.position, Vector2.right, 15f, ~layerMask);
                    hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 15f, ~layerMask);

                    // 오른쪽 충돌체 체크
                    if (hitRight.collider != null)
                    {
                        // 충돌체가 플레이어인지 체크
                        if (hitRight.transform.CompareTag("Player"))
                        {
                            ChangeState(State.Chase);
                            yield return StartCoroutine(Blink_Color(gameObject, Color.red, 1));
                        }
                    }
                    // 왼쪽 충돌체 체크
                    if (hitLeft.collider != null)
                    {
                        // 충돌체가 플레이어인지 체크
                        if (hitLeft.transform.CompareTag("Player"))
                        {
                            ChangeState(State.Chase);
                            yield return StartCoroutine(Blink_Color(gameObject, Color.red, 1));
                        }
                    }
                    break;
                case State.Chase:
                    // 벽에 부딪혀서 스턴 상태인지 체크
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
        if (other.CompareTag("Weapon"))
        {
            hp--;
            if (hp <= 0)
                Destroy(gameObject);
            Debug.Log(hp);
        }
    }
}
