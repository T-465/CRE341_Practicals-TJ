using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;


public class AIBase : MonoBehaviour
{

    public NavMeshAgent agent;


    private IEnemyState currentState;
    public Transform player;


    public LayerMask whatIsGround, whatIsPlayer;

    public float killcountdown = 2;
    public bool isDying;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();



    }
    private void Start()
    {

        SetState(new EnemyState_Chase());
        Invoke("LocatePlayer", 1f);
    }
    private void OnEnable()
    {
        SetState(new EnemyState_Chase());

    }
    private void OnDisable()
    {

    }


    private void Update()
    {
        currentState?.Update(this);

        if (killcountdown >= 2)
        {
            killcountdown = 2;
        }
        if (isDying)
        {
            killcountdown -= Time.deltaTime;
        }
        if (!isDying && killcountdown < 2)
        {
            killcountdown += Time.deltaTime;
        }

if (killcountdown <= 0)
        {
            SetState(new EnemyState_Dead());
        }
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


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flashlight")
        {
            isDying = true;
        }
    }
        public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Flashlight")
        {
            killcountdown = 0.5f;
            isDying = false;
            
        }
    }
}
