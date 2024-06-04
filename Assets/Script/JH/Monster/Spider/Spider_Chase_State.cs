using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Spider_Chase_State : IMonsterState
{
    Animator anime;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    Transform playerPos;
    float goblinDistance;
    public Spider_Chase_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isChase", true);
        goblinDistance = _monster.GetComponent<Spider>().monsterInfo.monsterDistance;
        playerPos = GameObject.Find("Player").transform;
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position + Vector3.right, Vector2.right, goblinDistance, 1 << LayerMask.NameToLayer("Spider"));
        hitLeft = Physics2D.Raycast(_monster.transform.position + Vector3.left, Vector2.left, goblinDistance, 1 << LayerMask.NameToLayer("Spider"));

        if (Player_Position(playerPos, _monster.transform) > 0 && hitRight.collider == null)
        {
            _monster.transform.position += Vector3.right * Time.deltaTime * 4f;
            _monster.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Player_Position(playerPos, _monster.transform) < 0 && hitLeft.collider == null)
        {
            _monster.transform.position += Vector3.left * Time.deltaTime * 4f;
            _monster.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public override void StateExit()
    {
        anime.SetBool("isChase", false);
        if (Vector3.Distance(playerPos.position, _monster.transform.position) <=
        _monster.GetComponent<Spider>().monsterInfo.attackRange)
            anime.SetBool("isAttack", true);
    }
    int Player_Position(Transform player, Transform monster)
    {
        if (player.position.x - monster.position.x > 0)
            return 1;
        else if (player.position.x - monster.position.x < 0)
            return -1;
        return 0;
    }
}
