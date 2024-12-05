using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkWizardEnemy : MonoBehaviour, Slowable
{
    private Animator animator;
    private NavMeshAgent agent;
    private DarkWizardMagicSystem magicSystem;
    private HealthComponent health;

    // Modifiable variables (on engine)
    [SerializeField] private float castSpellDelay = 3; // Seconds
    [SerializeField] private Transform target;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float chaseSpeed = 4;

    // Perception variables
    [SerializeField] private float visionRadius = 8;
    [SerializeField] private float scareRadius = 2;
    [SerializeField] private float combatRadius = 6;

    // Attack variables
    private float castSpellTimer = 0;

    // Patrol variables


    // State variables

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        magicSystem = GetComponent<DarkWizardMagicSystem>();
        health = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        castSpellTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= combatRadius)
            Attack();
        else if (distance <= visionRadius)
            Chase();
        else
            Patrol();

    }

    void Patrol()
    {
        Debug.Log("Patrol");
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        if (castSpellTimer >= castSpellDelay)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(target.position);
            // Calculate rotation
            Vector3 directorVector = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(directorVector);

            magicSystem.CastFireBall(rot);
            castSpellTimer = 0;
        }
    }

    public void SlowDown(float downS, float duration)
    {
        
    }
}
