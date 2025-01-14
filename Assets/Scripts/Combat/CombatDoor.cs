using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDoor : MonoBehaviour
{

    private Collider collider;

    public CombatAction action;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void LockDoor()
    {
        collider.isTrigger = false;
    }

    public void UnlockDoor()
    {
        collider.isTrigger = true;

        if (action != null)
        {
            action.DoAction();
        }
    }
}
