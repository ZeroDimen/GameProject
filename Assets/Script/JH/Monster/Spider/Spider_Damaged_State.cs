using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Damaged_State : IMonsterState
{
    Animator anime;
    public Spider_Damaged_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isDamaged", true);
    }
    public override void StateUpdate()
    {

    }
    public override void StateExit()
    {
        anime.SetBool("isDamaged", false);
        if (_monster.GetComponent<Spider>().hp <= 0)
            anime.SetBool("isDeath", true);
        else if (Vector3.Distance(GameObject.Find("Player").transform.position, _monster.transform.position) <=
        _monster.GetComponent<Spider>().monsterInfo.attackRange)
            anime.SetBool("isAttack", true);
        else
            anime.SetBool("isChase", true);

    }
}
