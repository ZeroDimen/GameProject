using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoarChaseState : IMonsterState
{
    Vector3 playerPos;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    bool isRight;
    public WildBoarChaseState(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        playerPos = GameObject.Find("Player").transform.position;

        // 플레이어가 오른쪽에 있는지 왼쪽에 있는지 체크
        if (playerPos.x - _monster.transform.position.x < 0)
            isRight = false;
        else
            isRight = true;
    }
    public override void StateUpdate()
    {
        // 오른쪽, 왼쪽에 ray를 쏴서 벽이 있는지 체크
        hitRight = Physics2D.Raycast(_monster.transform.position, Vector2.right, 3f, 1 << LayerMask.NameToLayer("Platform"));
        hitLeft = Physics2D.Raycast(_monster.transform.position, Vector2.left, 3f, 1 << LayerMask.NameToLayer("Platform"));

        // 충돌체가 있으면 스턴
        if ((hitRight && isRight) || (hitLeft && !isRight))
        {
            _monster.GetComponent<WildBoar>().isStun = true;
            return;
        }

        // 이동
        if (isRight)
            _monster.transform.position += Vector3.right * Time.deltaTime * _monster.GetComponent<WildBoar>().monsterInfo.moveSpeed;
        else
            _monster.transform.position += Vector3.left * Time.deltaTime * _monster.GetComponent<WildBoar>().monsterInfo.moveSpeed;
    }
    public override void StateExit()
    {

    }
}
