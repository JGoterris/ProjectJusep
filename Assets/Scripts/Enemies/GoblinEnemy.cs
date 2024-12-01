using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinEnemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Quaternion angulo;
    public float grado;
    private Animation ani;
    public GameObject target;
    private bool atacando = false;
    private NavMeshAgent agent;
    private bool walkpointSet;
    private Vector3 destPoint;
    [SerializeField] private float walkpointRange;
    void Start()
    {
        ani = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        walkpointSet = false;
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
            /* cronometro += Time.deltaTime;
            if(cronometro >= 4){
                rutina = Random.Range(0, 2);
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch(rutina){
                case 0:
                    ani.Play("combat_idle");
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                    ani.Play("walk");
                    break;
            } */
        } else{
            if(Vector3.Distance(transform.position, target.transform.position) > 2 && !atacando){
                agent.isStopped = true;
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                ani.Stop("walk");
                ani.Play("run");
                transform.Translate(Vector3.forward * 3 * Time.deltaTime);
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
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        finAtaque();
    }

    private void finAtaque(){
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
}
