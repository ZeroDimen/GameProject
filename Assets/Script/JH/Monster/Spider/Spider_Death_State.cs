using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Death_State : IMonsterState
{
    Animator anime;
    public Spider_Death_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isDeath", true);
    }
    public override void StateUpdate()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Death") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            _monster.GetComponent<Spider>().isdead = true;
    }
    public override void StateExit()
    {

    }
}
