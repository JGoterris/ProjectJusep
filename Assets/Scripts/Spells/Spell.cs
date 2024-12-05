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
    private float spellSpeed;
    private bool exploded;
    

    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;
        
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        spellSpeed = SpellToCast.Speed;

        exploded = false;

        Destroy(this.gameObject, SpellToCast.Lifetime);
    }
    private void Update(){
        if(spellSpeed > 0)
            transform.Translate(Vector3.forward * spellSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        spellSpeed = 0;
        if(other.gameObject.CompareTag("Enemy")){
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            ParticleSystem enemyBlood = other.GetComponent<ParticleSystem>();
            Slowable enemyController = other.GetComponent<GoblinEnemy>();
            if(enemyBlood != null)
                enemyBlood.Play();
            if(this.tag == "IceSpell"){
                enemyController.SlowDown(1, 5);
            }
            enemyHealth.TakeDamage(SpellToCast.DamageAmount);
        }

        if(!exploded){
            Explode();
            exploded = true;
        }
    }

    private void Explode(){
        if (SpellCollisionSound != null)
        {
            SpellCollisionSound.Play();
        }
        SpellCollisionParticleSystem.Play();

        if (SpellDestroyParticleSystemsOnCollision != null)
        {
            foreach (ParticleSystem p in SpellDestroyParticleSystemsOnCollision)
            {
                GameObject.Destroy(p);
            }
        }
    }
}
