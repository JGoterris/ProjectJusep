using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))] 
public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;
    [SerializeField] private AudioSource SpellCollisionSound;
    [SerializeField] private ParticleSystem SpellCollisionParticleSystem;
    [SerializeField] private ParticleSystem[] SpellDestroyParticleSystemsOnCollision;
    

    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;
        
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        Destroy(this.gameObject, SpellToCast.Lifetime);
    }
    private void Update(){
        if(SpellToCast.Speed > 0)
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        // Aplicar los efectos a lo que le demos
        //if(other.gameObject.CompareTag("Enemy")){
        //    other.gameObject.GetComponent<Enemy>().TakeDamage(SpellToCast.DamageAmount);
        //}
        if(other.gameObject.CompareTag("Enemy")){
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            enemyHealth.TakeDamage(SpellToCast.DamageAmount);
        }

        if (SpellDestroyParticleSystemsOnCollision != null)
        {
            foreach (ParticleSystem p in SpellDestroyParticleSystemsOnCollision)
            {
                GameObject.Destroy(p);
            }
        }

        if (SpellCollisionSound != null)
        {
            SpellCollisionSound.Play();
        }
        SpellCollisionParticleSystem.Play();
    }
}
