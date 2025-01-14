using System.Collections;
using UnityEngine;

public class DarkWizardEnemy : MonoBehaviour, ISlowable, IDeath, ITargeteable
{
    // Movement pivot
    private Animator animator;
    private DarkWizardMagicSystem magicSystem;

    // Modifiable variables (on engine)
    public float castSpellDelay = 3f; // Seconds
    public Transform target;

    // Speed and distance
    public float patrolSpeed = 2;
    public float chaseSpeed = 4;
    public float scareSpeed = 5;
    public float elevationDistance = 10;
    public float elevationSpeed = 1;
    public float repositionDuration = 1;

    // Perception variables
    public float visionRadius = 8;
    public float scareRadius = 3.5f;
    public float combatRadius = 7;

    // Attack variables
    private float castSpellTimer = 0;

    // Patrol variables
    public float patrolSegmentTime = 2;

    // Slowed variables
    private float slowedDuration;

    // Scare variables
    public float scareDuration = 1;

    // State variables
    private bool m_hostile = false;
    private bool m_scared = false;
    private bool m_reposition = false;
    private bool m_slowed = false;
    private bool m_dead = false;


    // Temporal variables
    float distance;

    // Timers
    float scareTimer = 0;
    float repositionTimer = 0;
    float patrolTimer = 0;
    float slowedTimer = 0;

    // Ice variables
    float speedReduction = 0;

    Vector3 repositionVector;
    Vector3 patrolDestination;

    public AudioClip[] audioClips;
    private AudioSource audioSource;


    void Awake()
    {
        animator = GetComponent<Animator>();
        magicSystem = GetComponent<DarkWizardMagicSystem>();
        audioSource = GetComponent<AudioSource>();

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
        if (target != null && !m_dead)
        {
            distance = Vector3.Distance(transform.position, target.position);

            if (!m_hostile && distance < visionRadius)
            {
                m_hostile = true;
                audioSource.clip = audioClips[0];
                audioSource.Play();
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
        }
        else if (m_scared && !m_reposition)
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
            Vector2 unitCircle = Random.insideUnitCircle * Random.Range(2, 3);
            patrolDestination.x = unitCircle.x;
            patrolDestination.z = unitCircle.y;
            patrolDestination.Normalize();
            //CustomLookAt(patrolDestination);
        }

        if (patrolTimer <= patrolSegmentTime)
        {
            MoveEntity(patrolDestination, patrolSpeed);
        }
        else
        {
            patrolTimer = 0;
            patrolDestination = Vector3.zero;
        }

    }

    void Orbit()
    {
        Vector3 directorVector = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
        Vector3 rotated = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * directorVector;
        rotated.Normalize();
        CustomLookAt(target.position);
        MoveEntity(rotated, chaseSpeed);
    }

    void GoAway()
    {
        if (scareTimer < scareDuration)
        {
            //transform.Translate(Vector3.back * scareSpeed * Time.deltaTime);
            CustomLookAt(target.position);
            MoveEntity(Vector3.back, scareSpeed);
        }
        else
        {
            m_scared = false;
            scareTimer = 0;
        }
    }

    void Attack()
    {
        if (castSpellTimer >= castSpellDelay)
        {
            CustomLookAt(new Vector3(target.position.x, 0, target.position.z));
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
            Vector2 unitCircle = Random.insideUnitCircle * Random.Range(2, 4);
            repositionVector.x = unitCircle.x;
            repositionVector.z = unitCircle.y;
            repositionVector.Normalize();
        }

        if (repositionTimer <= repositionDuration)
        {
            MoveEntity(repositionVector, patrolSpeed);

        }
        else
        {
            m_reposition = false;
            repositionTimer = 0;
            repositionVector = Vector3.zero;
        }
    }

    void MoveEntity(Vector3 destination, float speed)
    {
        Physics.Raycast(new Vector3(transform.position.x, 4f, transform.position.z), destination, out RaycastHit hit, Mathf.Infinity);
        // Debug.DrawRay(transform.position, destination, Color.red, 0.1f);
        // Debug.Log("Moving: " + hit.distance + " meters\tTransform: " + transform.position);

        if (hit.distance > 5)
        {
            transform.Translate((speed - speedReduction) * Time.deltaTime * destination);
        }
    }

    // This funciton is needed to look at a position without interfering with the y-axis
    private void CustomLookAt(Vector3 destination)
    {
        transform.LookAt(new Vector3(destination.x, 0, destination.z));
    }

    public void SlowDown(float downS, float duration)
    {
        //Debug.Log("Slowed");
        m_slowed = true;
        slowedTimer = 0;
        slowedDuration = duration;
        speedReduction = downS;
    }

    public void die()
    {
        m_dead = true;
        StartCoroutine(WaitForDyingAnimation());
    }

    IEnumerator WaitForDyingAnimation()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
        animator.SetBool("dying", true);
        animator.SetBool("patrolling", false);
        animator.SetBool("attacking", false);
        animator.SetBool("chasing", false);
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
