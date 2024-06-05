using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin_Chase_State : IMonsterState
{
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitDown;
    RaycastHit2D hitRightPlatform;
    RaycastHit2D hitLeftPlatform;
    Vector3 firstPos;
    Transform playerPos;
    float goblinDistance;
    public Goblin_Chase_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        goblinDistance = _monster.GetComponent<Goblin>().monsterInfo.monsterDistance;
        firstPos = _monster.GetComponent<Goblin>().firstPos;
        playerPos = GameObject.Find("Player").transform;
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position + Vector3.right, Vector2.right, goblinDistance, 1 << LayerMask.NameToLayer("Goblin"));
        hitLeft = Physics2D.Raycast(_monster.transform.position + Vector3.left, Vector2.left, goblinDistance, 1 << LayerMask.NameToLayer("Goblin"));
        hitDown = Physics2D.Raycast(_monster.transform.position, Vector3.down, 2f, 1 << LayerMask.NameToLayer("Platform"));

        hitRightPlatform = Physics2D.Raycast(_monster.transform.position, Vector2.right, 2f, 1 << LayerMask.NameToLayer("Platform"));
        hitLeftPlatform = Physics2D.Raycast(_monster.transform.position, Vector2.left, 2f, 1 << LayerMask.NameToLayer("Platform"));

        if ((hitRightPlatform.collider == null && hitLeftPlatform.collider == null) || canChase(playerPos.position, _monster.transform.position, firstPos))
        {
            if (hitDown.collider != null || canChase(playerPos.position, _monster.transform.position, firstPos))
            {
                if (Player_Position(playerPos, _monster.transform) > 0 && hitRight.collider == null)
                    _monster.transform.position += Vector3.right * Time.deltaTime * _monster.GetComponent<Goblin>().monsterInfo.moveSpeed;
                if (Player_Position(playerPos, _monster.transform) < 0 && hitLeft.collider == null)
                    _monster.transform.position += Vector3.left * Time.deltaTime * _monster.GetComponent<Goblin>().monsterInfo.moveSpeed;
            }
        }
        if (Player_Position(playerPos, _monster.transform) > 0)
            _monster.transform.localScale = new Vector3(1, 1, 1);
        else
            _monster.transform.localScale = new Vector3(-1, 1, 1);
    }
    public override void StateExit()
    {

    }
}
