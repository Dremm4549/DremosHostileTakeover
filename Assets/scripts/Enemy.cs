using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float EnemyHealth;
    public Transform playerPosition;
    public NavMeshAgent agent;
    public SpawnManager spawnManager;
    public Transform[] wayPoints;
    public Transform[] spawnPoints;
    public Transform hands;
    
    public int currentSpawnLocation;
    int waypointIndex;

    public float maxDistanceToPlayer = 5f;
    public float attackRange = 2;
    public float damage = 50;
    public float timePerHit;
    public float hpGainedPerRound = 0;

    public float timeOfLastAttack = 0;
    public float attackSpeed = 0.5f;
    public float attackDelay = 0.8f;
    [SerializeField] float zombieGroanMinTime;
    [SerializeField] float zombieGroanMaxTime;
    [SerializeField] float randZombieGroanTime;


    SphereCollider sc;

    [SerializeField] bool inTrigger;
    bool hasStopped = false;

    Animator animator;

    Vector3 target;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] zombieGroans;
    [SerializeField] AudioClip[] zombieAttacks;


    private void Start()
    {
        determineWayPointDestination();
        transform.LookAt(target);
        agent.SetDestination(target);
        animator = GetComponentInChildren<Animator>();
        sc = FindObjectOfType<SphereCollider>();
        InitalizeAudioClips();
        randZombieGroanTime = Random.Range(zombieGroanMinTime, zombieGroanMaxTime);
    }
   
    void Update()
    {       
        CheckDistanceToPlayer();

        PlayZombieGroan();

        //if (EnemyHealth <= 0)
        //{
        //    spawnManager.deductEnemiesInRound(1);
        //    animator.SetBool("isDead", true);
        //    agent.isStopped = true;
        //    Destroy(gameObject,4.5f);
        //}
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red);
    }

    public void takeDamage(float damage)
    {
        EnemyHealth -= damage;
        if(EnemyHealth <= 0)
        {
            spawnManager.deductEnemiesInRound(1);
            agent.isStopped = true;
            animator.SetBool("isDead", true);
            Destroy(gameObject,4.5f);
        }       
    }

    void determineWayPointDestination()
    {
        spawnManager = FindObjectOfType<SpawnManager>();

        for (int i = 0; i < spawnManager.spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnManager.spawnPoints[i].transform;
        }
        for (int i = 0; i < wayPoints.Length; i++)
        {

            wayPoints[i] = GameObject.Find("wayPoint (" + i + ")").transform;
        }

        currentSpawnLocation = spawnManager.randomSpawnIndex;
        

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            if (currentSpawnLocation == i)
            {
                target = wayPoints[i].position;
            }
        }            
    }

    void CheckDistanceToPlayer()
    {
        float currentDistance = Vector3.Distance(this.transform.position, playerPosition.position);
        //Debug.Log(currentDistance);
        //if(currentDistance <= maxDistanceToPlayer)
        if(inTrigger)
        {
            animator.SetFloat("speed", 0f, 0.3f, Time.deltaTime);

            if (!hasStopped)
            {
                agent.isStopped = true;
                hasStopped = true;
                timeOfLastAttack = Time.time;               
            }

            if (Time.time >= timeOfLastAttack + attackSpeed)
            {
                Debug.Log("cunnt");
                if(EnemyHealth > 0)
                {
                    AttackPlayer();
                }
                
            }
        }
        else
        {
            if (hasStopped || !hasStopped && EnemyHealth > 0)
            {
                hasStopped = false;
                //agent.isStopped = false;
                if(spawnManager.currentRound < 9)
                {
                    animator.SetFloat("speed", 0.5f, 0.3f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("speed", 1f, 0.3f, Time.deltaTime);
                    agent.speed = 1.3f;
                }
            }      
        }
    }

    void AttackPlayer()
    {
        RaycastHit hit;
        animator.SetTrigger("attack");

        timePerHit -= Time.deltaTime;
        if(timePerHit <= 0)
        {
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                if(hit.collider.tag == "Player")
                {              
                    
                    PlayerClass pc = FindObjectOfType<PlayerClass>();

                    pc.hurtPlayer(damage);
                    attackDelay = Time.time;
                    Debug.Log("hit");                                                                                       
                }             
            }
            timePerHit = 0.4f;
        }      
    }

    public void IncreaseEnemyHP(float newHp)
    {    
        EnemyHealth += newHp;  
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player")
        {
            inTrigger = true;
            //sc.enabled = false;
        }
    }

    void PlayZombieGroan()
    {
        

        randZombieGroanTime -= Time.deltaTime;

        if(randZombieGroanTime <= 0)
        {
            int selectedZombieGroan = Random.Range(0, zombieGroans.Length);
            source.PlayOneShot(zombieGroans[selectedZombieGroan]);
            randZombieGroanTime = Random.Range(zombieGroanMinTime, zombieGroanMaxTime);
            Debug.Log("NOW");
        }

    }

    void InitalizeAudioClips()
    {
        source = GetComponent<AudioSource>();
        //for(int i = 0; i < zombieGroans.Length; i++)
        //{
        //    if(zombieGroans[i] != null)
        //    {
        //        zombieGroans[i] = GetComponent<AudioClip>();
        //    }
        //}
    }

}
