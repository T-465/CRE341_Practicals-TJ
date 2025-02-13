using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;


public class AIBase : MonoBehaviour
{

    public NavMeshAgent agent;


    private IEnemyState currentState;
    public Transform player;


    public LayerMask whatIsGround, whatIsPlayer;











    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();



    }
    private void Start()
    {

        SetState(new EnemyState_Tele());
        Invoke("LocatePlayer", 1f);
    }
    private void OnEnable()
    {
        SetState(new EnemyState_Tele());

    }
    private void OnDisable()
    {

    }


    private void Update()
    {
        currentState?.Update(this);

    }
    public void SetState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }
    public string GetCurrentStateName()
    {
        return currentState != null ? currentState.GetType().Name.Replace("AI", "") : "No State";
    }

    private void LocatePlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }


    }
}
