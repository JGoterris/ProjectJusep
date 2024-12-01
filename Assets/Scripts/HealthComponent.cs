using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;
    public Image healthBar = null;

    private void Awake(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if(currentHealth == 0){
            Destroy(this.gameObject);
        }
    }

    public void Health(float health){
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
