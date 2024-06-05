using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Death_State : IMonsterState
{
    Animator anime;
    float animeTime;
    public Spider_Death_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.Play("Death");
    }
    public override void StateUpdate()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Death") == true)
        {
            animeTime = anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animeTime >= 1f)
                _monster.GetComponent<Spider>().isdead = true;
        }
    }
    public override void StateExit()
    {

    }
}
