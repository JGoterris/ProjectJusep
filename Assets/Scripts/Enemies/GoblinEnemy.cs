using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour, Slowable
{
    public Quaternion angulo;
    public float grado;
    private Animation ani;
    public GameObject target;
    private bool atacando = false;
    private NavMeshAgent agent;
    [SerializeField] private float agentSpeed = 4;
    private bool walkpointSet;
    private Vector3 destPoint;
    private float downSpeed;
    private float slowCronometro;
    private float slowDuration;
    private bool slowed;
    [SerializeField] private float walkpointRange;
    void Start()
    {
        ani = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        walkpointSet = false;
        downSpeed = 0;
        slowDuration = 0;
        agent.speed = agentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento_Enemigo();
    }

    public void Comportamiento_Enemigo(){
        if(Vector3.Distance(transform.position, target.transform.position) > 8){
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
                ani.Stop("walk");
                ani.Play("run");
                transform.Translate(Vector3.forward * (3-downSpeed) * Time.deltaTime);
            } else if (!atacando){
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
}
