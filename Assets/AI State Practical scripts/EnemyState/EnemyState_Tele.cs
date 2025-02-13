using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Tele : IEnemyState
{
   

    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Tele State");

    }

    public void Update(AIBase aiBase)
    {
        aiBase.agent.speed = 1;
        aiBase.agent.Stop();

        if (aiBase.player == null) return;

        if (Input.GetKey(KeyCode.R))
        {
            aiBase.SetState(new EnemyState_Chase());
        }

        Debug.Log("AITele");
    }

    public void Exit(AIBase aIBase)
    {
        Debug.Log("Exiting Idle State");
    }

}
