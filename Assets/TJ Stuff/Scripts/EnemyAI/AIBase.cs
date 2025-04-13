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

    public UI ui;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public Transform centrePoint;
    public int range;
    public Vector3 point;

    [Header("Detection Settings")]
    public float detectionRadius = 5f;   
    public bool playerDetected;

    private void Awake()
    {
  
        agent = GetComponent<NavMeshAgent>();

        ui = GameObject.Find("UI").GetComponent<UI>();
    }

    private void Start()
    {
        player = null;

        StartCoroutine(WaitForPlayer());
    }
    public IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(2f);
          if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            SetState(new EnemyState_Idle());
            Invoke("LocatePlayer", 1f);
        }
    }
    private void OnEnable()
    {
     
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
        if (player != null)
        {
            DetectPlayerInRadius();
        
        }
       
    }

    public void DetectPlayerInRadius()
    {
        playerDetected = Physics.CheckSphere(transform.position, detectionRadius, whatIsPlayer);
        

        Debug.Log("Player detected within radius!");
    
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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

    public void PatrolPoints()
    {
         if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }
    }
    
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        if (NavMesh.SamplePosition(center + Random.insideUnitSphere * range, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
    
        result = Vector3.zero;
        return false;
    }
}
