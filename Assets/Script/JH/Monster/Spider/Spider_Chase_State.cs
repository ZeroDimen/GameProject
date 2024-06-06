using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Spider_Chase_State : IMonsterState
{
    Animator anime;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    RaycastHit2D hitDown;
    RaycastHit2D hitRightPlatform;
    RaycastHit2D hitLeftPlatform;

    Transform playerPos;
    Vector3 firstPos;
    float goblinDistance;
    public Spider_Chase_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isChase", true);
        goblinDistance = _monster.GetComponent<Spider>().monsterInfo.monsterDistance;
        playerPos = GameObject.Find("Player").transform;
        firstPos = _monster.GetComponent<Spider>().firstPos;
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position + new Vector3(2, 2, 0), Vector2.right, goblinDistance, 1 << LayerMask.NameToLayer("Spider"));
        hitLeft = Physics2D.Raycast(_monster.transform.position + new Vector3(-2, 2, 0), Vector2.left, goblinDistance, 1 << LayerMask.NameToLayer("Spider"));
        hitDown = Physics2D.Raycast(_monster.transform.position + Vector3.up * 2, Vector3.down, 2f, 1 << LayerMask.NameToLayer("Platform"));

        hitRightPlatform = Physics2D.Raycast(_monster.transform.position + new Vector3(2, 2, 0), Vector2.right, 0.5f, 1 << LayerMask.NameToLayer("Platform"));
        hitLeftPlatform = Physics2D.Raycast(_monster.transform.position + new Vector3(-2, 2, 0), Vector2.left, 0.5f, 1 << LayerMask.NameToLayer("Platform"));

        anime.SetBool("isMove", false);

        if ((hitRightPlatform.collider == null && hitLeftPlatform.collider == null) || canChase(playerPos.position, _monster.transform.position, firstPos))
        {
            if (hitDown.collider != null || canChase(playerPos.position, _monster.transform.position, firstPos))
            {
                if (Player_Position(playerPos, _monster.transform) > 0 && hitRight.collider == null)
                    _monster.transform.position += Vector3.right * Time.deltaTime * _monster.GetComponent<Spider>().monsterInfo.moveSpeed;

                if (Player_Position(playerPos, _monster.transform) < 0 && hitLeft.collider == null)
                    _monster.transform.position += Vector3.left * Time.deltaTime * _monster.GetComponent<Spider>().monsterInfo.moveSpeed;

                anime.SetBool("isMove", true);
            }
        }

        if (Player_Position(playerPos, _monster.transform) > 0)
            _monster.transform.localScale = new Vector3(1, 1, 1);
        else
            _monster.transform.localScale = new Vector3(-1, 1, 1);
    }
    public override void StateExit()
    {
        anime.SetBool("isChase", false);
        if (Vector3.Distance(playerPos.position, _monster.transform.position) <=
        _monster.GetComponent<Spider>().monsterInfo.attackRange)
            anime.SetBool("isAttack", true);
    }
}
