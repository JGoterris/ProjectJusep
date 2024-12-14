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
        if(other.gameObject.CompareTag("Hittable")){
            Debug.Log("Hit " + other.gameObject.name);
            Component[] slowableComponents = other.gameObject.GetComponents(typeof(ISlowable));
            Component[] damageableComponents = other.gameObject.GetComponents(typeof(IDamageable));
            IDamageable enemyHealth = damageableComponents[0] as IDamageable;
            ParticleSystem enemyBlood = other.GetComponent<ParticleSystem>();
            ISlowable enemyController = slowableComponents[0] as ISlowable;

            if (enemyBlood != null)
                enemyBlood.Play();
            if(CompareTag("IceSpell"))
            {
                enemyController.SlowDown(2, 5);
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
