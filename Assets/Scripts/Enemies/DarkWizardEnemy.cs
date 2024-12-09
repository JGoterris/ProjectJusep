using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkWizardEnemy : MonoBehaviour, Slowable
{
    private Animator animator;
    private NavMeshAgent agent;
    private DarkWizardMagicSystem magicSystem;

    // Modifiable variables (on engine)
    public float castSpellDelay = 2.5f; // Seconds
    public Transform target;
    
    // Speed and distance
    public float patrolSpeed = 2;
    public float chaseSpeed = 4;
    public float scareSpeed = 7;
    public float elevationDistance = 6;
    public float elevationSpeed = 1;
    public float repositionDuration = 1;

    // Perception variables
    public float visionRadius = 8;
    public float scareRadius = 2;
    public float combatRadius = 6;

    // Attack variables
    private float castSpellTimer = 0;

    // Patrol variables
    public float patrolSegmentTime = 2;
    // Slowed variables
    private float slowedDuration;

    // Scare variables
    public float scareDuration = 2;

    // State variables
    private bool m_hostile = false;
    private bool m_scared = false;
    private bool m_reposition = false;
    private bool m_slowed = false;


    // Temporal variables
    float distance;
    float scareTimer = 0;
    float repositionTimer = 0;
    float patrolTimer = 0;
    float slowedTimer = 0;

    float speedReduction = 0;

    Vector3 repositionVector;
    Vector3 patrolDestination;


    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        magicSystem = GetComponent<DarkWizardMagicSystem>();

        patrolDestination = new Vector3(0, 0, 0);
        repositionVector = new Vector3(0, 0, 0);
    }

    void Start()
    {
        animator.SetBool("patrolling", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);

            if (!m_hostile && distance < visionRadius)
            {
                m_hostile = true;
                animator.SetBool("patrolling", false);
                animator.SetBool("chasing", true);
            }

            if (m_hostile)
                HostileBehaviour();
            else
            {
                Patrol();
                patrolTimer += Time.deltaTime;
            }
        }
        else
            Patrol();
    }

    void HostileBehaviour()
    {
        castSpellTimer += Time.deltaTime;

        
        if (distance <= scareRadius)
        {
            // Run away, keep distance with the player
            m_scared = true;
            
        }

        if (!m_scared)
        {
            Orbit();
        } else
        {
            GoAway();
            scareTimer += Time.deltaTime;
        }

        if (m_reposition)
        {
            Reposition();
            repositionTimer += Time.deltaTime;
        }
        else
            Attack();

        if (m_slowed)
        {
            slowedTimer += Time.deltaTime;
            if (slowedTimer >= slowedDuration)
            {
                m_slowed = false;
                speedReduction = 0;
            }
        }
    }

    void Patrol()
    {        

        if (patrolDestination.Equals(Vector3.zero))
        {
            patrolDestination = Random.onUnitSphere * Random.Range(2, 4);
            patrolDestination.Normalize();
            transform.LookAt(patrolDestination);
        }

        if (patrolTimer <= patrolSegmentTime)
        {
            MoveEntity(patrolDestination, patrolSpeed);
        } else
        {
            patrolTimer = 0;
            patrolDestination = Vector3.zero;
        }
        
    }

    void Orbit()
    {
        transform.LookAt(target.position);
        Vector3 directorVector = new Vector3(target.position.x - transform.position.x, 0 , target.position.z - transform.position.z);
        Vector3 rotated = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * directorVector;
        rotated.Normalize();
        MoveEntity(rotated, chaseSpeed);
    }

    void GoAway()
    {
        if (scareTimer < scareDuration)
        {
            //transform.Translate(Vector3.back * scareSpeed * Time.deltaTime);
            transform.LookAt(target.position);
            MoveEntity(Vector3.back, scareSpeed);
        } else
        {
            m_scared = false;
            scareTimer = 0;
        }
    }

    void Attack()
    {
        if (castSpellTimer >= castSpellDelay)
        {
            

            transform.LookAt(target.position);
            // Calculate rotation
            Vector3 directorVector = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(directorVector);
            Physics.Raycast(magicSystem.GetCastPoint().position, directorVector, out RaycastHit hit, Mathf.Infinity);

            if (hit.distance > magicSystem.GetSpell().SpellToCast.SpellRadius)
            {
                animator.SetBool("attacking", true);
                animator.SetBool("chasing", false);

                magicSystem.CastSpell(rot);
                castSpellTimer = 0;

                StartCoroutine(WaitForAttackToEnd());
            }
            m_reposition = true;
        }
    }

    IEnumerator WaitForAttackToEnd()
    {
        yield return new WaitForSeconds(0.25f);
        FinAtaque();
    }

    void FinAtaque()
    {
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", true);
    }

    void Reposition()
    {
        if (repositionVector == Vector3.zero)
        {
            repositionVector = Random.insideUnitCircle * Random.Range(2, 3);
            repositionVector.Normalize();
        }

        if (repositionTimer <= repositionDuration)
        {
            //transform.Translate(repositionVector * chaseSpeed * Time.deltaTime);
            MoveEntity(repositionVector, chaseSpeed);
        } else
        {
            m_reposition = false;
            repositionTimer = 0;
            repositionVector = Vector3.zero;
        }
    }

    void MoveEntity(Vector3 destination, float speed)
    {
        Physics.Raycast(transform.position, destination, out RaycastHit hit, Mathf.Infinity);

        if (hit.distance > 0)
        {
            transform.Translate((speed - speedReduction) * Time.deltaTime * destination);
        }
    }

    public void SlowDown(float downS, float duration)
    {
        //Debug.Log("Slowed");
        m_slowed = true;
        slowedTimer = 0;
        slowedDuration = duration;
        speedReduction = downS;
    }
}
