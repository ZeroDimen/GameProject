using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public MonsterInfo monsterInfo;
    public enum State
    {
        Waiting,
        FirstAction,
        Idle,
        Earthquake,
        Cry,
        Tornado,
        Dash,
        Path,
        Throw
    }
    Animator anime;
    GameObject sliderObj;
    RectTransform sliderRectTransform;
    Camera mainCamera;
    Slider slider;
    public State _curState;
    Transform playerPos;
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;
    GameObject WideCam;
    public GameObject path;
    public GameObject stone;
    public GameObject shadow;
    public GameObject hitRange;
    Rigidbody2D rigid;
    public GameObject[] walls;
    float initialRadius = 0f; // 초기 원의 반지름
    float maxRadius = 10f; // 최대 원의 반지름
    float expandSpeed = 2.5f; // 원이 커지는 속도
    float currentRadius; // 현재 원의 반지름
    int segments = 50; // 원을 그릴 때 사용하는 세그먼트의 수
    private LineRenderer lineRenderer;
    float curAttackNum = 0;
    float nextAttackNum;
    float hp;
    Vector3 lastPlayerPos;
    IEnumerator phase_1;
    IEnumerator phase_2;
    RaycastHit2D right;
    RaycastHit2D left;
    bool flag;
    public bool phase_1_is_running;
    public bool phase_2_is_running;
    private void Awake()
    {
        anime = GetComponent<Animator>();
        phase_1_is_running = true;
        phase_2_is_running = false;
        phase_1 = Phase1();
        phase_2 = Phase2();
        hp = monsterInfo.hp;
        _curState = State.Waiting;
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        currentRadius = initialRadius;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartCoroutine(phase_1);
        sliderObj = GameObject.Find("BossHP").transform.GetChild(0).gameObject;
        slider = sliderObj.GetComponent<Slider>();

        sliderRectTransform = sliderObj.GetComponent<RectTransform>();
        CinemachineBrain cinemachineBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        mainCamera = cinemachineBrain.OutputCamera;
    }
    private void Update()
    {
        if (_curState == State.Dash && !flag)
        {
            playerPos = GameObject.Find("Player").transform;
            right = Physics2D.Raycast(transform.position + new Vector3(0.5f, 2f, 0), Vector3.right, 0.3f, 1 << LayerMask.NameToLayer("Player"));
            left = Physics2D.Raycast(transform.position + new Vector3(0.5f, 2f, 0), Vector3.left, 0.3f, 1 << LayerMask.NameToLayer("Player"));

            if (right.collider != null)
            {
                if (playerPos.position.x - transform.position.x > 0 && !flag)
                {
                    PlayerMoveinFixedUpdate.flag = true;
                    playerPos.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 40f + Vector3.up * 10, ForceMode2D.Impulse);
                    Invoke("playerFlag", 0.5f);
                    flag = true;
                }
            }

            if (left.collider != null)
            {
                if (playerPos.position.x - transform.position.x < 0 && !flag)
                {
                    PlayerMoveinFixedUpdate.flag = true;
                    playerPos.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 40f + Vector3.up * 10, ForceMode2D.Impulse);
                    Invoke("playerFlag", 0.5f);
                    flag = true;
                }
            }
        }

        if (transform.position.y <= 167)
            transform.position = new Vector3(transform.position.x, 167.5f, transform.position.z);

        if (GameObject.Find("Player") != null)
            playerPos = GameObject.Find("Player").transform;
        transform.localScale = new Vector3(-Character_Direction(playerPos, transform), 1, 1);

        // if (!phase_1_is_running && !phase_1_is_running)
        //     StartCoroutine(phase_2);
    }
    void CreatePoints(float radius)
    {
        float angle = 2 * Mathf.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(i * angle) * radius;
            float y = Mathf.Cos(i * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, -1));
        }
    }
    private void OnDrawGizmos()
    {
        if (_curState == State.Waiting)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0.5f, 2f, 0), monsterInfo.fieldOfView);
        }
    }
    IEnumerator Phase1()
    {
        if (GameObject.Find("Player") != null)
            playerPos = GameObject.Find("Player").transform;
        transform.localScale = new Vector3(-Character_Direction(playerPos, transform), 1, 1);
        while (true)
        {
            switch (_curState)
            {
                case State.Waiting:
                    if (Vector3.Distance(playerPos.position, transform.position) < monsterInfo.fieldOfView)
                    {
                        _curState = State.FirstAction;
                        PlayerMoveinFixedUpdate.moveFlag = false;
                        PlayerMoveinFixedUpdate.lastTime = Time.time;
                        PlayerMoveinFixedUpdate.lastPos = playerPos.position;
                        sliderObj.SetActive(true);

                        WideCam = GameObject.Find("Cam").transform.GetChild(0).gameObject;
                    }
                    break;
                case State.FirstAction:
                    WideCam.SetActive(true);
                    yield return new WaitForSeconds(3f);
                    walls[0].SetActive(true);
                    walls[1].SetActive(true);
                    yield return new WaitForSeconds(3f);
                    WideCam.SetActive(false);
                    walls[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    walls[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    PlayerMoveinFixedUpdate.moveFlag = true;
                    _curState = State.Idle;
                    break;
                case State.Idle:
                    if (Vector3.Distance(transform.position, playerPos.position) < 15)
                    {
                        nextAttackNum = Random.Range(0, 3);
                        if (nextAttackNum == curAttackNum)
                            nextAttackNum = Random.Range(0, 3);
                        curAttackNum = nextAttackNum;
                        switch (curAttackNum)
                        {
                            case 0:
                                _curState = State.Earthquake;
                                break;
                            case 1:
                                _curState = State.Cry;
                                break;
                            case 2:
                                _curState = State.Dash;
                                break;
                        }
                    }
                    else
                    {
                        nextAttackNum = Random.Range(0, 1);
                        if (nextAttackNum == curAttackNum)
                            nextAttackNum = Random.Range(0, 1);
                        curAttackNum = nextAttackNum;
                        switch (nextAttackNum)
                        {
                            case 0:
                                _curState = State.Dash;
                                break;
                            case 1:
                                _curState = State.Tornado;
                                break;
                        }
                    }
                    break;
                // if (Input.GetKeyDown(KeyCode.E))
                //     _curState = State.Earthquake;
                // if (Input.GetKeyDown(KeyCode.Q))
                //     _curState = State.Dash;
                // if (Input.GetKeyDown(KeyCode.C))
                //     _curState = State.Cry;
                // if (Input.GetKeyDown(KeyCode.T))
                //     _curState = State.Tornado;
                // break;
                case State.Earthquake:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.blue));
                    rigid.AddForce(Vector3.up * 400);
                    yield return new WaitUntil(() => rigid.velocity.y < 0);

                    rigid.gravityScale = 50f;

                    yield return new WaitUntil(() => rigid.velocity.y == 0);

                    noise.m_AmplitudeGain = 5;
                    noise.m_FrequencyGain = 1;

                    if (GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity.y == 0)
                    {
                        PlayerMoveinFixedUpdate.moveFlag = false;
                        PlayerMoveinFixedUpdate.stun = true;
                        PlayerMoveinFixedUpdate.lastTime = Time.time;
                        PlayerMoveinFixedUpdate.lastPos = playerPos.position;
                    }
                    yield return new WaitForSeconds(1f);
                    noise.m_AmplitudeGain = 0;
                    noise.m_FrequencyGain = 0;
                    rigid.gravityScale = 1;

                    _curState = State.Idle;
                    yield return new WaitForSeconds(1f);
                    break;
                case State.Tornado:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.green));
                    for (int i = 0; i < 100; i++)
                    {
                        float speed;
                        if (GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity.y == 0)
                            speed = 0.5f;
                        else
                            speed = 0.1f;
                        transform.Rotate(0, 20, 0);
                        if (playerPos.position.x - transform.position.x >= 0)
                            transform.position += Vector3.right * speed;
                        else
                            transform.position += Vector3.left * speed;
                        yield return new WaitForSeconds(0.02f);
                    }
                    _curState = State.Idle;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    yield return new WaitForSeconds(1f);
                    break;
                case State.Dash:
                    yield return StartCoroutine(Blink(gameObject));
                    flag = false;
                    yield return new WaitForSeconds(1f);
                    rigid.AddForce(new Vector3(playerPos.position.x - transform.position.x, 0, 0).normalized * 60f, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.3f);
                    rigid.velocity = Vector3.zero;
                    _curState = State.Idle;
                    yield return new WaitForSeconds(1f);
                    break;
                case State.Cry:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.red));
                    anime.SetBool("isCry", true);
                    StartCoroutine(CryRange());
                    if (Vector3.Distance(playerPos.position, transform.position) <= 10)
                    {
                        Rigidbody2D rigid2d = playerPos.GetComponent<Rigidbody2D>();
                        PlayerMoveinFixedUpdate.flag = true;
                        Vector3 direction = (playerPos.position - transform.position).normalized;
                        rigid2d.AddForce(direction * 50f, ForceMode2D.Impulse);
                        yield return new WaitUntil(() => Vector3.Distance(playerPos.position, transform.position) > 10);
                        PlayerMoveinFixedUpdate.flag = false;
                    }
                    yield return new WaitUntil(() => !PlayerMoveinFixedUpdate.flag);
                    yield return new WaitUntil(() => anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f);
                    anime.SetBool("isCry", false);
                    _curState = State.Idle;
                    yield return new WaitForSeconds(1f);
                    break;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }
    IEnumerator Phase2()
    {
        phase_2_is_running = true;
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (GameObject.Find("Player") != null)
                playerPos = GameObject.Find("Player").transform;
            transform.localScale = new Vector3(-Character_Direction(playerPos, transform), 1, 1);
            switch (_curState)
            {
                case State.Idle:
                    nextAttackNum = Random.Range(0, 4);
                    if (nextAttackNum == curAttackNum)
                        nextAttackNum = Random.Range(0, 4);
                    curAttackNum = nextAttackNum;
                    switch (curAttackNum)
                    {
                        case 0:
                            _curState = State.Earthquake;
                            break;
                        case 1:
                            _curState = State.Path;
                            break;
                        case 2:
                            _curState = State.Dash;
                            break;
                        case 3:
                            _curState = State.Throw;
                            break;
                    }
                    break;
                case State.Earthquake:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.blue));
                    yield return new WaitForSeconds(0.5f);
                    transform.position = playerPos.position + Vector3.up * 40;
                    yield return new WaitUntil(() => transform.position.y >= playerPos.position.y + 30);
                    shadow.SetActive(true);
                    shadow.transform.position = new Vector3(playerPos.position.x, shadow.transform.position.y, -1);
                    yield return new WaitForSeconds(1f);
                    rigid.gravityScale = 30f;
                    yield return new WaitUntil(() => rigid.velocity.y == 0 || rigid.gravityScale == 1f);
                    noise.m_AmplitudeGain = 5;
                    noise.m_FrequencyGain = 1;
                    shadow.SetActive(false);
                    //hitRange.SetActive(true);
                    //hitRange.transform.position = new Vector3(shadow.transform.position.x, hitRange.transform.position.y, -1);
                    //Invoke("HitRange", 0.4f);
                    rigid.gravityScale = 1f;
                    if (Vector3.Distance(transform.position, playerPos.position) <= 5)
                    {
                        PlayerMoveinFixedUpdate.flag = true;
                        playerPos.GetComponent<Rigidbody2D>().AddForce((playerPos.position - transform.position).normalized * 20f, ForceMode2D.Impulse);
                        Invoke("playerFlag", 0.5f);
                    }
                    yield return new WaitForSeconds(1.5f);
                    noise.m_AmplitudeGain = 0;
                    noise.m_FrequencyGain = 0;
                    _curState = State.Idle;
                    break;
                case State.Dash:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.white));
                    yield return new WaitForSeconds(1f);
                    flag = false;
                    rigid.AddForce(new Vector3(playerPos.position.x - transform.position.x, 0, 0).normalized * 60f, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.3f);
                    rigid.velocity = Vector3.zero;
                    _curState = State.Idle;
                    yield return new WaitForSeconds(1f);
                    break;
                case State.Path:
                    lastPlayerPos = playerPos.position;
                    for (int i = 0; i < 10; i++)
                    {
                        Instantiate(path, new Vector3(lastPlayerPos.x + 5 * i, 180, 0), Quaternion.identity);
                        Instantiate(path, new Vector3(lastPlayerPos.x + 5 * -i, 180, 0), Quaternion.identity);
                        yield return new WaitForSeconds(0.42f);
                    }
                    _curState = State.Idle;
                    break;
                case State.Throw:
                    yield return StartCoroutine(Functions.Blink_Color(gameObject, Color.blue));
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject obj = Instantiate(stone, transform.position + new Vector3(0.5f + Character_Direction(playerPos, transform) * 1.5f, 2f, 0), Quaternion.identity);
                        obj.GetComponent<Rigidbody2D>().AddForce(new Vector3(playerPos.position.x - transform.position.x, 0, 0).normalized * 30, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.7f);
                    }
                    _curState = State.Idle;
                    break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    void HitRange()
    {
        hitRange.SetActive(false);
    }
    int Character_Direction(Transform target, Transform myself)
    {
        if (target.position.x - myself.position.x > 0)
            return 1;
        else if (target.position.x - myself.position.x < 0)
            return -1;
        return 0;
    }
    IEnumerator Blink(GameObject obj)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 2; i++)
        {
            sprite.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.2f);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon") || other.CompareTag("Stone"))
        {
            hp--;
            slider.value = hp / monsterInfo.hp;
            if (hp == 10)
            {
                StopCoroutine(phase_1);
                phase_1_is_running = false;
                StartCoroutine(phase_2);
                PlayerMoveinFixedUpdate.flag = true;
                playerPos.GetComponent<Rigidbody2D>().AddForce(new Vector3(playerPos.position.x - transform.position.x, 0, 0).normalized * 40f, ForceMode2D.Impulse);
                Invoke("playerFlag", 0.5f);
            }
            else if (hp <= 0)
            {
                if (noise.m_AmplitudeGain != 0)
                {
                    noise.m_AmplitudeGain = 0;
                    noise.m_FrequencyGain = 0;
                }
                walls[0].SetActive(false);
                walls[1].SetActive(false);
                sliderObj.SetActive(false);
                shadow.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
    void playerFlag()
    {
        PlayerMoveinFixedUpdate.flag = false;
    }
    IEnumerator CryRange()
    {
        while (currentRadius < maxRadius)
        {
            currentRadius += expandSpeed; // 원의 반지름을 점차 증가
            CreatePoints(currentRadius);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        currentRadius = 0;
        CreatePoints(currentRadius);
    }
}
