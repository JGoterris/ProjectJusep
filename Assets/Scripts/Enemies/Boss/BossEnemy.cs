using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossEnemy : MonoBehaviour
{
    public Transform target;
    public Transform[] spawnPoints; // Used to spawn enemies into the arena

    // Not yet required
    //public GameObject phantomSphere;

    public GameObject p_goblin;
    public GameObject p_darkWizard;

    private BossHealthSystem healthSystem;
    private Shield shield;

    public int numberOfEnemiesDuringRounds = 0;

    private int enemiesOnArena;
    private bool onPhase;
    private bool spawnTriggerEnemy;

    // When triggered, shields down
    private bool triggered;

    // Timers


    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GetComponent<BossHealthSystem>();
        shield = GetComponent<Shield>();
        enemiesOnArena = -1;
        onPhase = false;
        spawnTriggerEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 2 rounds

        if (!onPhase)
        {
            // Not killed
            onPhase = true;
            for (int i = 0; i < numberOfEnemiesDuringRounds; i++)
            {
                SpawnAndSetTarget<SpawnedEnemy>(spawnPoints[i % 9], p_goblin);
                if (enemiesOnArena == -1)
                    enemiesOnArena = 0;
                enemiesOnArena++;
            }
        }

        if (enemiesOnArena == 0 && !spawnTriggerEnemy)
        {
            SpawnAndSetTarget<TriggerEnemy>(spawnPoints[Random.Range(0, 8)], p_darkWizard);
            spawnTriggerEnemy = true;
        }

        if (triggered)
        {
            // Shields down
            shield.Disable();
        }
        
    }

    // Returns the spawned gameObject
    GameObject Spawn<T>(Transform swp, GameObject type) where T : Component, ITriggers
    {
        GameObject spawned = Instantiate(type, swp.position, Quaternion.identity);
        T component = spawned.AddComponent<T>() as T;
        component.Trigger(this);

        return spawned;
    }

    void SpawnAndSetTarget<T>(Transform swp, GameObject type) where T : Component, ITriggers
    {
        GameObject spawned = Spawn<T>(swp, type);
        Component[] c_arr = spawned.GetComponents(typeof(ITargeteable));
        ITargeteable targeteable = c_arr[0] as ITargeteable;
        targeteable.SetTarget(target);
    }

    public void Decrease()
    {
        enemiesOnArena--;
        Debug.Log("Enemies on arena: " + enemiesOnArena);
    }

    public void Trigger()
    {
        triggered = true;
    }

    public void Kill()
    {
        Object.Destroy(this.gameObject);
    }

    public void Restore()
    {
        // Restore default values of the boss
        enemiesOnArena = 0;
        onPhase = false;
        spawnTriggerEnemy = false;
    }
}
