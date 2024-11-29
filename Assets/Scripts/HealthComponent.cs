using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    private void Awake(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Destroy(this.gameObject);
        }
    }
}
