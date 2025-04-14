using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase :  IEnemyState
{

    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Chase State");
        aiBase.agent.speed = 6;
        aiBase.agent.Resume();
        aiBase.agent.isStopped = false;
    }
    public void Update(AIBase aiBase)
    {
        
      
        Debug.Log("AIChasing");

        aiBase.transform.LookAt(aiBase.player);
        aiBase.agent.SetDestination(aiBase.player.position);

   


    }
    public void Exit(AIBase aiBase)
    {

        Debug.Log("Exiting Chase State");
    }
}
