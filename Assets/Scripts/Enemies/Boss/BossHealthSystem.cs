using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSystem : MonoBehaviour, IDamageable
{
    public int maxHealth;
    private int actualHealth; // Number of phases the boss has

    private BossEnemy boss;

    private void Start()
    {
        boss = GetComponent<BossEnemy>();
        actualHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        actualHealth -= 1;
        if (actualHealth <= 0)
        {
            boss.Kill();
        }
    }
}
