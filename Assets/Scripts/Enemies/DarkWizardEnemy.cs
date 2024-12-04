using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkWizardEnemy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private DarkWizardMagicSystem magicSystem;
    private HealthComponent health;

    // Modificable variables (on engine)
    [SerializeField] private float castSpellDelay = 3; // Seconds
    [SerializeField] private Transform target;
    [SerializeField] private float visionRadius = 8;
    // When the target is at 3 units or less of distance from the mage, it flees away
    [SerializeField] private float scareRadius = 2;
    [SerializeField] private float combatRadius = 6;

    // Attack variables
    private float castSpellTimer = 0;

    // Patrol variables
    [SerializeField] private float patrolRange = 4;

    private Vector3 patrolDestination;

    // State variables
    private bool m_attacking = false;
    private bool m_patroling = false;
    private bool m_scared = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        magicSystem = GetComponent<DarkWizardMagicSystem>();
        health = GetComponent<HealthComponent>();

        m_patroling = true;
    }

    // Update is called once per frame
    void Update()
    {

        castSpellTimer += Time.deltaTime;

        float distance = Vector3.Distance(agent.transform.position, target.position);

        
        
    }

    private void Attack()
    {
        
        
    }

    private void Chase()
    {
        agent.SetDestination(target.position);
    }

    private void Patrol()
    {
        if (agent.transform.position.Equals(patrolDestination))
        {
            if (RandomPoint(agent.transform.position, patrolRange, out patrolDestination))
            {
                agent.SetDestination(patrolDestination);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; // Random point in a shpere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    private void RunAway()
    {

    }

    public void SlowDown()
    {

    }

}
