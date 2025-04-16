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
    public Player playerScript;

    public LayerMask whatIsGround, whatIsPlayer;

    public float killcountdown = 2;
    public GameObject model;
    public GameObject additionalModel;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isDying;
    public bool isDead;
    public bool isAttacking;
    public bool onCooldown;
    public BoxCollider boxCollider;

    public UI ui;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public Transform centrePoint;
    public int range = 50;
    public Vector3 point;

    [Header("Detection Settings")]
    public float detectionRadius = 5f;   
    public bool playerDetected;
    [Header("Audio")]
    public Sound[] audioClips;
    public AudioSource audioSource;
    public AudioSource directionalWoo;


    private void Awake()
    {
  
        agent = GetComponent<NavMeshAgent>();

        ui = GameObject.Find("UI").GetComponent<UI>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        
    }

    private void Start()
    {
        player = null;
        isDead = false;

        StartCoroutine(WaitForPlayer());
    }
    public IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(2.3f);
          if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerScript = player.GetComponent<Player>();
            SetState(new EnemyState_Idle());
            Invoke("LocatePlayer", 1f);
        }
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
            killcountdown -= Time.fixedDeltaTime;
        }
        if (!isDying && killcountdown < 2)
        {
            killcountdown += Time.fixedDeltaTime;
        }
        if (killcountdown <= 0 && !isDead)
        {
            killcountdown = 2;
            isDying = false;
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
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !onCooldown && !isDead)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            SetState(new EnemyState_Attack());
            onCooldown = true; // Start cooldown to prevent rapid attacks
            StartCoroutine(AttackCooldown());
        }
    }

    public void PatrolPoints()
    {
         if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 1.0f);
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
    public void DeactivateEnemy()
    {
        
        StartCoroutine(DestroyEnemy());
    }
    public void AttackCool()
    {
        StartCoroutine(AttackCooldown());
    }
    public IEnumerator DestroyEnemy()
    {  yield return new WaitForSeconds(0.3f);
       animator.SetBool("Dead", false);
        yield return new WaitForSeconds(0.4f);
        ui.AddScore(1);
        gameObject.SetActive(false);
    }
        public IEnumerator AttackCooldown()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
        SetState(new EnemyState_Idle());
        boxCollider.enabled = false;
        onCooldown = true;
        yield return new WaitForSeconds(3f);
        onCooldown = false;
        boxCollider.enabled = true;
        isAttacking = false;
    }
        public void PlaySFX (string name)
    {
        Sound s = System.Array.Find(audioClips, sound => sound.name == name);
        if (s == null) return;
        audioSource.clip = s.clip;
        audioSource.Play();
    }
}
