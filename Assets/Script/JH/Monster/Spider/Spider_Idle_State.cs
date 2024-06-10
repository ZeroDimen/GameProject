using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Idle_State : IMonsterState
{
    Animator anime;
    Rigidbody2D rigid;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitDown;
    Vector3 firstPos;
    float targetPos;
    float random;
    float moveSpeed;
    float lastTime;
    float timeInterval;
    bool isStop;
    bool modify;
    int layerMask;
    public Spider_Idle_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        rigid = _monster.GetComponent<Rigidbody2D>();
        firstPos = _monster.GetComponent<Spider>().firstPos;
        targetPos = _monster.transform.position.x;
        moveSpeed = _monster.GetComponent<Spider>().monsterInfo.moveSpeed;
        isStop = false;
        modify = false;
        timeInterval = 3;
        //anime.SetBool("isIdle", true);
        layerMask = 1 << LayerMask.NameToLayer("Platform");
    }
    public override void StateUpdate()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }
        else
            anime.SetBool("isIdle", true);

        hitRight = Physics2D.Raycast(_monster.transform.position + Vector3.up * 2, Vector2.right, 2, layerMask);
        hitLeft = Physics2D.Raycast(_monster.transform.position + Vector3.up * 2, Vector2.left, 2, layerMask);
        hitDown = Physics2D.Raycast(_monster.transform.position + Vector3.up * 2, Vector2.down, 2, layerMask);

        if (hitRight.collider != null && !modify)
            Set_Random(-10, -3);
        if (hitLeft.collider != null && !modify)
            Set_Random(4, 10);
        if (hitDown.collider == null && !modify)
        {
            if (firstPos.x - _monster.transform.position.x > 0)
                Set_Random(4, 10);
            else
                Set_Random(-10, -4);
        }
        if (Mathf.Abs(_monster.transform.position.x - targetPos) <= 0.5f)
            Set_Random(-10, 11);

        if (isStop)
        {
            if (Time.time - lastTime >= timeInterval)
            {
                isStop = false;
                anime.SetBool("isMove", true);
                anime.SetBool("isIdle", false);
                modify = false;
                _monster.transform.position += new Vector3(targetPos - _monster.transform.position.x, 0, 0).normalized * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            _monster.transform.position += new Vector3(targetPos - _monster.transform.position.x, 0, 0).normalized * moveSpeed * Time.deltaTime;
        }
        if (targetPos - _monster.transform.position.x > 0)
            _monster.transform.localScale = new Vector3(1, 1, 1);
        else
            _monster.transform.localScale = new Vector3(-1, 1, 1);
    }
    public override void StateExit()
    {
        anime.SetBool("isIdle", false);
    }
    void Set_Random(int a, int b)
    {
        do
        {
            random = Random.Range(a, b);
        } while (Mathf.Abs(random) < 4);
        targetPos = _monster.transform.position.x + random;
        isStop = true;
        anime.SetBool("isMove", false);
        anime.SetBool("isIdle", true);
        modify = true;
        lastTime = Time.time;
    }
}
