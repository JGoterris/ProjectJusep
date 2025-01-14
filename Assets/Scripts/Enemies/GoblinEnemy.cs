using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour, ISlowable, IDeath, ITargeteable
{
    public Quaternion angulo;
    public float grado;
    private Animation ani;
    private GameObject target;
    private bool atacando = false;
    private NavMeshAgent agent;
    private GoblinDeaths goblinDeaths;
    [SerializeField] private float agentSpeed = 4;
    private bool walkpointSet;
    private Vector3 destPoint;
    private float downSpeed;
    private float slowCronometro;
    private float slowDuration;
    private bool slowed;
    private bool died = false;
    private AudioSource audioSource;
    [SerializeField] private float walkpointRange;
    public AudioClip[] audioClips;
    [SerializeField] private LayerMask obstacleLayer;
    void Start()
    {
        ani = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        target = GameObject.Find("Player");
        goblinDeaths = GameObject.Find("EventSystem").GetComponent<GoblinDeaths>();
        walkpointSet = false;
        downSpeed = 0;
        slowDuration = 0;
        agent.speed = agentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!died)
            Comportamiento_Enemigo();
    }

    public void Comportamiento_Enemigo(){
        if(Vector3.Distance(transform.position, target.transform.position) > 20){
            agent.isStopped = false;
            ani.Stop("run");
            Patrol();
            ani.Play("walk");
        } else{
            if(Vector3.Distance(transform.position, target.transform.position) > 2 && !atacando){
                agent.isStopped = true;
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                transform.Translate(Vector3.forward * (3-downSpeed) * Time.deltaTime);
                ani.Stop("walk");
                ani.Play("run");
            } else if (!atacando){
                audioSource.clip = audioClips[0];
                audioSource.Play();
                ani.Stop("walk");
                ani.Stop("run");
                ani.Play("attack3");
                atacando = true;
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                StartCoroutine(WaitForAnimationToEnd(ani["attack3"].length));
            }
        }
        if(slowed){
            slowCronometro += Time.deltaTime;
            if(slowCronometro >= slowDuration){
                downSpeed = 0;
                slowCronometro = 0;
                slowed = false;
                agent.speed = agentSpeed;
            }
        }
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        FinAtaque();
    }

    private void FinAtaque(){
        atacando = false;
        ani.Stop("attack3");
    }

    void Patrol(){
        if(!walkpointSet) SearchForDest();
        if(walkpointSet){
            agent.SetDestination(destPoint);
        }
        if(Vector3.Distance(transform.position, destPoint) < 10){
            walkpointSet = false;
        }
    }

    void SearchForDest(){
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);
        destPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(destPoint, Vector3.down)){
            walkpointSet = true;
        }
    }

    public void SlowDown(float downS, float duration){
        downSpeed = downS;
        slowCronometro = 0;
        slowDuration = duration;
        slowed = true;
        agent.speed -= downSpeed;
    }

    private System.Collections.IEnumerator WaitForAnimationToDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
    
    public void die(){
        if(!died) {
            died = true;
            ani.Stop("walk");
            ani.Stop("run");
            ani.Stop("attack3");
            if (ani["death"] != null)
            {
                ani.Play("death");
                Debug.Log("Muriendo");
            }
            else
            {
                Debug.LogError("Animación de muerte no encontrada.");
            }
            audioSource.clip = audioClips[1];
            audioSource.Play();
            goblinDeaths.goblinMuerto();
            StartCoroutine(WaitForAnimationToDestroy(ani["death"].length));
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target.gameObject;
    }
}
