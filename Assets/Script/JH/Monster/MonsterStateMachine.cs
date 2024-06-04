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
