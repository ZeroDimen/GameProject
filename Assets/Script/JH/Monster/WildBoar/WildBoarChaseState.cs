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
        if (playerPos.x - _monster.transform.position.x < 0)
            isRight = false;
        else
            isRight = true;
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position, Vector2.right, 3f, 1 << LayerMask.NameToLayer("Platform"));
        hitLeft = Physics2D.Raycast(_monster.transform.position, Vector2.left, 3f, 1 << LayerMask.NameToLayer("Platform"));

        if ((hitRight && isRight) || (hitLeft && !isRight))
        {
            _monster.GetComponent<WildBoar>().isStun = true;
            return;
        }

        if (isRight)
            _monster.transform.position += Vector3.right * Time.deltaTime * _monster.GetComponent<WildBoar>().monsterInfo.moveSpeed;
        else
            _monster.transform.position += Vector3.left * Time.deltaTime * _monster.GetComponent<WildBoar>().monsterInfo.moveSpeed;
    }
    public override void StateExit()
    {

    }
}
