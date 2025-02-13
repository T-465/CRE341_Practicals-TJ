using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Chase :  IEnemyState
{
    // Mothmans Chase state in which he will chase the player if their flashlight is on and will shake the  players camera as it approaches as a warning

    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Chase State");
        aiBase.agent.speed = 15;
    }
    public void Update(AIBase aiBase)
    {
        
      
        Debug.Log("AIChasing");

        aiBase.transform.LookAt(aiBase.player);
        aiBase.agent.SetDestination(aiBase.player.position);

        if (Input.GetKey(KeyCode.Space))
        {
            aiBase.SetState(new EnemyState_Tele());
        }

    }
    public void Exit(AIBase aiBase)
    {

        Debug.Log("Exiting Chase State");
    }
}
