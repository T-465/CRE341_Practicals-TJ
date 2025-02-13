using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : IEnemyState
{
    // Mothmans attack state that implements a jumpscare, lowers health and begins an attack delay
    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Attack State");


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
