using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : IEnemyState
{
    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Attack State");
        aiBase.playerScript.TakeDamage(4);
        aiBase.AttackCool();

    }

    public void Update(AIBase aIBase)
    {

        Debug.Log("AI Attacking");

  


    }
    public void Exit(AIBase aiBase)
    {

        Debug.Log("Exiting Attack State");

    }


}
