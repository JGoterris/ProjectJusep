using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthSystem : MonoBehaviour, IDamageable, IDeath
{

    // This a health system based on rounds
    // Each time the boss takes damage, a new round starts
    // The rounds variable specifies the number of rounds that the boss will play

    public int rounds;
    private int actualRound; // Number of phases the boss has

    private BossSpawner boss;

    private void Start()
    {
        boss = GetComponent<BossSpawner>();
        actualRound = rounds;
    }

    public void TakeDamage(float damage)
    {
        actualRound--;
        // Debug.Log("DamageTaken - Round decreased: " + actualRound);
        if (actualRound <= 0) {
            this.die();
        } else // else not necessary, just for clarity
        {
            boss.NextRound();
        }
    }

    public void die()
    {
        Destroy(this.gameObject);
    }
}
