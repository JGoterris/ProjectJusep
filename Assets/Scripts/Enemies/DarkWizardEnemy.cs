using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkWizardEnemy : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private DarkWizardMagicSystem magicSystem;
    [SerializeField] private float castSpellDelay = 3; // Seconds
    public Transform target;
    [SerializeField] private float visionRadius = 8;

    // Attack variables
    private float castSpellTimer = 0;
    private float lastSpellTimer = 0;

    // Patrol variables

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        magicSystem = GetComponent<DarkWizardMagicSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        castSpellTimer += Time.deltaTime;

        if (Vector3.Distance(agent.transform.position, target.position) <= visionRadius)
        {
            Attack();
        } else
        {
            Patrol();
        }

        MovementReaction();
    }

    void Attack()
    {
        if (castSpellTimer - lastSpellTimer >= castSpellDelay)
        {
            magicSystem.CastFireBall();
            lastSpellTimer = castSpellTimer;
        }
    }

    void Patrol()
    {

    }

    void MovementReaction()
    {

    }

}
