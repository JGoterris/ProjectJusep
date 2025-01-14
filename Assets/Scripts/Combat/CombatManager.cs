using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour, ITriggerReceiver
{

    public int numberOfEnemies;
    int actualNumberOfEnemies;

    public CombatDoor[] doors;

    bool locked;

    // Start is called before the first frame update
    void Start()
    {
        actualNumberOfEnemies = numberOfEnemies;
        locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualNumberOfEnemies == 0)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] != null)
                {
                    doors[i].UnlockDoor();
                    doors[i] = null;
                }
            }
        }
    }

    public void Decrease()
    {
        actualNumberOfEnemies--;
    }

    public void Lock()
    {
        if (!locked)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].LockDoor();
            }
        }
    }
}
