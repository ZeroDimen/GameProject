using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Attack_State : IMonsterState
{
    Animator anime;
    AnimatorStateInfo stateInfo;
    public Spider_Attack_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        anime = _monster.GetComponent<Animator>();
        anime.SetBool("isAttack", true);
    }
    public override void StateUpdate()
    {
        stateInfo = anime.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && stateInfo.IsName("Attack"))
        {
            anime.SetBool("isAttack", false);
            anime.SetBool("isIdle", true);
        }
        if (stateInfo.normalizedTime > 0.5f && stateInfo.IsName("Idle"))
        {
            anime.SetBool("isAttack", true);
            anime.SetBool("isIdle", false);
        }
    }
    public override void StateExit()
    {
        anime.SetBool("isAttack", false);
    }
}
