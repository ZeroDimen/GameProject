using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Damaged_State : IMonsterState
{
    Animator anime;
    float animeTime;
    public Spider_Damaged_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isDamaged", true);
        _monster.GetComponent<Spider>().hp--;
    }
    public override void StateUpdate()
    {

    }
    public override void StateExit()
    {
        anime.SetBool("isDamaged", false);
        if (Vector3.Distance(GameObject.Find("Player").transform.position, _monster.transform.position) <=
        _monster.GetComponent<Spider>().monsterInfo.attackRange)
            anime.SetBool("isAttack", true);
        else
            anime.SetBool("isChase", true);

    }
}
