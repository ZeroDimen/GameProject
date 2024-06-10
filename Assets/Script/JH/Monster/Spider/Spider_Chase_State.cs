using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Spider_Chase_State : IMonsterState
{
    Animator anime;

    RaycastHit2D hitDown;

    Transform playerPos;
    Vector3 firstPos;
    float goblinDistance;
    public Spider_Chase_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        goblinDistance = _monster.GetComponent<Spider>().monsterInfo.monsterDistance;
        playerPos = GameObject.Find("Player").transform;
        firstPos = _monster.GetComponent<Spider>().firstPos;
    }
    public override void StateUpdate()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }
        else
        {
            anime.SetBool("isMove", true);
            anime.SetBool("isIdle", false);
        }

        hitDown = Physics2D.Raycast(_monster.transform.position + Vector3.up * 2, Vector3.down, 2f, 1 << LayerMask.NameToLayer("Platform"));

        if (hitDown.collider != null || canChase(playerPos.position, _monster.transform.position, firstPos))
        {
            if (Player_Position(playerPos, _monster.transform) > 0)
                _monster.transform.position += Vector3.right * Time.deltaTime * _monster.GetComponent<Spider>().monsterInfo.moveSpeed;

            if (Player_Position(playerPos, _monster.transform) < 0)
                _monster.transform.position += Vector3.left * Time.deltaTime * _monster.GetComponent<Spider>().monsterInfo.moveSpeed;

            if (Player_Position(playerPos, _monster.transform) > 0)
                _monster.transform.localScale = new Vector3(1, 1, 1);
            else
                _monster.transform.localScale = new Vector3(-1, 1, 1);
        }


    }
    public override void StateExit()
    {
        anime.SetBool("isMove", false);
    }
}
