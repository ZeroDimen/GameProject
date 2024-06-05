using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class IMonsterState
{
    protected GameObject _monster;
    protected IMonsterState(GameObject monster)
    {
        _monster = monster;
    }
    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
    public int Player_Position(Transform player, Transform monster)
    {
        if (player.position.x - monster.position.x > 0)
            return 1;
        else if (player.position.x - monster.position.x < 0)
            return -1;
        return 0;
    }
    public bool canChase(Vector3 playerPos, Vector3 monsterPos, Vector3 firstPos)
    {
        if (playerPos.x - monsterPos.x > 0 && firstPos.x - monsterPos.x > 0)
            return true;
        else if (playerPos.x - monsterPos.x < 0 && firstPos.x - monsterPos.x < 0)
            return true;

        return false;
    }
}
public class MonsterStateMachine
{
    private IMonsterState _curState;

    public MonsterStateMachine(IMonsterState initState)
    {
        _curState = initState;
        ChangeState(_curState);
    }

    public void ChangeState(IMonsterState nextState)
    {
        if (_curState == nextState)
            return;

        if (_curState != null)
            _curState.StateExit();

        _curState = nextState;
        _curState.StateEnter();
    }

    public void UPdateStage()
    {
        if (_curState != null)
            _curState.StateUpdate();
    }
}
