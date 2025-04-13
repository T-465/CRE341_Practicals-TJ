using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Idle : IEnemyState
{
   

    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Idle State");

    }

    public void Update(AIBase aiBase)
    {
        aiBase.agent.speed = 4;
        

        aiBase.PatrolPoints();

        if (aiBase.player == null) return;

        if (aiBase.playerDetected && aiBase.onCooldown == false)
        {
            aiBase.SetState(new EnemyState_Chase());
        }
   

        Debug.Log("AIIdle");
    }

    public void Exit(AIBase aIBase)
    {
        Debug.Log("Exiting Idle State");
    }

}
