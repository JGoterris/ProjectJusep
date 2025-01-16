using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSpawner : MonoBehaviour, ITriggerReceiver, ITargeteable
{

    // Boss Components
    public BossShield shield;

    public Transform target;
    public Transform[] spawnPoints; // Used to spawn enemies into the arena
    public GameObject spawnEffect; // Prefab that contains the spawn effect
    public bool enableSpawnEffect;

    // Not yet required
    //public GameObject phantomSphere;

    public GameObject p_goblin;
    public GameObject p_darkWizard;

    public int numberOfGoblins;
    public int numberOfWizards;

    private int enemiesOnArena;
    private bool onPhase;
    private bool shieldsUp;

    // When triggered, shields down
    //private bool triggered;

    // Timers


    // Start is called before the first frame update
    void Start()
    {
        enemiesOnArena = 0;
        onPhase = false;
        shieldsUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Look at the player
        if (target != null)
        {
            transform.LookAt(target);

            if (!onPhase)
            {
                // Not killed
                onPhase = true;
                StartCoroutine(SpawnEnemies());

            }

            if (enemiesOnArena == 0 && onPhase == true && shieldsUp == true)
            {
                //Debug.Log("Shield Down");
                shield.Down();
                shieldsUp = false;
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        int i = 0;
        for (int j = 0; j < numberOfGoblins; j++)
        {
            SpawnAndSetTarget<SpawnedEnemy>(spawnPoints[i % 9], p_goblin);
            enemiesOnArena++;
            i++;
            yield return new WaitForSeconds(1f);
        }

        for (int j = 0; j < numberOfWizards; j++)
        {
            SpawnAndSetTarget<SpawnedEnemy>(spawnPoints[i % 9], p_darkWizard);
            enemiesOnArena++;
            i++;
            yield return new WaitForSeconds(1f);
        }
        yield break;
    }

    public GameObject Spawn<T>(Transform swp, GameObject type) where T : Component, ITriggers
    {
        if (spawnEffect != null && enableSpawnEffect)
        {
            SpawnEffect(swp.position);
        }

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

    void SpawnEffect(Vector3 spawnPoint)
    {
        Instantiate(spawnEffect, spawnPoint, Quaternion.identity);
    }

    public void Decrease()
    {
        enemiesOnArena--;
        //Debug.Log("Enemies on arena: " + enemiesOnArena);
    }

    public void NextRound()
    {
        shield.Up();
        shieldsUp = true;
        onPhase = false;
        enemiesOnArena = 0;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
