using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin_Chase_State : IMonsterState
{
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    Transform playerPos;
    float goblinDistance;
    public Goblin_Chase_State(GameObject monster) : base(monster) { }
    public override void StateEnter()
    {
        goblinDistance = _monster.GetComponent<Goblin>().monsterInfo.monsterDistance;
        playerPos = GameObject.Find("Player").transform;
    }
    public override void StateUpdate()
    {
        hitRight = Physics2D.Raycast(_monster.transform.position + Vector3.right, Vector2.right, goblinDistance, 1 << LayerMask.NameToLayer("Goblin"));
        hitLeft = Physics2D.Raycast(_monster.transform.position + Vector3.left, Vector2.left, goblinDistance, 1 << LayerMask.NameToLayer("Goblin"));

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
