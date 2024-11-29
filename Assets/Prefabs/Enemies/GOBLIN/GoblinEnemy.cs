using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinEnemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Quaternion angulo;
    public float grado;
    private Animation ani;
    public GameObject target;
    private bool atacando = false;
    void Start()
    {
        ani = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento_Enemigo();
    }

    public void Comportamiento_Enemigo(){
        if(Vector3.Distance(transform.position, target.transform.position) > 8){
            ani.Stop("run");
            cronometro += Time.deltaTime;
            if(cronometro >= 4){
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
            }
        } else{
            if(Vector3.Distance(transform.position, target.transform.position) > 2 && !atacando){
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

    private IEnumerator Esperar(float seconds){
        yield return new WaitForSeconds(seconds);
    }
}
