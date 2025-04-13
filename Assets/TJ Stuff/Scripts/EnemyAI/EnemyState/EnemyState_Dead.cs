using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Dead : IEnemyState
{
    public void Enter(AIBase aiBase)
    {
        Debug.Log("Entering Dead State");
        aiBase.agent.speed = 0;

        aiBase.model.SetActive(false);
        aiBase.additionalModel.SetActive(false);
        aiBase.animator.SetBool("Dead", true);
        aiBase.agent.Stop();
        aiBase.DeactivateEnemy();
    }

    public void Update(AIBase aiBase)
    {


 
    }

    public void Exit(AIBase aIBase)
    {
        Debug.Log("Exiting Death State");
    }

}
