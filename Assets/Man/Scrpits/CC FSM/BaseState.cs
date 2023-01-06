using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    // public StateMachine stateMachine;

    // public BaseState(string name, StateMachine stateMachine)
    // {
    //     this.name = name;
    //     this.stateMachine = stateMachine;
    // }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }

    /*protected void MoveOrFire(EnemyFSM fsm)
    {
        if (fsm.NotMoving)
        {
            var playerPing = fsm.enemyController.CanSeePlayer();
            // Debug.Log(playerPing);
            if (playerPing > 0f)
            {
                if (playerPing < fsm.range)
                {
                    stateMachine.ChangeState(fsm.firingState);
                }
                else
                {
                    stateMachine.ChangeState(fsm.movingState);
                }
            }
        }
        else
        {
            // stateMachine.ChangeState(fsm.idleState);
        }
    }*/
}