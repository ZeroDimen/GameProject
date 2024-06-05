using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Attack_State : IMonsterState
{
    Animator anime;
    int _hp;
    public Spider_Attack_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        _hp = _monster.GetComponent<Spider>().hp;
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isAttack", true);
    }
    public override void StateUpdate()
    {

    }
    public override void StateExit()
    {
        anime.SetBool("isAttack", false);
        if (_monster.GetComponent<Spider>().hp != _hp)
            anime.SetBool("isDamaged", true);
    }
}
