using System.Runtime.CompilerServices;
using UnityEngine;

public class Goblin_Idle_State : IMonsterState
{
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitDown;
    float targetPos;
    float random;
    float moveSpeed;
    float lastTime;
    float timeInterval;
    bool isStop;
    bool modify;
    int layerMask;
    public Goblin_Idle_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        targetPos = _monster.transform.position.x;
        moveSpeed = _monster.GetComponent<Goblin>().monsterInfo.moveSpeed;
        isStop = false;
        modify = false;
        timeInterval = 3;

        layerMask = 1 << LayerMask.NameToLayer("Platform");
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position, Vector2.right, 2, layerMask);
        hitLeft = Physics2D.Raycast(_monster.transform.position, Vector2.left, 2, layerMask);
        hitDown = Physics2D.Raycast(_monster.transform.position, Vector2.down, 2, layerMask);

        if (hitRight.collider != null && !modify)
            Set_Random(-3, -10);
        if (hitLeft.collider != null && !modify)
            Set_Random(4, 10);
        if (hitDown.collider == null && !modify)
            Set_Random(4, 10);
        if (Mathf.Abs(_monster.transform.position.x - targetPos) <= 0.5f)
            Set_Random(-10, 11);

        if (isStop)
        {
            if (Time.time - lastTime >= timeInterval)
            {
                isStop = false;
                modify = false;
                _monster.transform.position += new Vector3(targetPos - _monster.transform.position.x, 0, 0).normalized * moveSpeed;
            }
        }
        else
            _monster.transform.position += new Vector3(targetPos - _monster.transform.position.x, 0, 0).normalized * moveSpeed;
    }
    public override void StateExit()
    {

    }
    void Set_Random(int a, int b)
    {
        do
        {
            random = Random.Range(a, b);
        } while (Mathf.Abs(random) < 4);
        targetPos = _monster.transform.position.x + random;
        isStop = true;
        modify = true;
        lastTime = Time.time;
    }
}
