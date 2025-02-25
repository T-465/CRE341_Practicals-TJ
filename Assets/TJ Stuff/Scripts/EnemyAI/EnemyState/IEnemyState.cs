using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    //Mothman State Interface

    void Enter(AIBase aiBase); // Called when entering the state

    void Update(AIBase aiBase); // Called every frame in this state
    void Exit(AIBase aiBase); // Called when exiting the state
}
    