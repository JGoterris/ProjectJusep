using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossEnemy : MonoBehaviour
{
    public Transform[] spawnPoints; // Used to spawn enemies into the arena
    public GameObject phantomSphere;

    public GameObject p_goblin;
    public GameObject p_darkWizard;

    private BossHealthSystem healthSystem;

    public int numberOfEnemiesDuringRounds;

    private int enemiesOnArena;
    private bool triggered;




    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GetComponent<BossHealthSystem>();
        enemiesOnArena = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 2 rounds
        // 1. Enemies spawn
        // 2. When all enemies are destroyed, a trigger enemy appears

        
    }

    void Spawn<T>(Transform swp, GameObject type) where T : Component, ITriggers
    {
        GameObject spawned = Instantiate(type, swp.position, Quaternion.identity);
        T component = spawned.AddComponent<T>() as T;
        component.Trigger(this);
        
    }

    public void Decrease()
    {
        enemiesOnArena--;
    }

    public void Trigger()
    {
        triggered = true;
    }

    public void Kill()
    {

    }
}
