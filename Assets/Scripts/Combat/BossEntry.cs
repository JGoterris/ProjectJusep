using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntry : DoorAction
{
    // Components for actions
    AudioSource source;
    BoxCollider collider;
    
    public BossSpawner spawner;
    public Transform teleportPoint;
    public GameObject target;

    CharacterController controller;

    void Start()
    {
        source = GetComponent<AudioSource>();
        collider = GetComponent<BoxCollider>();
        controller = target.GetComponent<CharacterController>();
    }

    public override void DoAction()
    {
        collider.isTrigger = false;
        source.Play();

        controller.enabled = false;
        target.transform.position = teleportPoint.position;
        controller.enabled = true;

        spawner.SetTarget(target.transform);

    }
}
