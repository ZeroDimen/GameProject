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
        anime.SetBool("isDamage", true);
    }
    public override void StateUpdate()
    {

    }
    public override void StateExit()
    {
        anime.SetBool("isDamage", false);
    }
}
