using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CutSceneBattle_Monster : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Attack,
        Damaged,
        Dead
    }
    State _curstate;
    Transform playerPos;
    Animator anime;
    float hp = 4;
    void Start()
    {
        anime = GetComponent<Animator>();
        _curstate = State.Idle;
        StartCoroutine(Monster());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        if (_curstate == State.Idle)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 5);
        }
    }
    IEnumerator Monster()
    {
        while (true)
        {
            playerPos = GameObject.Find("Player_CutScene_Battle").transform;
            // if (playerPos.position.x - transform.position.x > 0)
            //     transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, playerPos.position.z);
            // else if (playerPos.position.x - transform.position.x < 0)
            //     transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, playerPos.position.z);
            if (playerPos.position.x - transform.position.x > 0)
                transform.localScale = new Vector3(0.4f, 0.4f, 0);
            else if (playerPos.position.x - transform.position.x < 0)
                transform.localScale = new Vector3(-0.4f, 0.4f, 0);

            switch (_curstate)
            {
                case State.Idle:
                    if (Vector3.Distance(transform.position, playerPos.position) <= 5)
                    {
                        _curstate = State.Walk;
                        anime.SetBool("isWalk", true);
                    }
                    break;
                case State.Walk:
                    transform.position += new Vector3(playerPos.position.x - transform.position.x, 0, 0).normalized * 0.05f;
                    if (Vector3.Distance(transform.position, playerPos.position) <= 2)
                    {
                        _curstate = State.Attack;
                        anime.SetBool("isWalk", false);
                        anime.SetBool("isAttack", true);
                    }
                    break;
                case State.Attack:
                    if (Vector3.Distance(transform.position, playerPos.position) > 2)
                    {
                        anime.SetBool("isWalk", true);
                        anime.SetBool("isAttack", false);
                        anime.Play("Walk");
                        _curstate = State.Walk;
                    }
                    break;
                case State.Damaged:
                    if (anime.GetCurrentAnimatorStateInfo(0).IsName("Damage") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
                    {
                        if (hp <= 0)
                        {
                            _curstate = State.Dead;
                            anime.SetBool("isDead", true);
                        }
                        else if (Vector3.Distance(transform.position, playerPos.position) <= 2)
                        {
                            _curstate = State.Attack;
                            anime.SetBool("isWalk", false);
                            anime.SetBool("isAttack", true);
                        }
                        else
                        {
                            anime.SetBool("isWalk", true);
                            anime.SetBool("isAttack", false);
                            _curstate = State.Walk;
                        }
                    }
                    break;
                case State.Dead:
                    if (anime.GetCurrentAnimatorStateInfo(0).IsName("Death") && anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            anime.SetBool("isWalk", false);
            anime.SetBool("isAttack", false);
            anime.Play("Damage");
            _curstate = State.Damaged;
            hp--;
        }
    }
}
